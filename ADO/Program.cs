// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace NPGSql
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            const string connectionString = "Server=localhost;Database=aspnet5-Benchmarks;User Id=postgres;password=Password1;";
            const int concurrency = 1;
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
                                        using (var connection = new NpgsqlConnection(connectionString))
                                        {
                                            await connection.OpenAsync();

                                            using (var command = connection.CreateCommand())
                                            {
                                                command.CommandText = "select id, message from fortune";

                                                using (var reader = command.ExecuteReader())
                                                {
                                                    for (var j = 0; j < 12; j++)
                                                    {
                                                        await reader.ReadAsync();

                                                        var id = reader.GetInt32(0);
                                                        var message = reader.GetString(1);
                                                    }
                                                }
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
    }
}
