
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
   * 设置socket选项，请见setsockopt
   */
  bool set_option (SOCKET sock, 
	              int level,
                  int option,
                  void *optval,
                  int optlen);
  /**
   * 获得socket选项，请见getsockopt
   */
  bool get_option (SOCKET sock, 
				  int level,
                  int option,
                  void *optval,
                  int *optlen);
  
  /**
   * 判断并等待直到socket可以进行读(写)操作，或出错，或超时
   * @params[ in ] timval 超时时间
   * @params[ in ] mode 判断的的操作类型，请见select_mode枚举
   * @return 可以操作返回true
   */
  bool poll(SOCKET sock, const TIMEVAL& timeval, int select_mode);

  /**
   * 初始化socket服务
   */
  bool initializeScket();

  /**
   * 关闭socket服务
   */
  void shutdownSocket();

  /**
   * 启动socket的选项
   * @params[ in ] value 可取值请见ioctlsocket
   */
  bool enable(SOCKET sock, int value);

  /**
   * 启动socket的选项
   * @params[ in ] value 可取值请见ioctlsocket
   */
  bool disable(SOCKET sock, int value);

  
  bool isReadable(SOCKET sock);

  bool isWritable(SOCKET sock);

  void setBlocking(SOCKET sock, bool val);

  bool transmitFile(SOCKET hSocket,
		  HANDLE hFile,
		  DWORD nNumberOfBytesToWrite,
		  DWORD nNumberOfBytesPerSend,
		  LPOVERLAPPED lpOverlapped,
		  LPTRANSMIT_FILE_BUFFERS lpTransmitBuffers,
		  DWORD dwFlags );

  bool acceptEx( SOCKET sListenSocket,
          SOCKET sAcceptSocket,
          PVOID lpOutputBuffer,
          DWORD dwReceiveDataLength,
          DWORD dwLocalAddressLength,
          DWORD dwRemoteAddressLength,
          LPDWORD lpdwBytesReceived,
          LPOVERLAPPED lpOverlapped );
  
  bool transmitPackets( SOCKET hSocket,
		  LPTRANSMIT_PACKETS_ELEMENT lpPacketArray,
		  DWORD nElementCount,
		  DWORD nSendSize,
		  LPOVERLAPPED lpOverlapped,
		  DWORD dwFlags);

  bool connectEx(SOCKET s,
          const struct sockaddr* name,
          int namelen,
          PVOID lpSendBuffer,
          DWORD dwSendDataLength,
          LPDWORD lpdwBytesSent,
          LPOVERLAPPED lpOverlapped );

  bool disconnectEx(SOCKET hSocket,
          LPOVERLAPPED lpOverlapped,
          DWORD dwFlags,
          DWORD reserved);

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
