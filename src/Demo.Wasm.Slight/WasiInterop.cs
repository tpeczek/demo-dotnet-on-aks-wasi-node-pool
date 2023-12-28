using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Demo.Wasm.Slight.Wasi
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly unsafe struct WasiBuffer
    {
        private readonly nint _ptr;
        private readonly int _len;

        public readonly unsafe ReadOnlySpan<byte> AsSpan() => new ReadOnlySpan<byte>((void*)_ptr, _len);

        private WasiBuffer(nint ptr, int len)
        {
            _ptr = ptr;
            _len = len;
        }

        public static unsafe WasiBuffer FromString(string value)
        {
            var exactByteCount = checked(Encoding.UTF8.GetByteCount(value));
            var mem = Marshal.AllocHGlobal(exactByteCount);
            var buffer = new Span<byte>((void*)mem, exactByteCount);
            int byteCount = Encoding.UTF8.GetBytes(value, buffer);
            return new WasiBuffer(mem, byteCount);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct WasiString
    {
        private readonly nint _utf8Ptr;
        private readonly int _utf8Length;

        private WasiString(nint utf8Ptr, int utf8Length)
        {
            _utf8Ptr = utf8Ptr;
            _utf8Length = utf8Length;
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

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct WasiStringKeyValuePairList
    {
        private readonly nint _ptr;
        private readonly int _len;

        public int Length => _len;

        private WasiStringKeyValuePairList(nint ptr, int len)
        {
            _ptr = ptr;
            _len = len;
        }

        public unsafe KeyValuePair<string, string>[] ToArray()
        {
            KeyValuePair<string, string>[] result = new KeyValuePair<string, string>[_len];

            Span<(WasiString, WasiString)> wasiStringTupleBuffer = new Span<(WasiString, WasiString)>((void*)_ptr, _len);
            for (var i = 0; i < _len; i++)
            {
                result[i] = new(wasiStringTupleBuffer[i].Item1.ToString(), wasiStringTupleBuffer[i].Item2.ToString());
            }

            return result;
        }

        internal static WasiStringKeyValuePairList FromEnumerable(IEnumerable<KeyValuePair<string, string>> value)
        {
            (WasiString, WasiString)[] wasiStringTupleArray = value.Select(x => (WasiString.FromString(x.Key), WasiString.FromString(x.Value))).ToArray();

            return new WasiStringKeyValuePairList(Marshal.UnsafeAddrOfPinnedArrayElement(wasiStringTupleArray, 0), wasiStringTupleArray.Length);
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
    internal readonly struct WasiOption<T> where T : struct
    {
        private readonly byte _isSome;
        private readonly T _value;

        public WasiOption()
        {
            _isSome = 0;
        }

        public WasiOption(T value)
        {
            _isSome = 1;
            _value = value;
        }

        public T? Value => (_isSome == 1) ? _value : default;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WasiExpected<T> where T : struct
    {
        private byte _isError;

        private T _resultOrError;

        public bool IsError => _isError != 0;

        public T? Result => (_isError == 0) ? null : _resultOrError;

        public unsafe WasiError? Error
        {
            get
            {
                if (_isError != 0)
                {
                    var errorPointer = Unsafe.AsPointer(ref _resultOrError);
                    
                    return Marshal.PtrToStructure<WasiError>((nint)errorPointer);
                }

                return null;
            }
        }

        public WasiExpected(T result)
        {
            _isError = 0;
            _resultOrError = result;
        }
    }

    internal static class HttpRouterFunctions
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void New(ref WasiExpected<uint> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Get(uint httpRouterIndex, ref WasiString route, ref WasiString handler, ref WasiExpected<uint> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Put(uint httpRouterIndex, ref WasiString route, ref WasiString handler, ref WasiExpected<uint> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Post(uint httpRouterIndex, ref WasiString route, ref WasiString handler, ref WasiExpected<uint> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Delete(uint httpRouterIndex, ref WasiString route, ref WasiString handler, ref WasiExpected<uint> ret0);
    }

    internal static class HttpServerFunctions
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Serve(ref WasiString address, uint httpRouterIndex, ref WasiExpected<uint> ret0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern unsafe void Stop(uint httpServerIndex, ref WasiExpected<uint> ret0);
    }
}