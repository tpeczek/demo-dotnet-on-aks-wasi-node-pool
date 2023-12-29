#include <string.h>
#include <mono-wasi/driver.h>
#include "http.h"
#include "http-handler.h"

void attach_internal_calls() {
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::New", http_router_new);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Get", http_router_get);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Put", http_router_put);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Post", http_router_post);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpRouterFunctions::Delete", http_router_delete);

    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpServerFunctions::Serve", http_server_serve);
    mono_add_internal_call("Demo.Wasm.Slight.Wasi.HttpServerFunctions::Stop", http_server_stop);
}

void http_handler_handle_http(http_handler_request_t* req, http_handler_expected_response_error_t* ret0) {
    MonoMethod* method = lookup_dotnet_method("Demo.Wasm.Slight", "Demo.Wasm.Slight", "HttpServer", "HandleRequest", -1);
    void* method_params[] = { req, ret0 };
    MonoObject* exception;
    MonoObject* result = mono_wasm_invoke_method(method, NULL, method_params, &exception);

    if (exception) {
        char* exception_string_utf8 = mono_string_to_utf8((MonoString*)result);
        *ret0 = (http_handler_expected_response_error_t){
            .is_err = 1,
            .val = {
                .err = {
                    .tag = HTTP_HANDLER_ERROR_ERROR_WITH_DESCRIPTION,
                    .val = {
                        .error_with_description = {
                            .ptr = exception_string_utf8,
                            .len = strlen(exception_string_utf8)
                        }
                    }
                }
            }
        };
    }
}