#include <stdlib.h>
#include "http.h"

__attribute__((weak, export_name("canonical_abi_realloc")))
void *canonical_abi_realloc(
void *ptr,
size_t orig_size,
size_t org_align,
size_t new_size
) {
  void *ret = realloc(ptr, new_size);
  if (!ret)
  abort();
  return ret;
}

__attribute__((weak, export_name("canonical_abi_free")))
void canonical_abi_free(
void *ptr,
size_t size,
size_t align
) {
  free(ptr);
}

__attribute__((import_module("canonical_abi"), import_name("resource_drop_router")))
void __resource_router_drop(uint32_t idx);

void http_router_free(http_router_t *ptr) {
  __resource_router_drop(ptr->idx);
}

__attribute__((import_module("canonical_abi"), import_name("resource_clone_router")))
uint32_t __resource_router_clone(uint32_t idx);

http_router_t http_router_clone(http_router_t *ptr) {
  return (http_router_t){__resource_router_clone(ptr->idx)};
}

__attribute__((import_module("canonical_abi"), import_name("resource_drop_server")))
void __resource_server_drop(uint32_t idx);

void http_server_free(http_server_t *ptr) {
  __resource_server_drop(ptr->idx);
}

__attribute__((import_module("canonical_abi"), import_name("resource_clone_server")))
uint32_t __resource_server_clone(uint32_t idx);

http_server_t http_server_clone(http_server_t *ptr) {
  return (http_server_t){__resource_server_clone(ptr->idx)};
}
#include <string.h>

void http_string_set(http_string_t *ret, const char *s) {
  ret->ptr = (char*) s;
  ret->len = strlen(s);
}

void http_string_dup(http_string_t *ret, const char *s) {
  ret->len = strlen(s);
  ret->ptr = canonical_abi_realloc(NULL, 0, 1, ret->len);
  memcpy(ret->ptr, s, ret->len);
}

void http_string_free(http_string_t *ret) {
  canonical_abi_free(ret->ptr, ret->len, 1);
  ret->ptr = NULL;
  ret->len = 0;
}
void http_uri_free(http_uri_t *ptr) {
  http_string_free(ptr);
}
void http_error_free(http_error_t *ptr) {
  switch ((int32_t) ptr->tag) {
    case 0: {
      http_string_free(&ptr->val.error_with_description);
      break;
    }
  }
}
void http_expected_router_error_free(http_expected_router_error_t *ptr) {
  if (!ptr->is_err) {
    http_router_free(&ptr->val.ok);
  } else {
    http_error_free(&ptr->val.err);
  }
}
void http_expected_server_error_free(http_expected_server_error_t *ptr) {
  if (!ptr->is_err) {
    http_server_free(&ptr->val.ok);
  } else {
    http_error_free(&ptr->val.err);
  }
}
void http_expected_unit_error_free(http_expected_unit_error_t *ptr) {
  if (!ptr->is_err) {
  } else {
    http_error_free(&ptr->val.err);
  }
}

__attribute__((aligned(4)))
static uint8_t RET_AREA[16];
__attribute__((import_module("http"), import_name("router::new")))
void __wasm_import_http_router_new(int32_t);
void http_router_new(http_expected_router_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_router_new(ptr);
  http_expected_router_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_router_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("router::new-with-base")))
void __wasm_import_http_router_new_with_base(int32_t, int32_t, int32_t);
void http_router_new_with_base(http_uri_t *base, http_expected_router_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_router_new_with_base((int32_t) (*base).ptr, (int32_t) (*base).len, ptr);
  http_expected_router_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_router_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("router::get")))
void __wasm_import_http_router_get(int32_t, int32_t, int32_t, int32_t, int32_t, int32_t);
void http_router_get(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_router_get((self).idx, (int32_t) (*route).ptr, (int32_t) (*route).len, (int32_t) (*handler).ptr, (int32_t) (*handler).len, ptr);
  http_expected_router_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_router_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("router::put")))
void __wasm_import_http_router_put(int32_t, int32_t, int32_t, int32_t, int32_t, int32_t);
void http_router_put(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_router_put((self).idx, (int32_t) (*route).ptr, (int32_t) (*route).len, (int32_t) (*handler).ptr, (int32_t) (*handler).len, ptr);
  http_expected_router_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_router_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("router::post")))
void __wasm_import_http_router_post(int32_t, int32_t, int32_t, int32_t, int32_t, int32_t);
void http_router_post(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_router_post((self).idx, (int32_t) (*route).ptr, (int32_t) (*route).len, (int32_t) (*handler).ptr, (int32_t) (*handler).len, ptr);
  http_expected_router_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_router_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("router::delete")))
void __wasm_import_http_router_delete(int32_t, int32_t, int32_t, int32_t, int32_t, int32_t);
void http_router_delete(http_router_t self, http_string_t *route, http_string_t *handler, http_expected_router_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_router_delete((self).idx, (int32_t) (*route).ptr, (int32_t) (*route).len, (int32_t) (*handler).ptr, (int32_t) (*handler).len, ptr);
  http_expected_router_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_router_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("server::serve")))
void __wasm_import_http_server_serve(int32_t, int32_t, int32_t, int32_t);
void http_server_serve(http_string_t *address, http_router_t router, http_expected_server_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_server_serve((int32_t) (*address).ptr, (int32_t) (*address).len, (router).idx, ptr);
  http_expected_server_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      expected.val.ok = (http_server_t){ *((int32_t*) (ptr + 4)) };
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
__attribute__((import_module("http"), import_name("server::stop")))
void __wasm_import_http_server_stop(int32_t, int32_t);
void http_server_stop(http_server_t self, http_expected_unit_error_t *ret0) {
  int32_t ptr = (int32_t) &RET_AREA;
  __wasm_import_http_server_stop((self).idx, ptr);
  http_expected_unit_error_t expected;
  switch ((int32_t) (*((uint8_t*) (ptr + 0)))) {
    case 0: {
      expected.is_err = false;
      
      
      break;
    }
    case 1: {
      expected.is_err = true;
      http_error_t variant;
      variant.tag = (int32_t) (*((uint8_t*) (ptr + 4)));
      switch ((int32_t) variant.tag) {
        case 0: {
          variant.val.error_with_description = (http_string_t) { (char*)(*((int32_t*) (ptr + 8))), (size_t)(*((int32_t*) (ptr + 12))) };
          break;
        }
      }
      
      expected.val.err = variant;
      break;
    }
  }*ret0 = expected;
}
