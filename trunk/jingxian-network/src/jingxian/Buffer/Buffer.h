
#ifndef _InternalBuffer_H_
#define _InternalBuffer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include <Mswsock.h>
# include <vector>

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
	if(is_null(buf))
		return;

	if(is_null(buf->freebuffer))
		my_free(buf);
	else
		buf->freebuffer(buf, buf->context);
}

_jingxian_end

#endif //_InternalBuffer_H_