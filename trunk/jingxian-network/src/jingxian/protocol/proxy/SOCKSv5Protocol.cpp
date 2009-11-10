
# include "pro_config.h"
# include <fcntl.h>
# include <sys/types.h>
# include <sys/stat.h>
# include <io.h>
# include <stdio.h>
# include "jingxian/directory.h"
# include "jingxian/protocol/NullProtocol.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"
# include "jingxian/protocol/proxy/Proxy.h"


_jingxian_begin

namespace proxy
{

SOCKSv5Protocol:: SOCKSv5Protocol(Proxy* server)
        : server_(server)
        , status_(0)
        , credentialPolicy_(null_ptr)
        , connectProxy_(null_ptr)
{
    outgoing_.initialize(this);
    incoming_.initialize(this);

#ifdef DEBUG_TRACE
    sessionPath_ = combinePath(combinePath(server_->basePath(), _T("session")), ::toString((int)this) + _T(".txt"));
    sessionId_ = _tfopen(sessionPath_.c_str(), _T("w+"));
#endif
}

SOCKSv5Protocol::~SOCKSv5Protocol()
{
#ifdef DEBUG_TRACE
    if (null_ptr != sessionId_)
    {
        ::fclose(sessionId_);
        sessionId_ = null_ptr;
    }
    ::remove(sessionPath_);
#endif

    if (null_ptr != connectProxy_)
        connectProxy_->shutdown();
}

void SOCKSv5Protocol::writeIncoming(const std::vector<io_mem_buf>& buffers)
{
    incoming_.write(buffers);
}

void SOCKSv5Protocol::writeOutgoing(const std::vector<io_mem_buf>& buffers)
{
    outgoing_.write(buffers);
}

void SOCKSv5Protocol::onConnected(ProtocolContext& context)
{
#ifdef DEBUG_TRACE
    _fputts(context.transport().toString().c_str(), sessionId_);
    _fputts(_T("\r\n"), sessionId_);
    ::fflush(sessionId_);
#endif

    BaseProtocol::onConnected(context);
    status_ = 0;
}

void SOCKSv5Protocol::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
    if (incoming_.isActive())
    {
        incoming_.disconnection();
        return;
    }

    if (outgoing_.isActive())
    {
        outgoing_.disconnection();
        return;
    }

    delete this;
}

size_t SOCKSv5Protocol::onReceived(ProtocolContext& context)
{
    InBuffer inBuffer(&context.inMemory(), context.inBytes());
    size_t bytes = 0;

begin:

    size_t len = 0;

    switch (status_)
    {
    case 0: // INITIALIZE
        len = onHello(context, inBuffer);
        break;
    case 1://AUTHENTICATING:
        len = onAuthenticating(context, inBuffer);
        break;
    case 2:// COMMAND;
        len = onCommand(context, inBuffer);
        break;
    case 3:// CONNECTING;
        len = 0;
        break;
    case 8: //TRANSFORMING
        assert(false);
        break;
    }

    bytes += len;
    if (0 != len)
        goto begin;

    return bytes;
}

void SOCKSv5Protocol::onError(ProtocolContext& context)
{
    context.transport().disconnection();
}

size_t SOCKSv5Protocol::onHello(ProtocolContext& context, InBuffer& inBuffer)
{
    size_t length = inBuffer.size();
    if (length < 2)
        return 0;

    int8_t version = inBuffer.readInt8();
    size_t nmethods = inBuffer.readInt8();

    if (length < nmethods + 2)
        return 0;

    std::vector<int> methods(nmethods);

    for (size_t i = 0; i < nmethods; ++ i)
    {
        methods[i] = inBuffer.readInt8();
    }

    credentialPolicy_.reset(server_->credentials().GetCredential(methods));

    // 发送回复
    OutBuffer outBuffer(&context.transport());
    outBuffer.writeInt8(version);
    outBuffer.writeInt8(credentialPolicy_->authenticationType());
    status_ = 1; //AUTHENTICATING;
    LOG_TRACE(logger_, _T("握手成功!"));
    return 2 + nmethods;
}

size_t SOCKSv5Protocol::onAuthenticating(ProtocolContext& context, InBuffer& inBuffer)
{
    size_t len = credentialPolicy_->onReceived(context, inBuffer);
    if (credentialPolicy_->isComplete())
    {
        status_ = 2;// COMMAND;
        LOG_TRACE(logger_, _T("登录成功!"));
        return len;
    }

    return len;
}

bool readNetAddress(InBuffer& inBuffer, tstring& host, int af, size_t len)
{
    SOCKADDR addr;
    memset(&addr, 0, sizeof(addr));

    ((sockaddr_in*)&addr)->sin_family = af;
    inBuffer.readBlob(&(((sockaddr_in*)&addr)->sin_addr), len);
    ((sockaddr_in*)&addr)->sin_port = inBuffer.readInt16();

    return networking::addressToString(&addr, sizeof(addr), _T("tcp"), host);
}

