
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
   * ��ʼ��socket����
   */
  bool initializeScket();

  /**
   * �ر�socket����
   */
  void shutdownSocket();
  
  /**
   * �ж� socket �Ƿ������ݿɶ�
   */
  bool isReadable(SOCKET sock);

  /**
   * �ж� socket �Ƿ��д
   */
  bool isWritable(SOCKET sock);

  /**
   * ���� socket �Ƿ�����
   */
  void setBlocking(SOCKET sock, bool val);

  /**
   * �жϲ��ȴ�ֱ��socket���Խ��ж�(д)�������������ʱ
   * @params[ in ] timval ��ʱʱ��
   * @params[ in ] mode �жϵĵĲ������ͣ����select_modeö��
   * @return ���Բ�������true
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
