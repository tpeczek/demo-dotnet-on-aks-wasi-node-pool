#include <stdlib.h>
#include <http.h>

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

__attribute__((import_module("canonical_abi"), import_name("resource_new_router")))
uint32_t __resource_router_new(uint32_t val);

http_router_t http_router_new(void *data) {
  return (http_router_t){__resource_router_new((uint32_t) data)};
}

__attribute__((import_module("canonical_abi"), import_name("resource_get_router")))
uint32_t __resource_router_get(uint32_t idx);

void* http_router_get(http_router_t *ptr) {
  return (void*) __resource_router_get(ptr->idx);
}

__attribute__((export_name("canonical_abi_drop_router")))
void __resource_router_dtor(uint32_t val) {
  if (http_router_dtor)
  http_router_dtor((void*) val);
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

__attribute__((import_module("canonical_abi"), import_name("resource_new_server")))
uint32_t __resource_server_new(uint32_t val);

http_server_t http_server_new(void *data) {
  return (http_server_t){__resource_server_new((uint32_t) data)};
}

__attribute__((import_module("canonical_abi"), import_name("resource_get_server")))
uint32_t __resource_server_get(uint32_t idx);

void* http_server_get(http_server_t *ptr) {
  return (void*) __resource_server_get(ptr->idx);
}

__attribute__((export_name("canonical_abi_drop_server")))
void __resource_server_dtor(uint32_t val) {
  if (http_server_dtor)
  http_server_dtor((void*) val);
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
__attribute__((export_name("router::new")))
int32_t __wasm_export_http_router_new(void) {
  http_expected_router_error_t ret;
  http_router_new(&ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload0 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload0).tag) {
      case 0: {
        const http_string_t *payload1 = &(*payload0).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload1).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload1).ptr;
        break;
      }
    }
    
  } else {
    const http_router_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("router::new-with-base")))
int32_t __wasm_export_http_router_new_with_base(int32_t arg, int32_t arg0) {
  http_uri_t arg1 = (http_string_t) { (char*)(arg), (size_t)(arg0) };
  http_expected_router_error_t ret;
  http_router_new_with_base(&arg1, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload2 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload2).tag) {
      case 0: {
        const http_string_t *payload3 = &(*payload2).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload3).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload3).ptr;
        break;
      }
    }
    
  } else {
    const http_router_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("router::get")))
int32_t __wasm_export_http_router_get(int32_t arg, int32_t arg0, int32_t arg1, int32_t arg2, int32_t arg3) {
  http_string_t arg4 = (http_string_t) { (char*)(arg0), (size_t)(arg1) };
  http_string_t arg5 = (http_string_t) { (char*)(arg2), (size_t)(arg3) };
  http_expected_router_error_t ret;
  http_router_get((http_router_t){ arg }, &arg4, &arg5, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload6 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload6).tag) {
      case 0: {
        const http_string_t *payload7 = &(*payload6).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload7).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload7).ptr;
        break;
      }
    }
    
  } else {
    const http_router_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("router::put")))
int32_t __wasm_export_http_router_put(int32_t arg, int32_t arg0, int32_t arg1, int32_t arg2, int32_t arg3) {
  http_string_t arg4 = (http_string_t) { (char*)(arg0), (size_t)(arg1) };
  http_string_t arg5 = (http_string_t) { (char*)(arg2), (size_t)(arg3) };
  http_expected_router_error_t ret;
  http_router_put((http_router_t){ arg }, &arg4, &arg5, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload6 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload6).tag) {
      case 0: {
        const http_string_t *payload7 = &(*payload6).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload7).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload7).ptr;
        break;
      }
    }
    
  } else {
    const http_router_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("router::post")))
int32_t __wasm_export_http_router_post(int32_t arg, int32_t arg0, int32_t arg1, int32_t arg2, int32_t arg3) {
  http_string_t arg4 = (http_string_t) { (char*)(arg0), (size_t)(arg1) };
  http_string_t arg5 = (http_string_t) { (char*)(arg2), (size_t)(arg3) };
  http_expected_router_error_t ret;
  http_router_post((http_router_t){ arg }, &arg4, &arg5, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload6 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload6).tag) {
      case 0: {
        const http_string_t *payload7 = &(*payload6).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload7).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload7).ptr;
        break;
      }
    }
    
  } else {
    const http_router_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("router::delete")))
int32_t __wasm_export_http_router_delete(int32_t arg, int32_t arg0, int32_t arg1, int32_t arg2, int32_t arg3) {
  http_string_t arg4 = (http_string_t) { (char*)(arg0), (size_t)(arg1) };
  http_string_t arg5 = (http_string_t) { (char*)(arg2), (size_t)(arg3) };
  http_expected_router_error_t ret;
  http_router_delete((http_router_t){ arg }, &arg4, &arg5, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload6 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload6).tag) {
      case 0: {
        const http_string_t *payload7 = &(*payload6).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload7).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload7).ptr;
        break;
      }
    }
    
  } else {
    const http_router_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("server::serve")))
int32_t __wasm_export_http_server_serve(int32_t arg, int32_t arg0, int32_t arg1) {
  http_string_t arg2 = (http_string_t) { (char*)(arg), (size_t)(arg0) };
  http_expected_server_error_t ret;
  http_server_serve(&arg2, (http_router_t){ arg1 }, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload3 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload3).tag) {
      case 0: {
        const http_string_t *payload4 = &(*payload3).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload4).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload4).ptr;
        break;
      }
    }
    
  } else {
    const http_server_t *payload = &(ret).val.ok;
    *((int8_t*)(ptr + 0)) = 0;
    *((int32_t*)(ptr + 4)) = (*payload).idx;
    
  }
  return ptr;
}
__attribute__((export_name("server::stop")))
int32_t __wasm_export_http_server_stop(int32_t arg) {
  http_expected_unit_error_t ret;
  http_server_stop((http_server_t){ arg }, &ret);
  int32_t ptr = (int32_t) &RET_AREA;
  
  if ((ret).is_err) {
    const http_error_t *payload0 = &(ret).val.err;
    *((int8_t*)(ptr + 0)) = 1;
    switch ((int32_t) (*payload0).tag) {
      case 0: {
        const http_string_t *payload1 = &(*payload0).val.error_with_description;
        *((int8_t*)(ptr + 4)) = 0;
        *((int32_t*)(ptr + 12)) = (int32_t) (*payload1).len;
        *((int32_t*)(ptr + 8)) = (int32_t) (*payload1).ptr;
        break;
      }
    }
    
  } else {
    
    *((int8_t*)(ptr + 0)) = 0;
    
  }
  return ptr;
}