size_t SOCKSv5Protocol::onCommand(ProtocolContext& context, InBuffer& inBuffer)
{
    //认证机制相关的子协商完成后，SOCKS Client提交转发请求:
    //+----+-----+-------+------+----------+----------+
    //|VER | CMD |  RSV  | ATYP | DST.ADDR | DST.PORT |
    //+----+-----+-------+------+----------+----------+
    //| 1  |  1  | X'00' |  1   | Variable |    2     |
    //+----+-----+-------+------+----------+----------+
    //VER         对于版本5这里是0x05
    //CMD         可取如下值:
    //            0x01    CONNECT
    //            0x02    BIND
    //            0x03    UDP ASSOCIATE
    //RSV         保留字段，必须为0x00
    //ATYP        用于指明DST.ADDR域的类型，可取如下值:
    //            0x01    IPv4地址
    //            0x03    FQDN(全称域名)
    //            0x04    IPv6地址
    //DST.ADDR    CMD相关的地址信息，不要为DST所迷惑
    //            如果是IPv4地址，这里是big-endian序的4字节数据
    //            如果是FQDN，比如"www.nsfocus.net"，这里将是:
    //            0F 77 77 77 2E 6E 73 66 6F 63 75 73 2E 6E 65 74
    //            注意，没有结尾的NUL字符，非ASCIZ串，第一字节是长度域
    //            如果是IPv6地址，这里是16字节数据。
    //DST.PORT    CMD相关的端口信息，big-endian序的2字节数据

    size_t length = inBuffer.size();
    if (length <= 4)
        return 0;
    char buf[4];
    inBuffer.readBlob(buf, 4);

    int bytes = 6;
    int addressType = buf[3];

    //char addr[1024];
    //char addressLen = 0;

    tstring host;
    switch (addressType)
    {

    case 1:
        bytes += 4;
        if (!readNetAddress(inBuffer, host, AF_INET, 4))
        {
            tstring err = concat<tstring>(_T("连接地址格式不正确 - "), lastError(WSAGetLastError()));
            LOG_ERROR(logger_, err);

            sendReply(context, SOCKSv5Error::Error, 5, addressType, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
            context.transport().disconnection(err);
            return 0;
        }
        break;
    case 4:
        bytes += 16;
        if (!readNetAddress(inBuffer, host, AF_INET6, 16))
        {
            tstring err = concat<tstring>(_T("连接地址格式不正确 - "), lastError(WSAGetLastError()));
            LOG_ERROR(logger_, err);
            sendReply(context, SOCKSv5Error::Error, 5, addressType, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 16, 0);
            context.transport().disconnection(err);
            return 0;
        }
        break;
    case 3:
    {
        int nameLen = inBuffer.readInt8();
        bytes += 1;
        bytes += nameLen;
        char buf[1024];
        inBuffer.readBlob(buf, nameLen);
        buf[nameLen] = 0;

        host = concat<tstring>(_T("tcp://"), toTstring(buf), _T(":"), ::toString(ntohs(inBuffer.readInt16())));
    }
    break;
    default:
        sendReply(context, SOCKSv5Error::NotSupportAddr, 5, 1, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
        context.transport().disconnection(concat<tstring>(_T("格式不正确，不可识别的地址类型 - addressType") , toString(), _T("!")));
        return 0;
    }

    if (inBuffer.fail())
        return 0;

    int8_t version = buf[0];
    int8_t cmd = buf[1];
    int8_t reserve = buf[2];

    switch (cmd)
    {
    case 0x01://    CONNECT
    {
        connectTo(context, host.c_str());
        status_ = 3; // CONNECTING;
        return bytes;
    }
    case 0x02://    BIND
    {
        sendReply(context, SOCKSv5Error::NotSupportCommand, 5, 1, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
        context.transport().disconnection();

        //listenOn( context.Reactor, new IPEndPoint(addr, port));
        status_ = 5; // WAITTING;
        return bytes;
    }
    case 0x03://    UDP ASSOCIATE
    {
        sendReply(context, SOCKSv5Error::NotSupportCommand, 5, 1, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
        context.transport().disconnection();
        //associating(addressType == 1)?AF_INET:AF_INET6, address, addressLen, host, port);
        return bytes;
    }
    default:
    {
        sendReply(context, SOCKSv5Error::NotSupportCommand, 5, 1, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
        context.transport().disconnection();
        return 0;
    }
    }
}

void SOCKSv5Protocol::connectTo(ProtocolContext& context, const tstring& host)
{
    connectProxy_ = new connectorType(this, host, &context.core(), &SOCKSv5Protocol::onConnectComplete, &SOCKSv5Protocol::onConnectError, context);
    connectProxy_->connectWith();

    LOG_TRACE(logger_, _T("发出连接请求!"));
}

//      public IProtocol BuildProtocol(ITransport transport, SOCKSv5 context)
//      {
//          return new SOCKSv5Outgoing(context);
//      }

void SOCKSv5Protocol::onConnectComplete(ITransport* transport, ProtocolContext& context)
{
#ifdef DEBUG_TRACE
    _fputts(transport->toString().c_str(), sessionId_);
    _fputts(_T("\r\n"), sessionId_);
    ::fflush(sessionId_);
#endif

    connectProxy_ = null_ptr;
    if (3 != status_)
    {
        static NullProtocol nullProtocol(true);
        transport->bindProtocol(&nullProtocol);
        LOG_TRACE(logger_, _T("连接请求返回但发现状态不对!"));
        return;
    }

    LOG_TRACE(logger_, _T("连接请求返回成功!"));

    transport->bindProtocol(&outgoing_);
    transport->initialize();


    sendReply(context, (int)SOCKSv5Error::Success, 5, 1, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
    context.transport().bindProtocol(&incoming_);
    incoming_.onConnected(context);

    status_ = 8; //TRANSFORMING;
}

void SOCKSv5Protocol::onConnectError(const ErrorCode&, ProtocolContext& context)
{
    LOG_TRACE(logger_, _T("连接请求返回失败!"));

    connectProxy_ = null_ptr;
    if (3 != status_)
        return;

    sendReply(context, SOCKSv5Error::Error, 5, 1, "\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0", 4, 0);
    context.transport().disconnection();
}

//      #endregion

//      #region Listening

//      public void ListenOn(IReactorCore reactor, IPEndPoint endPoint)
//      {
//          try
//          {
//              int port = _server.GetBindPort();
//              if (0 == port)
//              {
//                  SendReply((int)SOCKSv5Error.NetworkError);
//                  Transport.Disconnection( );
//                  return;
//              }
//              IProtocolFactory protocolFactory = new SocksProtocolFactory<SOCKSv5>(this.BuildIncomingProtocol, this, new TCPEndpoint(endPoint));
//              protocolFactory.Misc["Acceptor.WaitConnect"] = 1;
//              _acceptor = reactor.ListenWith(new TCPEndpoint(_server.BindIP, port),protocolFactory);
//              _acceptor.OnException += this.OnBindError;
//              _acceptor.OnTimeout += this.OnBindTimeout;
//              _acceptor.StartListening();

//              SendReply((int)SOCKSv5Error.Success, _server.BindIP, port);
//          }
//          catch (Exception e)
//          {
//              SendReply((int)SOCKSv5Error.NetworkError);
//              Transport.Disconnection(e);
//          }
//      }

//      public IProtocol BuildIncomingProtocol(ITransport transport, SOCKSv5 context, Endpoint expect)
//      {
//          IPAddress addr = IPAddress.Parse( expect.Host );

//          if (IPAddress.IsLoopback(addr) && transport.Peer.Host == _server.BindIP.ToString() )
//              return new SOCKSv5Incoming(context);

//
//          if (expect.Host != transport.Peer.Host)
//              return null;

//          return new SOCKSv5Incoming(context);
//      }

//      public void OnBindSuccess(SOCKSv5Incoming incoming)
//      {
//          _acceptor.StopListening();
//          _server.ReleaseBindPort(((TCPEndpoint)_acceptor.BindPoint).Port);
//          _acceptor = null;

//          _peer = incoming;

//          TCPEndpoint endpoint = incoming.Transport.Host as TCPEndpoint;

//          SendReply((int)SOCKSv5Error.Success, IPAddress.Parse(endpoint.Host), endpoint.Port);
//      }

//      public void OnBindTimeout( IAcceptor acceptor)
//      {
//          _acceptor.StopListening();
//          _server.ReleaseBindPort(((TCPEndpoint)_acceptor.BindPoint).Port);
//          _acceptor = null;
//          SendReply((int)SOCKSv5Error.HostError);
//          Transport.Disconnection();
//      }

//      public void OnBindError(Exception e)
//      {
//          _acceptor.StopListening();
//          _server.ReleaseBindPort(((TCPEndpoint)_acceptor.BindPoint).Port);
//          _acceptor = null;
//          SendReply((int)SOCKSv5Error.HostError);
//          Transport.Disconnection(e);
//      }

//      #endregion

//      #region Associating

//      public void Associating(IPEndPoint endPoint)
//      {
//          //    serv = reactor.listenTCP(port, klass(*args))
//          //    return defer.succeed(serv.getHost()[1:])
//      }

//      #endregion

Proxy* SOCKSv5Protocol::internalCore()
{
    return server_;
}

void SOCKSv5Protocol::sendReply(ProtocolContext& context, int reply, int version, int addressType, const char* addr, size_t len, int port)
{
    /// +----+-----+-------+------+----------+----------+
    /// |VER | REP |  RSV  | ATYP | BND.ADDR | BND.PORT |
    /// +----+-----+-------+------+----------+----------+
    /// | 1  |  1  | X'00' |  1   | Variable |    2     |
    /// +----+-----+-------+------+----------+----------+

    OutBuffer out(&context.transport());
    out.writeInt8(version);
    out.writeInt8(reply);
    out.writeInt8(0);
    out.writeInt8(addressType);

    switch (addressType)
    {
    case 1:
        out.writeBlob(addr, 4);
        break;
    case 4:
        out.writeBlob(addr, 16);
        break;
    case 3:
        out.writeInt8(len);
        out.writeBlob(addr, len);
        break;
    default:
        assert(false);
    }


    out.writeInt16(htons(port));
}

}

_jingxian_end
