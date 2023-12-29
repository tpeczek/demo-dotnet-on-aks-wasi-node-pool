#ifndef __BINDINGS_HTTP_H
#define __BINDINGS_HTTP_H
#ifdef __cplusplus
extern "C"
{
  #endif
  
  #include <stdint.h>
  #include <stdbool.h>
  
  typedef struct {
    uint32_t idx;
  } http_router_t;
  void http_router_free(http_router_t *ptr);
  http_router_t http_router_clone(http_router_t *ptr);
  
  typedef struct {
    uint32_t idx;
  } http_server_t;
  void http_server_free(http_server_t *ptr);
  http_server_t http_server_clone(http_server_t *ptr);
  
  typedef struct {
    char *ptr;
    size_t len;
  } http_string_t;
  
  void http_string_set(http_string_t *ret, const char *s);
  void http_string_dup(http_string_t *ret, const char *s);
  void http_string_free(http_string_t *ret);
  typedef http_string_t http_uri_t;
  void http_uri_free(http_uri_t *ptr);
  typedef struct {
    uint8_t tag;
    union {
      http_string_t error_with_description;
    } val;
  } http_error_t;
  #define HTTP_ERROR_ERROR_WITH_DESCRIPTION 0
  void http_error_free(http_error_t *ptr);
  typedef struct {
    bool is_err;
    union {
      http_router_t ok;
      http_error_t err;
    } val;
  } http_expected_router_error_t;
  void http_expected_router_error_free(http_expected_router_error_t *ptr);
  typedef struct {
    bool is_err;
    union {
      http_server_t ok;
      http_error_t err;
    } val;
  } http_expected_server_error_t;
  void http_expected_server_error_free(http_expected_server_error_t *ptr);
  typedef struct {
    bool is_err;
    union {
      http_error_t err;
    } val;
  } http_expected_unit_error_t;
  void http_expected_unit_error_free(http_expected_unit_error_t *ptr);
  void http_router_new(http_expected_router_error_t *ret0);
  void http_router_new_with_base(http_uri_t *base, http_expected_router_error_t *ret0);
  void http_router_get(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0);
  void http_router_put(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0);
  void http_router_post(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0);
  void http_router_delete(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0);
  void http_server_serve(http_string_t *address, http_router_t router, http_expected_server_error_t *ret0);
  void http_server_stop(http_server_t self, http_expected_unit_error_t *ret0);
  #ifdef __cplusplus
}
#endif
#endif
