
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
   * ȡ��socket����
   */
  BaseSocket& socket();

  /**
   * ȡ��socket����
   */
  const BaseSocket& socket() const;

  /**
   * ȡ�ü����Ķ˿�
   */
  const NetAddress& bind_addr() const;

  /**
   * socket�Ƿ���Ч
   */
  bool is_good() const;

  /**
   * �󶨵�ָ���Ķ˿�
   */
  bool bind( const NetAddress& addr);

  /**
   * ��������
   */
  bool listen( int backlog = SOMAXCONN );

  /**
   * ��ȡһ������
   */
  bool accept( TCPClient& accepted );

  /**
   * �첽��ȡһ������
   */
  bool accept( TCPClient& accepted
						, void* data_buffer
						, size_t data_len
						, size_t local_addr_len
						, size_t remote_addr_len
						, OVERLAPPED& overlapped);

  void swap( TCPServer& r);

  /**
   * ����һ���˿ɶ����ַ���
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
