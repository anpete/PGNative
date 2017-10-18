// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace PGNative
{
    internal class Program
    {
        private static async Task Main()
        {
            const string connInfo = "host=localhost dbname=aspnet5-Benchmarks user=postgres password=Password1 client_encoding=utf8";
            const int concurrency = 32;
            const int timeS = 10;
            const int maxTransactions = 100000;

            var counter = 0;
            var stopping = false;

            var tasks
                = Enumerable
                    .Range(1, concurrency)
                    .Select(
                        i => Task.Run(
                            async () =>
                                {
                                    while (!stopping
                                           && Interlocked.Increment(ref counter) < maxTransactions)
                                    {
                                        var conn = IntPtr.Zero;
                                        try
                                        {
                                            conn = await Connect(connInfo);

                                            await Execute(conn);
                                        }
                                        catch
                                        {
                                        }
                                        finally
                                        {
                                            if (conn != IntPtr.Zero)
                                            {
                                                LibPQ.PQfinish(conn);
                                            }
                                        }
                                    }
                                })).ToList();

            tasks.Add(Task.Delay(TimeSpan.FromSeconds(timeS)));

            await Task.WhenAny(tasks);

            stopping = true;

            Console.WriteLine("Shutting down");

            await Task.WhenAll(tasks);

            Console.WriteLine($"Executed {counter} queries.");
        }

        private static async Task<IntPtr> Connect(string connInfo)
        {
            var conn = LibPQ.PQconnectStart(connInfo);

            if (conn == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to allocate connection handle.");
            }

            while (true)
            {
                switch (LibPQ.PQconnectPoll(conn))
                {
                    case LibPQ.PostgresPollingStatusType.PGRES_POLLING_OK:
                        return conn;
                    case LibPQ.PostgresPollingStatusType.PGRES_POLLING_FAILED:
                        throw new InvalidOperationException($"Unable to connect: {LibPQ.PQerrorMessage(conn)}");
                    case LibPQ.PostgresPollingStatusType.PGRES_POLLING_READING:
                    case LibPQ.PostgresPollingStatusType.PGRES_POLLING_WRITING:
                    case LibPQ.PostgresPollingStatusType.PGRES_POLLING_ACTIVE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                await Task.Yield();
            }
        }

        private static async Task Execute(IntPtr conn)
        {
            if (LibPQ.PQsendQuery(conn, "select id, message from fortune") != 1)
            {
                throw new InvalidOperationException($"Unable to dispatch command: {LibPQ.PQerrorMessage(conn)}");
            }

            //Console.WriteLine("Sent query.");

            while (true)
            {
                var result = LibPQ.PQgetResult(conn);

                if (result == IntPtr.Zero)
                {
                    break;
                }

                var status = LibPQ.PQresultStatus(result);
                var statusName = Marshal.PtrToStringAnsi(LibPQ.PQresStatus(status));

                //Console.WriteLine($"Got result. ({statusName})");

                switch (status)
                {
                    case LibPQ.ExecStatusType.PGRES_TUPLES_OK:
                    {
                        for (var i = 0; i < 12; i++)
                        {
                            var id = Marshal.PtrToStringUTF8(LibPQ.PQgetvalue(result, i, 0));
                            var message = Marshal.PtrToStringUTF8(LibPQ.PQgetvalue(result, i, 1));

                            //Console.WriteLine($"Read row: [{id}, {message}]");
                        }

                        LibPQ.PQclear(result);

                        break;
                    }
                    case LibPQ.ExecStatusType.PGRES_EMPTY_QUERY:
                    case LibPQ.ExecStatusType.PGRES_COMMAND_OK:
                    case LibPQ.ExecStatusType.PGRES_COPY_OUT:
                    case LibPQ.ExecStatusType.PGRES_COPY_IN:
                    case LibPQ.ExecStatusType.PGRES_BAD_RESPONSE:
                    case LibPQ.ExecStatusType.PGRES_NONFATAL_ERROR:
                    case LibPQ.ExecStatusType.PGRES_FATAL_ERROR:
                    case LibPQ.ExecStatusType.PGRES_COPY_BOTH:
                    case LibPQ.ExecStatusType.PGRES_SINGLE_TUPLE:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                await Task.Yield();
            }
        }
    }
}
