using Demo.Wasm.Slight.Wasi;
using System.Runtime.InteropServices;

namespace Demo.Wasm.Slight
{
    internal enum HttpMethod : byte
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
        PATCH = 4,
        HEAD = 5,
        OPTIONS = 6
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly struct HttpRequest
    {
        private readonly HttpMethod _method;
        private readonly WasiString _uri;
        private readonly WasiStringKeyValuePairList _headers;
        private readonly WasiStringKeyValuePairList _params;
        private readonly WasiOption<WasiBuffer> _body;

        public readonly HttpMethod Method => _method;
        
        public readonly string Uri => _uri.ToString();
        
        public readonly IReadOnlyList<KeyValuePair<string, string>> Headers => _headers.ToArray();
        
        public readonly IReadOnlyList<KeyValuePair<string, string>> Parameters => _params.ToArray();
        
        public byte[]? Body => _body.Value.HasValue ? _body.Value.Value.AsSpan().ToArray() : default;
    }
}
