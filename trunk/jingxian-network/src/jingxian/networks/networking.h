
#ifndef _base_socket_h_
#define _base_socket_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "Winsock2.h"
# include "Mswsock.h"
# include "jingxian/string/string.h"
# include "jingxian/buffer/IBuffer.h"

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
                  DWORD dwFlags);

/**
 * @see MSDN
 */
bool acceptEx(SOCKET sListenSocket,
              SOCKET sAcceptSocket,
              PVOID lpOutputBuffer,
              DWORD dwReceiveDataLength,
              DWORD dwLocalAddressLength,
              DWORD dwRemoteAddressLength,
              LPDWORD lpdwBytesReceived,
              LPOVERLAPPED lpOverlapped);

/**
 * @see MSDN
 */
bool transmitPackets(SOCKET hSocket,
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
               LPOVERLAPPED lpOverlapped);

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

/**
 *  从 <schema>://<addr>:<port> 格式中取出 addr，其中 schema 与 port 是
 *  可选的
 */
tstring fetchAddr(const tchar* host);

/**
 *  从 <schema>://<addr>:<port> 格式中取出 port，其中 schema 与 port 是
 *  可选的
 */
const tchar* fetchPort(const tchar* host);

/**
 * 从 <schema>://<addr>:<port> 格式中取出 addr 和 port 转换成 sockaddr，其
 * 中 schema 与 port 是可选的,其中 schema 中最后一个字符是 '6' 时表示采用
 * IPv6格式.
 */
bool stringToAddress(const tchar* host
                     , struct sockaddr* addr
                     , int* len);

bool addressToString(struct sockaddr* name
                     , int len
                     , const tchar* schema
                     , tstring& host);
}

_jingxian_end

#endif /* _base_socket_h_ */
