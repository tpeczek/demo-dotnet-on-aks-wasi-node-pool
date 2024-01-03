#include <string.h>
#include <wasm/driver.h>
#include "http-handler.h"

int mono_runtime_loaded = 0;
MonoMethod* handle_request_method;

void mono_wasm_invoke_method_ref(MonoMethod* method, MonoObject** this_arg_in, void* params[], MonoObject** _out_exc, MonoObject** out_result);

void http_handler_handle_http(http_handler_request_t* req, http_handler_expected_response_error_t* ret0) {
    if (!mono_runtime_loaded) {
        mono_wasm_load_runtime("", 0);

        mono_runtime_loaded = 1;
    }

    if (!handle_request_method)
    {
        handle_request_method = lookup_dotnet_method("Demo.Wasm.Slight", "Demo.Wasm.Slight", "HttpServer", "HandleRequest", -1);
    }

    void* method_params[] = { req, ret0 };
    MonoObject* exception;
    MonoObject* result;
    mono_wasm_invoke_method_ref(handle_request_method, NULL, method_params, &exception, &result);

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