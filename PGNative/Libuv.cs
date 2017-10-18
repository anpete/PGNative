// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace PGNative
{
    public class Libuv
    {
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_loop_init(UvLoopHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_loop_close(IntPtr a0);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_run(UvLoopHandle handle, int mode);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern void uv_stop(UvLoopHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern void uv_ref(UvHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern void uv_unref(UvHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_fileno(UvHandle handle, ref IntPtr socket);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern void uv_close(IntPtr handle, uv_close_cb close_cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_async_init(UvLoopHandle loop, UvAsyncHandle handle, uv_async_cb cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_async_send(UvAsyncHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl, EntryPoint = "uv_async_send")]
//        public static extern int uv_unsafe_async_send(IntPtr handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_tcp_init(UvLoopHandle loop, UvTcpHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_tcp_bind(UvTcpHandle handle, ref SockAddr addr, int flags);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_tcp_open(UvTcpHandle handle, IntPtr hSocket);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_tcp_nodelay(UvTcpHandle handle, int enable);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_pipe_init(UvLoopHandle loop, UvPipeHandle handle, int ipc);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_pipe_bind(UvPipeHandle loop, string name);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_pipe_open(UvPipeHandle handle, IntPtr hSocket);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_listen(UvStreamHandle handle, int backlog, uv_connection_cb cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_accept(UvStreamHandle server, UvStreamHandle client);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
//        public static extern void uv_pipe_connect(UvConnectRequest req, UvPipeHandle handle, string name, uv_connect_cb cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_pipe_pending_count(UvPipeHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_read_start(UvStreamHandle handle, uv_alloc_cb alloc_cb, uv_read_cb read_cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_read_stop(UvStreamHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_try_write(UvStreamHandle handle, uv_buf_t[] bufs, int nbufs);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern unsafe int uv_write(UvRequest req, UvStreamHandle handle, uv_buf_t* bufs, int nbufs, uv_write_cb cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern unsafe int uv_write2(UvRequest req, UvStreamHandle handle, uv_buf_t* bufs, int nbufs, UvStreamHandle sendHandle, uv_write_cb cb);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern IntPtr uv_err_name(int err);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern IntPtr uv_strerror(int err);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_loop_size();
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_handle_size(HandleType handleType);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_req_size(RequestType reqType);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_ip4_addr(string ip, int port, out SockAddr addr);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_ip6_addr(string ip, int port, out SockAddr addr);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_tcp_getsockname(UvTcpHandle handle, out SockAddr name, ref int namelen);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_tcp_getpeername(UvTcpHandle handle, out SockAddr name, ref int namelen);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_walk(UvLoopHandle loop, uv_walk_cb walk_cb, IntPtr arg);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_timer_init(UvLoopHandle loop, UvTimerHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_timer_start(UvTimerHandle handle, uv_timer_cb cb, long timeout, long repeat);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern int uv_timer_stop(UvTimerHandle handle);
//
//        [DllImport("libuv", CallingConvention = CallingConvention.Cdecl)]
//        public static extern long uv_now(UvLoopHandle loop);
//
//        [DllImport("WS2_32.dll", CallingConvention = CallingConvention.Winapi)]
//        public static extern unsafe int WSAIoctl(
//            IntPtr socket,
//            int dwIoControlCode,
//            int* lpvInBuffer,
//            uint cbInBuffer,
//            int* lpvOutBuffer,
//            int cbOutBuffer,
//            out uint lpcbBytesReturned,
//            IntPtr lpOverlapped,
//            IntPtr lpCompletionRoutine
//        );
//
//        [DllImport("WS2_32.dll", CallingConvention = CallingConvention.Winapi)]
//        public static extern int WSAGetLastError();
    }
}
