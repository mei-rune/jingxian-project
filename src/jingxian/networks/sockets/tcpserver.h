
#ifndef tcp_server_h
#define tcp_server_h

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/NetAddress.h"
# include "jingxian/networks/sockets/tcpclient.h"

_jingxian_begin

class TCPServer
{
public:

  TCPServer (void);

  ~TCPServer (void);

  /**
   * 取得socket对象
   */
  BaseSocket& socket();

  /**
   * 取得socket对象
   */
  const BaseSocket& socket() const;

  /**
   * 取得监听的端口
   */
  const NetAddress& bind_addr() const;

  /**
   * socket是否有效
   */
  bool is_good() const;

  /**
   * 绑定到指定的端口
   */
  bool bind( const NetAddress& addr);

  /**
   * 启动监听
   */
  bool listen( int backlog = SOMAXCONN );

  /**
   * 获取一个连接
   */
  bool accept( TCPClient& accepted );

  /**
   * 异步获取一个连接
   */
  bool accept( TCPClient& accepted
						, void* data_buffer
						, size_t data_len
						, size_t local_addr_len
						, size_t remote_addr_len
						, OVERLAPPED& overlapped);

  void swap( TCPServer& r);

  /**
   * 返回一个人可读的字符串
   */
  const tstring& toString() const;

private:
	NOCOPY( TCPServer );
	NetAddress bind_addr_;
	BaseSocket socket_;
	mutable tstring toString_;
};


inline tostream& operator<<( tostream& target, const TCPServer& server )
{
  if( server.is_good() )
  {
	  target << _T("TCPServer[ ")
		  <<  server.socket().handle() 
		  << _T(":") << server.bind_addr()
		  << _T("]" );
  }
  else
  {
	  target << _T("TCPClient[ no listen ]" );
  }
  
  return target;
}

#if defined (OS_HAS_INLINED)
#include "jingxian/networks/sockets/TCPServer.inl"
#endif

_jingxian_end

#endif /* tcp_server_h */
