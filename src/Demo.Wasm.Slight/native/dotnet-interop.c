#include <mono-wasi/driver.h>
#include "http.h"

void attach_internal_calls() {
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::New", http_router_new);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Get", http_router_get);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Put", http_router_put);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Post", http_router_post);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Delete", http_router_delete);

    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpServerFunctions::Serve", http_server_serve);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpServerFunctions::Stop", http_server_stop);
}