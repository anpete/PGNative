// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 4014

namespace LibPQ.Bench
{
    internal class Program
    {
        private const string ConnInfo = "host=localhost dbname=aspnet5-Benchmarks user=postgres password=Password1 client_encoding=utf8";
        private const string StmtName = "q0";

        private const int NumTasks = 32;

        private static int _counter;
        private static bool _stopping;

        private static async Task Main()
        {
            var lastDisplay = DateTime.UtcNow;

            var tasks
                = Enumerable
                    .Range(1, NumTasks)
                    .Select(_ => Task.Factory.StartNew(DoWork, TaskCreationOptions.LongRunning))
                    .ToList();

            Task.Run(
                async () =>
                    {
                        while (!_stopping)
                        {
                            await Task.Delay(1000);

                            var now = DateTime.UtcNow;
                            var tps = (int)(_counter / (now - lastDisplay).TotalSeconds);

                            Console.Write($"{tasks.Count} Threads, {tps} tps");

                            Console.CursorLeft = 0;

                            lastDisplay = now;

                            _counter = 0;
                        }
                    });

            Task.Run(
                () =>
                    {
                        Console.ReadLine();

                        _stopping = true;
                    });

            await Task.WhenAll(tasks);
        }

        private static void DoWork()
        {
            var conn = IntPtr.Zero;

            try
            {
                conn = Connect();

                while (!_stopping)
                {
                    Interlocked.Increment(ref _counter);

                    var queryResult = IntPtr.Zero;

                    try
                    {
                        queryResult = PGNative.LibPQ.PQexecPrepared(conn, StmtName, 0, IntPtr.Zero, 0, 0, 0);

                        if (queryResult == IntPtr.Zero
                            || PGNative.LibPQ.PQresultStatus(queryResult) != PGNative.LibPQ.ExecStatusType.PGRES_TUPLES_OK)
                        {
                            throw new InvalidOperationException("Unable to execute query!");
                        }

                        for (var i = 0; i < PGNative.LibPQ.PQntuples(queryResult); i++)
                        {
                            var id = Marshal.PtrToStringUTF8(PGNative.LibPQ.PQgetvalue(queryResult, i, 0));
                            var message = Marshal.PtrToStringUTF8(PGNative.LibPQ.PQgetvalue(queryResult, i, 1));
                        }
                    }
                    finally
                    {
                        if (queryResult != IntPtr.Zero)
                        {
                            PGNative.LibPQ.PQclear(queryResult);
                        }
                    }
                }
            }
            finally
            {
                if (conn != IntPtr.Zero)
                {
                    PGNative.LibPQ.PQfinish(conn);
                }
            }
        }

        private static IntPtr Connect()
        {
            var conn = PGNative.LibPQ.PQconnectdb(ConnInfo);

            if (conn == IntPtr.Zero
                || PGNative.LibPQ.PQstatus(conn) != PGNative.LibPQ.ConnStatusType.CONNECTION_OK)
            {
                throw new InvalidOperationException("Unable to connect!");
            }

            var prepareResult = IntPtr.Zero;

            try
            {
                prepareResult
                    = PGNative.LibPQ.PQprepare(conn, StmtName, "select id, message from fortune", 0, null);

                if (prepareResult == IntPtr.Zero
                    || PGNative.LibPQ.PQresultStatus(prepareResult) != PGNative.LibPQ.ExecStatusType.PGRES_COMMAND_OK)
                {
                    throw new InvalidOperationException("Unable to prepare query!");
                }
            }
            finally
            {
                if (prepareResult != IntPtr.Zero)
                {
                    PGNative.LibPQ.PQclear(prepareResult);
                }
            }

            return conn;
        }
    }
}
