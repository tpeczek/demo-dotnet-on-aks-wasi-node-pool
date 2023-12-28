using System.Runtime.InteropServices;

namespace Demo.Wasm.Slight
{
    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct HttpRouter
    {
        private readonly uint _idx;

        public readonly uint Index => _idx;
    }
}
