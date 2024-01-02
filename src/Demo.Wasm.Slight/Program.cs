using System;
using Demo.Wasm.Slight;

Console.WriteLine("-- Demo.Wasm.Slight --");

Console.WriteLine("Demo.Wasm.Slight: Starting server");

HttpServer.Serve("0.0.0.0:8080");

Console.WriteLine("Demo.Wasm.Slight: Moving on");