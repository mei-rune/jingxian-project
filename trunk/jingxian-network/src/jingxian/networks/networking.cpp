# include "pro_config.h"
# include <vector>
# include "jingxian/networks/networking.h"

_jingxian_begin

namespace networking
{

static LPFN_TRANSMITFILE __transmitfile;
static LPFN_ACCEPTEX __acceptex;
static LPFN_TRANSMITPACKETS __transmitpackets;
static LPFN_CONNECTEX __connectex;
static LPFN_DISCONNECTEX __disconnectex;
static LPFN_GETACCEPTEXSOCKADDRS __getacceptexsockaddrs;

bool set_option(SOCKET sock,
                int level,
                int option,
                void *optval,
                int optlen)
{
    return (SOCKET_ERROR != setsockopt(sock, level,
                                       option, (char *) optval, optlen));
}

bool get_option(SOCKET sock,
                int level,
                int option,
                void *optval,
                int *optlen)
{
    return (SOCKET_ERROR != getsockopt(sock, level,
                                       option, (char *) optval, optlen));
}

bool enable(SOCKET sock, int value)
{
    u_long nonblock = 1;
    return (0 ==::ioctlsocket(sock,
                              value,
                              &nonblock));
}

bool disable(SOCKET sock, int value)
{
    u_long nonblock = 0;
    return (0 == ioctlsocket(sock,
                             value,
                             &nonblock));
}

bool poll(SOCKET sock, const TIMEVAL& time_val, int mode)
{
    fd_set socket_set;
    FD_ZERO(&socket_set);
    FD_SET(sock, &socket_set);

    return (1 == ::select(0, (mode & select_read) ? &socket_set : NULL
                          , (mode & select_write) ? &socket_set : NULL
                          , (mode & select_error) ? &socket_set : NULL
                          , &time_val));
}

bool isReadable(SOCKET sock)
{
    TIMEVAL time_val;
    time_val.tv_sec = 0;
    time_val.tv_usec = 0;
    return poll(sock, time_val, select_read);
}

bool isWritable(SOCKET sock)
{
    TIMEVAL time_val;
    time_val.tv_sec = 0;
    time_val.tv_usec = 0;
    return poll(sock, time_val, select_write);
}

void setBlocking(SOCKET sock, bool val)
{
    if (val)
        enable(sock, FIONBIO);
    else
        disable(sock, FIONBIO);
}

bool send_n(SOCKET sock, const char* buf, size_t length)
{
    do
    {
#pragma warning(disable: 4267)
        int n = ::send(sock, buf, length, 0);
#pragma warning(default: 4267)
        if (0 >= n)
            return false;

        length -= n;
        buf += n;
    }
    while (0 < length);

    return true;
}

bool recv_n(SOCKET sock, char* buf, size_t length)
{
    do
    {
#pragma warning(disable: 4267)
        int n = ::recv(sock, buf, length, 0);
#pragma warning(default: 4267)

        if (0 >= n)
            return false;

        length -= n;
        buf += n;
    }
    while (0 < length);

    return true;
}

bool sendv_n(SOCKET sock, const io_mem_buf* wsaBuf, size_t size)
{
    std::vector<io_mem_buf> buf(wsaBuf, wsaBuf + size);
    io_mem_buf* p = &buf[0];

    do
    {
        DWORD numberOfBytesSent = 0;
#pragma warning(disable: 4267)
        if (SOCKET_ERROR == ::WSASend(sock, p, size, &numberOfBytesSent, 0, 0 , 0))
#pragma warning(default: 4267)
            return false;

        do
        {
            if (numberOfBytesSent < p->len)
            {
                p->len -= numberOfBytesSent;
                p->buf = p->buf + numberOfBytesSent;
                break;
            }
            numberOfBytesSent -= p->len;
            ++ p;
            -- size;
        }
        while (0 < numberOfBytesSent);
    }
    while (0 < size);

    return true;
}

bool recvv_n(SOCKET sock, io_mem_buf* wsaBuf, size_t size)
{
    io_mem_buf* p = wsaBuf;

    do
    {
        DWORD numberOfBytesRecvd = 0;
#pragma warning(disable: 4267)
        if (SOCKET_ERROR == ::WSARecv(sock, p, size, &numberOfBytesRecvd, 0, 0 , 0))
#pragma warning(default: 4267)
            return false;

        do
        {
            if (numberOfBytesRecvd < p->len)
            {
                p->len -= numberOfBytesRecvd;
                p->buf = p->buf + numberOfBytesRecvd;
                break;
            }
            numberOfBytesRecvd -= p->len;
            ++ p;
            -- size;
        }
        while (0 < numberOfBytesRecvd);
    }
    while (0 < size);

    return true;
}
bool  initializeScket()
{
    WSADATA wsaData;
    if (0 != WSAStartup(MAKEWORD(2, 2), &wsaData))
        return false;

    if (LOBYTE(wsaData.wVersion) != 2 ||
            HIBYTE(wsaData.wVersion) != 2)
    {
        WSACleanup();
        return false;
    }

    SOCKET cliSock = ::socket(AF_INET , SOCK_STREAM, IPPROTO_TCP);

    GUID GuidConnectEx = WSAID_CONNECTEX;
    GUID GuidDisconnectEx = WSAID_DISCONNECTEX;
    GUID GuidGetAcceptExSockAddrs = WSAID_GETACCEPTEXSOCKADDRS;
    GUID GuidAcceptEx = WSAID_ACCEPTEX;
    GUID GuidTransmitFile = WSAID_TRANSMITFILE;
    GUID GuidTransmitPackets = WSAID_TRANSMITPACKETS;

    DWORD dwBytes = 0;
    if (SOCKET_ERROR == WSAIoctl(cliSock,
                                 SIO_GET_EXTENSION_FUNCTION_POINTER,
                                 &GuidConnectEx,
                                 sizeof(GuidConnectEx),
                                 &__connectex,
                                 sizeof(__connectex),
                                 &dwBytes,
                                 NULL,
                                 NULL))
    {
        __connectex = NULL;
    }


    dwBytes = 0;
    if (SOCKET_ERROR == WSAIoctl(cliSock,
                                 SIO_GET_EXTENSION_FUNCTION_POINTER,
                                 &GuidDisconnectEx,
                                 sizeof(GuidDisconnectEx),
                                 &__disconnectex,
                                 sizeof(__disconnectex),
                                 &dwBytes,
                                 NULL,
                                 NULL))
    {
        __disconnectex = NULL;
    }

    dwBytes = 0;
    if (SOCKET_ERROR == WSAIoctl(cliSock,
                                 SIO_GET_EXTENSION_FUNCTION_POINTER,
                                 &GuidTransmitFile,
                                 sizeof(GuidTransmitFile),
                                 &__transmitfile,
                                 sizeof(__transmitfile),
                                 &dwBytes,
                                 NULL,
                                 NULL))
    {
        __transmitfile = NULL;
    }

    dwBytes = 0;
    if (SOCKET_ERROR == WSAIoctl(cliSock,
                                 SIO_GET_EXTENSION_FUNCTION_POINTER,
                                 &GuidAcceptEx,
                                 sizeof(GuidAcceptEx),
                                 &__acceptex,
                                 sizeof(__acceptex),
                                 &dwBytes,
                                 NULL,
                                 NULL))
    {
        __acceptex = NULL;
    }

    dwBytes = 0;
    if (SOCKET_ERROR == WSAIoctl(cliSock,
                                 SIO_GET_EXTENSION_FUNCTION_POINTER,
                                 &GuidTransmitPackets,
                                 sizeof(GuidTransmitPackets),
                                 &__transmitpackets,
                                 sizeof(__transmitpackets),
                                 &dwBytes,
                                 NULL,
                                 NULL))
    {
        __transmitpackets = NULL;
    }

    dwBytes = 0;
    if (SOCKET_ERROR == WSAIoctl(cliSock,
                                 SIO_GET_EXTENSION_FUNCTION_POINTER,
                                 &GuidGetAcceptExSockAddrs,
                                 sizeof(GuidGetAcceptExSockAddrs),
                                 &__getacceptexsockaddrs,
                                 sizeof(__getacceptexsockaddrs),
                                 &dwBytes,
                                 NULL,
                                 NULL))
    {
        __getacceptexsockaddrs = NULL;
    }

    closesocket(cliSock);

    return true;
}

void shutdownSocket()
{
    WSACleanup();
}

bool transmitFile(SOCKET hSocket,
                  HANDLE hFile,
                  DWORD nNumberOfBytesToWrite,
                  DWORD nNumberOfBytesPerSend,
                  LPOVERLAPPED lpOverlapped,
                  LPTRANSMIT_FILE_BUFFERS lpTransmitBuffers,
                  DWORD dwFlags)
{
    return TRUE == __transmitfile(hSocket
                                  , hFile
                                  , nNumberOfBytesToWrite
                                  , nNumberOfBytesPerSend
                                  , lpOverlapped
                                  , lpTransmitBuffers
                                  , dwFlags);
}

bool acceptEx(SOCKET sListenSocket,
              SOCKET sAcceptSocket,
              PVOID lpOutputBuffer,
              DWORD dwReceiveDataLength,
              DWORD dwLocalAddressLength,
              DWORD dwRemoteAddressLength,
              LPDWORD lpdwBytesReceived,
              LPOVERLAPPED lpOverlapped)
{
    return TRUE == __acceptex(sListenSocket,
                              sAcceptSocket,
                              lpOutputBuffer,
                              dwReceiveDataLength,
                              dwLocalAddressLength,
                              dwRemoteAddressLength,
                              lpdwBytesReceived,
                              lpOverlapped);
}

bool transmitPackets(SOCKET hSocket,
                     LPTRANSMIT_PACKETS_ELEMENT lpPacketArray,
                     DWORD nElementCount,
                     DWORD nSendSize,
                     LPOVERLAPPED lpOverlapped,
                     DWORD dwFlags)
{
    return TRUE == __transmitpackets(hSocket,
                                     lpPacketArray,
                                     nElementCount,
                                     nSendSize,
                                     lpOverlapped,
                                     dwFlags);
}

bool connectEx(SOCKET s,
               const struct sockaddr* name,
               int namelen,
               PVOID lpSendBuffer,
               DWORD dwSendDataLength,
               LPDWORD lpdwBytesSent,
               LPOVERLAPPED lpOverlapped)
{
    return TRUE == __connectex(s,
                               name,
                               namelen,
                               lpSendBuffer,
                               dwSendDataLength,
                               lpdwBytesSent,
                               lpOverlapped);
}

bool disconnectEx(SOCKET hSocket,
                  LPOVERLAPPED lpOverlapped,
                  DWORD dwFlags,
                  DWORD reserved)
{
    return TRUE == __disconnectex(hSocket,
                                  lpOverlapped,
                                  dwFlags,
                                  reserved);
}

void getAcceptExSockaddrs(PVOID lpOutputBuffer,
                          DWORD dwReceiveDataLength,
                          DWORD dwLocalAddressLength,
                          DWORD dwRemoteAddressLength,
                          LPSOCKADDR* LocalSockaddr,
                          LPINT LocalSockaddrLength,
                          LPSOCKADDR* RemoteSockaddr,
                          LPINT RemoteSockaddrLength)
{
    __getacceptexsockaddrs(lpOutputBuffer,
                           dwReceiveDataLength,
                           dwLocalAddressLength,
                           dwRemoteAddressLength,
                           LocalSockaddr,
                           LocalSockaddrLength,
                           RemoteSockaddr,
                           RemoteSockaddrLength);
}


bool stringToAddress(const tchar* host
                     , struct sockaddr* addr
                     , int* len)
{
    memset(addr, 0, *len);
    addr->sa_family = AF_INET;

    const tchar* begin = string_traits<tchar>::strstr(host, _T("://"));
    if (null_ptr != begin)
    {
        if (begin != host && _T('6') == *(begin - 1))
            addr->sa_family = AF_INET6;

        begin += 3;
    }
    else
    {
        begin = host;
    }

    return (SOCKET_ERROR != ::WSAStringToAddress((LPTSTR)begin
									, addr->sa_family
									, 0
									, addr
									, len));
}

tstring fetchAddr(const tchar* host)
{
    const tchar* begin = string_traits<tchar>::strstr(host, _T("://"));
    if (null_ptr == begin)
        begin = host;
    else
        begin += 3;

    const tchar* end = string_traits<tchar>::strchr(begin, _T(':'));
    if (null_ptr == end)
        return begin;

    return tstring(begin, end);
}


const tchar* fetchPort(const tchar* host)
{
    const tchar* end = string_traits<tchar>::strrchr(host, _T(':'));
    if (null_ptr == end)
        return 0;
    return ++end;
}

bool addressToString(struct sockaddr* addr
                     , int len
                     , const tchar* schema
                     , tstring& host)
{
    host = (null_ptr == schema) ? _T("tcp") : schema;
    host += (addr->sa_family == AF_INET6) ? _T("6://") : _T("://");

    size_t prefix = host.size();
    host.resize(256);
    DWORD addressLength = host.size() - prefix;

    if (SOCKET_ERROR == ::WSAAddressToString(addr
					, len
					, NULL
					, (LPTSTR)host.c_str() + prefix
					, &addressLength))
        return false;

    host.resize(addressLength + prefix - 1);
    return true;
}
}

_jingxian_end