using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Demo.Wasm.Slight.Wasi
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct WasiString
    {
        private readonly nint _utf8Ptr;
        private readonly int _utf8Length;

        internal WasiString(nint ptr, int length)
        {
            _utf8Ptr = ptr;
            _utf8Length = length;
        }

        public override string ToString() => Marshal.PtrToStringUTF8(_utf8Ptr, _utf8Length);

        public static unsafe WasiString FromString(string value)
        {
            var exactByteCount = checked(Encoding.UTF8.GetByteCount(value));
            var mem = Marshal.AllocHGlobal(exactByteCount);
            var buffer = new Span<byte>((void*)mem, exactByteCount);
            int byteCount = Encoding.UTF8.GetBytes(value, buffer);
            return new WasiString(mem, byteCount);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    internal readonly struct WasiError
    {
        [FieldOffset(0)]
        private readonly byte _tag;

        [FieldOffset(4)]
        private readonly WasiString _errorWithDescription;

        public WasiString ErrorWithDescription => _errorWithDescription;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct WasiHttpRouter
    {
        private readonly uint _idx;

        public readonly uint Index => _idx;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal readonly struct WasiHttpRouterOrError
    {
        [FieldOffset(0)]
        private readonly bool _isError;

        [FieldOffset(4)]
        private readonly WasiHttpRouter _router;

        [FieldOffset(4)]
        private readonly WasiError _error;

        public readonly bool IsError => _isError;

        public readonly WasiHttpRouter? Router => _isError ? null : _router;

        public readonly WasiError? Error => _isError ? _error : null;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct WasiHttpServer
    {
        private readonly uint _idx;

        public readonly uint Index => _idx;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal readonly struct WasiHttpServerOrError
    {
        [FieldOffset(0)]
        private readonly bool _isError;

        [FieldOffset(4)]
        private readonly WasiHttpServer _server;

        [FieldOffset(4)]
        private readonly WasiError _error;

        public readonly bool IsError => _isError;

        public readonly WasiHttpServer? Server => _isError ? null : _server;

        public readonly WasiError? Error => _isError ? _error : null;
    }

    internal static class HttpRouterFunctions
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_router_new(ref WasiHttpRouterOrError ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_router_get(WasiHttpRouter self, ref WasiString route, ref WasiString handler, ref WasiHttpRouterOrError ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_router_put(WasiHttpRouter self, ref WasiString route, ref WasiString handler, ref WasiHttpRouterOrError ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_router_post(WasiHttpRouter self, ref WasiString route, ref WasiString handler, ref WasiHttpRouterOrError ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_router_delete(WasiHttpRouter self, ref WasiString route, ref WasiString handler, ref WasiHttpRouterOrError ret0);
    }

    internal static class HttpServerFunctions
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_server_serve(ref WasiString address, WasiHttpRouter router, ref WasiHttpServerOrError ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void http_server_stop(WasiHttpServer self, ref WasiHttpServerOrError ret0);
    }
}