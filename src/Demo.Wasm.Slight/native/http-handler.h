#ifndef __BINDINGS_HTTP_HANDLER_H
#define __BINDINGS_HTTP_HANDLER_H
#ifdef __cplusplus
extern "C"
{
  #endif
  
  #include <stdint.h>
  #include <stdbool.h>
  
  typedef struct {
    char *ptr;
    size_t len;
  } http_handler_string_t;
  
  void http_handler_string_set(http_handler_string_t *ret, const char *s);
  void http_handler_string_dup(http_handler_string_t *ret, const char *s);
  void http_handler_string_free(http_handler_string_t *ret);
  typedef uint8_t http_handler_method_t;
  #define HTTP_HANDLER_METHOD_GET 0
  #define HTTP_HANDLER_METHOD_POST 1
  #define HTTP_HANDLER_METHOD_PUT 2
  #define HTTP_HANDLER_METHOD_DELETE 3
  #define HTTP_HANDLER_METHOD_PATCH 4
  #define HTTP_HANDLER_METHOD_HEAD 5
  #define HTTP_HANDLER_METHOD_OPTIONS 6
  typedef http_handler_string_t http_handler_uri_t;
  void http_handler_uri_free(http_handler_uri_t *ptr);
  typedef struct {
    http_handler_string_t f0;
    http_handler_string_t f1;
  } http_handler_tuple2_string_string_t;
  void http_handler_tuple2_string_string_free(http_handler_tuple2_string_string_t *ptr);
  typedef struct {
    http_handler_tuple2_string_string_t *ptr;
    size_t len;
  } http_handler_headers_t;
  void http_handler_headers_free(http_handler_headers_t *ptr);
  typedef struct {
    http_handler_tuple2_string_string_t *ptr;
    size_t len;
  } http_handler_params_t;
  void http_handler_params_free(http_handler_params_t *ptr);
  typedef struct {
    uint8_t *ptr;
    size_t len;
  } http_handler_body_t;
  void http_handler_body_free(http_handler_body_t *ptr);
  typedef struct {
    bool is_some;
    http_handler_body_t val;
  } http_handler_option_body_t;
  void http_handler_option_body_free(http_handler_option_body_t *ptr);
  typedef struct {
    http_handler_method_t method;
    http_handler_uri_t uri;
    http_handler_headers_t headers;
    http_handler_params_t params;
    http_handler_option_body_t body;
  } http_handler_request_t;
  void http_handler_request_free(http_handler_request_t *ptr);
  typedef uint16_t http_handler_http_status_t;
  typedef struct {
    bool is_some;
    http_handler_headers_t val;
  } http_handler_option_headers_t;
  void http_handler_option_headers_free(http_handler_option_headers_t *ptr);
  typedef struct {
    http_handler_http_status_t status;
    http_handler_option_headers_t headers;
    http_handler_option_body_t body;
  } http_handler_response_t;
  void http_handler_response_free(http_handler_response_t *ptr);
  typedef struct {
    uint8_t tag;
    union {
      http_handler_string_t error_with_description;
    } val;
  } http_handler_error_t;
  #define HTTP_HANDLER_ERROR_ERROR_WITH_DESCRIPTION 0
  void http_handler_error_free(http_handler_error_t *ptr);
  typedef struct {
    bool is_err;
    union {
      http_handler_response_t ok;
      http_handler_error_t err;
    } val;
  } http_handler_expected_response_error_t;
  void http_handler_expected_response_error_free(http_handler_expected_response_error_t *ptr);
  void http_handler_handle_http(http_handler_request_t *req, http_handler_expected_response_error_t *ret0);
  #ifdef __cplusplus
}
#endif
#endif
