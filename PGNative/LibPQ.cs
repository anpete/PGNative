// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace PGNative
{
    public static class LibPQ
    {
        public enum PostgresPollingStatusType
        {
            PGRES_POLLING_FAILED = 0,
            PGRES_POLLING_READING,
            PGRES_POLLING_WRITING,
            PGRES_POLLING_OK,
            PGRES_POLLING_ACTIVE
        }

        public enum ExecStatusType
        {
            PGRES_EMPTY_QUERY = 0,
            PGRES_COMMAND_OK,
            PGRES_TUPLES_OK,
            PGRES_COPY_OUT,
            PGRES_COPY_IN,
            PGRES_BAD_RESPONSE,
            PGRES_NONFATAL_ERROR,
            PGRES_FATAL_ERROR,
            PGRES_COPY_BOTH,
            PGRES_SINGLE_TUPLE
        }

        public enum ConnStatusType
        {
            CONNECTION_OK,
            CONNECTION_BAD,
            CONNECTION_STARTED,
            CONNECTION_MADE,
            CONNECTION_AWAITING_RESPONSE,
            CONNECTION_AUTH_OK,
            CONNECTION_SETENV,
            CONNECTION_SSL_STARTUP,
            CONNECTION_NEEDED,
            CONNECTION_CHECK_WRITABLE,
            CONNECTION_CONSUME
        }

        [DllImport("libpq", EntryPoint = "PQconnectStart")]
        public static extern IntPtr PQconnectStart([In] [MarshalAs(UnmanagedType.LPStr)] string conninfo);

        [DllImport("libpq", EntryPoint = "PQconnectPoll")]
        public static extern PostgresPollingStatusType PQconnectPoll(IntPtr conn);

        [DllImport("libpq", EntryPoint = "PQconnectdb")]
        public static extern IntPtr PQconnectdb([In] [MarshalAs(UnmanagedType.LPStr)] string conninfo);

        [DllImport("libpq", EntryPoint = "PQstatus")]
        public static extern ConnStatusType PQstatus(IntPtr conn);

        [DllImport("libpq", EntryPoint = "PQfinish")]
        public static extern void PQfinish(IntPtr conn);

        [DllImport("libpq", EntryPoint = "PQsendQuery")]
        public static extern int PQsendQuery(IntPtr conn, [In] [MarshalAs(UnmanagedType.LPStr)] string query);

        [DllImport("libpq", EntryPoint = "PQgetResult")]
        public static extern IntPtr PQgetResult(IntPtr conn);

        [DllImport("libpq", EntryPoint = "PQclear")]
        public static extern void PQclear(IntPtr res);

        [DllImport("libpq", EntryPoint = "PQresultStatus")]
        public static extern ExecStatusType PQresultStatus(IntPtr res);

        [DllImport("libpq", EntryPoint = "PQresStatus")]
        public static extern IntPtr PQresStatus(ExecStatusType status);

        [DllImport("libpq", EntryPoint = "PQgetvalue")]
        public static extern IntPtr PQgetvalue(IntPtr res, int tup_num, int field_num);

        [DllImport("libpq", EntryPoint = "PQerrorMessage")]
        public static extern string PQerrorMessage(IntPtr conn);

        [DllImport("libpq", EntryPoint = "PQprepare")]
        public static extern IntPtr PQprepare(
            IntPtr conn,
            [In] [MarshalAs(UnmanagedType.LPStr)] string stmtName,
            [In] [MarshalAs(UnmanagedType.LPStr)] string query,
            int nParams,
            uint[] paramTypes);

        [DllImport("libpq", EntryPoint = "PQexecPrepared")]
        public static extern IntPtr PQexecPrepared(
            IntPtr conn,
            [In] [MarshalAs(UnmanagedType.LPStr)] string stmtName,
            int nParams,
            IntPtr paramValues,
            int paramLengths,
            int paramFormats,
            int resultFormat);

        [DllImport("libpq", EntryPoint = "PQntuples")]
        public static extern int PQntuples(IntPtr res);
    }
}
