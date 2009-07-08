
#ifndef _base_socket_h_
#define _base_socket_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "Winsock2.h"
# include "Mswsock.h"
# include "jingxian/string/string.hpp"

typedef WSABUF iovec;

# ifndef ___iopack___
# define ___iopack___
 typedef TRANSMIT_PACKETS_ELEMENT iopack;
# endif // ___iopack___

 # ifndef ___iofile___
# define ___iopack___
 typedef TRANSMIT_FILE_BUFFERS io_file_buf;
# endif // ___iopack___


_jingxian_begin

namespace networking
{
  enum select_mode
  {
		  select_read = 1
		, select_write = 2
		, select_error = 4
  };

  
  /**
   * 初始化socket服务
   */
  bool initializeScket();

  /**
   * 关闭socket服务
   */
  void shutdownSocket();
  
  /**
   * 判断 socket 是否有数据可读
   */
  bool isReadable(SOCKET sock);

  /**
   * 判断 socket 是否可写
   */
  bool isWritable(SOCKET sock);

  /**
   * 设置 socket 是否阻塞
   */
  void setBlocking(SOCKET sock, bool val);

  /**
   * 判断并等待直到socket可以进行读(写)操作，或出错，或超时
   * @params[ in ] timval 超时时间
   * @params[ in ] mode 判断的的操作类型，请见select_mode枚举
   * @return 可以操作返回true
   */
  bool poll(SOCKET sock, const TIMEVAL& timeval, int select_mode);

  /**
   * @see MSDN
   */
  bool transmitFile(SOCKET hSocket,
		  HANDLE hFile,
		  DWORD nNumberOfBytesToWrite,
		  DWORD nNumberOfBytesPerSend,
		  LPOVERLAPPED lpOverlapped,
		  LPTRANSMIT_FILE_BUFFERS lpTransmitBuffers,
		  DWORD dwFlags );

  /**
   * @see MSDN
   */
  bool acceptEx( SOCKET sListenSocket,
          SOCKET sAcceptSocket,
          PVOID lpOutputBuffer,
          DWORD dwReceiveDataLength,
          DWORD dwLocalAddressLength,
          DWORD dwRemoteAddressLength,
          LPDWORD lpdwBytesReceived,
          LPOVERLAPPED lpOverlapped );
  
  /**
   * @see MSDN
   */
  bool transmitPackets( SOCKET hSocket,
		  LPTRANSMIT_PACKETS_ELEMENT lpPacketArray,
		  DWORD nElementCount,
		  DWORD nSendSize,
		  LPOVERLAPPED lpOverlapped,
		  DWORD dwFlags);

  /**
   * @see MSDN
   */
  bool connectEx(SOCKET s,
          const struct sockaddr* name,
          int namelen,
          PVOID lpSendBuffer,
          DWORD dwSendDataLength,
          LPDWORD lpdwBytesSent,
          LPOVERLAPPED lpOverlapped );

  /**
   * @see MSDN
   */
  bool disconnectEx(SOCKET hSocket,
          LPOVERLAPPED lpOverlapped,
          DWORD dwFlags,
          DWORD reserved);

  /**
   * @see MSDN
   */
  void getAcceptExSockaddrs(PVOID lpOutputBuffer,
          DWORD dwReceiveDataLength,
          DWORD dwLocalAddressLength,
          DWORD dwRemoteAddressLength,
          LPSOCKADDR* LocalSockaddr,
          LPINT LocalSockaddrLength,
          LPSOCKADDR* RemoteSockaddr,
          LPINT RemoteSockaddrLength);
}

_jingxian_end

#endif /* _base_socket_h_ */
