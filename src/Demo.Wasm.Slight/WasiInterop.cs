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

    [StructLayout(LayoutKind.Explicit)]
    internal readonly struct WasiExpected<T> where T : struct
    {
        [FieldOffset(0)]
        private readonly bool _isError;

        [FieldOffset(4)]
        private readonly T _result;

        [FieldOffset(4)]
        private readonly WasiError _error;

        public readonly bool IsError => _isError;

        public readonly T? Result => _isError ? null : _result;

        public readonly WasiError? Error => _isError ? _error : null;
    }

    internal static class HttpRouterFunctions
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void New(ref WasiExpected<HttpRouter> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Get(HttpRouter self, ref WasiString route, ref WasiString handler, ref WasiExpected<HttpRouter> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Put(HttpRouter self, ref WasiString route, ref WasiString handler, ref WasiExpected<HttpRouter> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Post(HttpRouter self, ref WasiString route, ref WasiString handler, ref WasiExpected<HttpRouter> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Delete(HttpRouter self, ref WasiString route, ref WasiString handler, ref WasiExpected<HttpRouter> ret0);
    }

    internal static class HttpServerFunctions
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Serve(ref WasiString address, HttpRouter router, ref WasiExpected<HttpServer> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Stop(HttpServer self, ref WasiExpected<HttpServer> ret0);
    }
}