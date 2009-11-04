
#ifndef _Buffer_H_
#define _Buffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include <Mswsock.h>

_jingxian_begin

# ifndef _io_mem_buf_buf_
# define _io_mem_buf_buf_
typedef WSABUF io_mem_buf;
# endif //_io_mem_buf_buf_

# ifndef _io_packect_buf_
# define _io_packect_buf_
typedef TRANSMIT_PACKETS_ELEMENT io_packect_buf;
# endif // ___iopack___

# ifndef _io_file_buf_
# define _io_file_buf_
typedef TRANSMIT_FILE_BUFFERS io_file_buf;
# endif // _io_file_buf_

struct buffer_chain;

typedef void (*freebuffer_callback)(struct buffer_chain* ptr, void* context);

typedef struct buffer_chain
{
    void* context;
    freebuffer_callback freebuffer;
    int type;
    struct buffer_chain* _next;
} buffer_chain_t;


inline bool is_null(const freebuffer_callback cb)
{
    return NULL == cb;
}

inline void freebuffer(buffer_chain_t* buf)
{
    if (is_null(buf))
        return;

    if (is_null(buf->freebuffer))
        my_free(buf);
    else
        buf->freebuffer(buf, buf->context);
}

void freeBuffer(buffer_chain_t* chain, void* context);


const int BUFFER_ELEMENT_MEMORY = 1;
const int BUFFER_ELEMENT_FILE = 2;
const int BUFFER_ELEMENT_PACKET = 3;

typedef struct databuffer
{
    buffer_chain_t chain;
    // 内存块大小（可选值，为0时为无效值）
    size_t capacity;
    // 数据在 buf 的起始位置
    char* start;
    // 数据在 buf 的结束位置
    char* end;
    // 数据内存指针
    char ptr[1];
} databuffer_t;

typedef struct filebuffer
{
    buffer_chain_t chain;
    HANDLE file;
    DWORD  write_bytes;
    DWORD  bytes_per_send;
    TRANSMIT_FILE_BUFFERS buf;
} filebuffer_t;

typedef struct packetbuffer
{
    buffer_chain_t chain;

    DWORD element_count;
    DWORD send_size;

    TRANSMIT_PACKETS_ELEMENT packetArray[1];

} packetbuffer_t;

_jingxian_end

#endif //_Buffer_H_
