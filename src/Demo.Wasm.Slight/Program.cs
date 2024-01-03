using System;
using System.Collections.Generic;

namespace Demo.Wasm.Slight
{
    class Program
    {
        [HttpHandler(HttpMethod.GET, "/hello")]
        internal static HttpResponse HandleHello(HttpRequest request)
        {
            HttpResponse response = new HttpResponse(200);
            response.SetHeaders(new[] { KeyValuePair.Create("Content-Type", "text/plain") });
            response.SetBody($"Hello from Demo.Wasm.Slight!");

            return response;
        }

        [HttpHandler(HttpMethod.GET, "/goodbye")]
        internal static HttpResponse HandleGoodbye(HttpRequest request)
        {
            HttpResponse response = new HttpResponse(200);
            response.SetHeaders(new[] { KeyValuePair.Create("Content-Type", "text/plain") });
            response.SetBody($"Goodbye from Demo.Wasm.Slight!");

            return response;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-- Demo.Wasm.Slight --");

            Console.WriteLine("Demo.Wasm.Slight: Starting server");

            HttpServer.Serve("0.0.0.0:80");

            Console.WriteLine("Demo.Wasm.Slight: Moving on");
        }
    }
}