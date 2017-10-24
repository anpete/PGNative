// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

#pragma warning disable 4014

namespace NPGSql.Bench
{
    internal class Program
    {
        private const string ConnInfo
            = "Server=localhost;Database=aspnet5-Benchmarks;User Id=postgres;password=Password1;";

        private const int NumTasks = 32;

        private static int _counter;
        private static bool _stopping;

        private static async Task Main()
        {
            var lastDisplay = DateTime.UtcNow;

            var tasks
                = Enumerable
                    .Range(1, NumTasks)
                    //.Select(_ => Task.Factory.StartNew(DoWork, TaskCreationOptions.LongRunning))
                    .Select(_ => Task.Factory.StartNew(DoWorkAsync, TaskCreationOptions.LongRunning).Unwrap())
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
            using (var connection = new NpgsqlConnection(ConnInfo))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, message from fortune";
                    command.Prepare();

                    while (!_stopping)
                    {
                        Interlocked.Increment(ref _counter);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var id = reader.GetInt32(0);
                                var message = reader.GetString(1);
                            }
                        }
                    }
                }
            }
        }

        private static async Task DoWorkAsync()
        {
            using (var connection = new NpgsqlConnection(ConnInfo))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select id, message from fortune";
                    command.Prepare();

                    while (!_stopping)
                    {
                        Interlocked.Increment(ref _counter);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var id = reader.GetInt32(0);
                                var message = reader.GetString(1);
                            }
                        }
                    }
                }
            }
        }
    }
}
