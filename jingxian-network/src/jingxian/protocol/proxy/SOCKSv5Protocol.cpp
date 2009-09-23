
# include "pro_config.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"

_jingxian_begin

namespace proxy
{

SOCKSv5Protocol:: SOCKSv5Protocol(Proxy* server)
: _server(server)
, _credentialPolicy(null_ptr)
{
	outgoing.initialize(this);
	incoming.initialize(this);
}


void SOCKSv5Protocol::writeIncoming(const std::vector<io_mem_buf>& buffers)
{
	incoming.write(buffers);
}

void SOCKSv5Protocol::writeOutgoing(const std::vector<io_mem_buf>& buffers)
{
	outgoing.write(buffers);
}

void SOCKSv5Protocol::onConnected(ProtocolContext& context)
{
	onConnected(context);
	_dataReceived = this.OnHello;
}

size_t SOCKSv5Protocol::onReceived(ProtocolContext& context)
{
	InBuffer inBuffer;
	inBuffer.reset(&context.inMemory(), context.inBytes());
	size_t length = inBuffer.rawLength();
	if (length < 2)
		return 0;

	TranscationScope<InBuffer, int> scope(inBuffer);


	if (length < (((size_t)_buffer[1]) + 2))
		return 0;

	int version = (int)_buffer.ReadByte();
	int nmethods = (int)_buffer.ReadByte();
	int[] methods = new int[nmethods];
	for (int i = 0; i < nmethods; i++)
	{
		methods[i] = (int)_buffer.ReadByte();
	}

	_credentialPolicy = _server.Credentials.GetCredential(methods);
	SendCredentialReply(context, version, _credentialPolicy.AuthenticationType);
	_dataReceived = this.OnAuthenticating;

	/// 不管有没有数据，马上触发一次，因为无需授权方式，是不需要数据就要跳转
	_dataReceived(context, _buffer);

	scope.commit();
}

void SOCKSv5Protocol::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
	if (null_ptr != _peer)
		_peer.transport().disconnection();

	if (null != _acceptor)
	{
		_acceptor->stopListening();
		//_server.ReleaseBindPort(((TCPEndpoint)_acceptor.BindPoint).Port);
		_acceptor = null_ptr;
	}
}


  //      public void OnError(ProtocolContext context, IOBuffer IOBuffer)
  //      {
  //          _peer.Transport.Disconnection();
  //      }

  //      void OnHello(ProtocolContext context, IOBuffer IOBuffer)
  //      {
  //          if (null == _buffer || 0 == _buffer.Count)
  //              _buffer = IOBuffer;
  //          else
  //              _buffer = _buffer.Join(IOBuffer);

  //          int length = _buffer.Count;
  //          if (length < 2)
  //              return;

  //          if (length < (((int)_buffer[1]) + 2))
  //              return;

  //          int version = (int)_buffer.ReadByte();
  //          int nmethods = (int)_buffer.ReadByte();
  //          int[] methods = new int[nmethods];
  //          for (int i = 0; i < nmethods; i++)
  //          {
  //              methods[i] = (int)_buffer.ReadByte();
  //          }

  //          _credentialPolicy = _server.Credentials.GetCredential(methods);
  //          SendCredentialReply(context, version, _credentialPolicy.AuthenticationType);
  //          _dataReceived = this.OnAuthenticating;

  //          /// 不管有没有数据，马上触发一次，因为无需授权方式，是不需要数据就要跳转
  //          _dataReceived(context, _buffer);
  //      }

  //      public void OnAuthenticating(ProtocolContext context, IOBuffer IOBuffer)
  //      {

  //          try
  //          {
  //              if (null == _buffer || 0 == _buffer.Count)
  //                  _buffer = IOBuffer;
  //              else
  //                  _buffer = _buffer.Join(IOBuffer);

  //              _credentialPolicy.OnReceived(context, _buffer);
  //              if (_credentialPolicy.Complete)
  //              {
  //                  _dataReceived = this.OnCommand;

  //                  /// 可能有数据，马上触发一次
  //                  if (0 != _buffer.Count )
  //                      _dataReceived(context, _buffer);
  //                  return;
  //              }
  //              return;
  //          }
  //          catch (Exception e)
  //          {
  //              Transport.Disconnection(e);
  //              _dataReceived = this.OnError;
  //          }
  //      }

  //      public void SendCredentialReply(ProtocolContext context, int version, int authenticationType)
  //      {
  //          byte[] buffer = new byte[2];
  //          buffer[0] = (byte)version;
  //          buffer[1] = (byte)authenticationType;

  //          context.Transport.Write(buffer);
  //      }


  //      public void OnCommand(ProtocolContext context, IOBuffer IOBuffer)
  //      {
  //          //认证机制相关的子协商完成后，SOCKS Client提交转发请求:
  //          //+----+-----+-------+------+----------+----------+
  //          //|VER | CMD |  RSV  | ATYP | DST.ADDR | DST.PORT |
  //          //+----+-----+-------+------+----------+----------+
  //          //| 1  |  1  | X'00' |  1   | Variable |    2     |
  //          //+----+-----+-------+------+----------+----------+
  //          //VER         对于版本5这里是0x05
  //          //CMD         可取如下值:
  //          //            0x01    CONNECT
  //          //            0x02    BIND
  //          //            0x03    UDP ASSOCIATE
  //          //RSV         保留字段，必须为0x00
  //          //ATYP        用于指明DST.ADDR域的类型，可取如下值:
  //          //            0x01    IPv4地址
  //          //            0x03    FQDN(全称域名)
  //          //            0x04    IPv6地址
  //          //DST.ADDR    CMD相关的地址信息，不要为DST所迷惑
  //          //            如果是IPv4地址，这里是big-endian序的4字节数据
  //          //            如果是FQDN，比如"www.nsfocus.net"，这里将是:
  //          //            0F 77 77 77 2E 6E 73 66 6F 63 75 73 2E 6E 65 74
  //          //            注意，没有结尾的NUL字符，非ASCIZ串，第一字节是长度域
  //          //            如果是IPv6地址，这里是16字节数据。
  //          //DST.PORT    CMD相关的端口信息，big-endian序的2字节数据

  //          if (null == _buffer || 0 == _buffer.Count )
  //              _buffer = IOBuffer;
  //          else
  //              _buffer = _buffer.Join(IOBuffer);

  //          int length = _buffer.Count;
  //          if (length <= 4)
  //              return;

  //          int expectLength = 6;
  //          switch (_buffer[3])
  //          {
  //              case 1:
  //                  expectLength += 4;
  //                  break;
  //              case 3:
  //                  expectLength += (int)_buffer[4];
  //                  break;
  //              case 4:
  //                  expectLength += 16;
  //                  break;
  //              default:

  //                  SendReply((int)SOCKSv5Error.NotSupportAddr);
  //                  Transport.Disconnection(new RuntimeError("格式不正确，不可识别的地址类型[{0}]!", _buffer[3]));
  //                  break;
  //                  //throw new RuntimeError("格式不正确，不可识别的地址类型[{0}]!", _buffer[3]);
  //          }

  //          if (length < expectLength)
  //              return;

  //          int version = (int)_buffer.ReadByte();
  //          int cmd = (int)_buffer.ReadByte();
  //          int reserve = (int)_buffer.ReadByte();
  //          int addrType = (int)_buffer.ReadByte();

  //          IPAddress addr = null;
  //          string host = string.Empty;
  //          switch (addrType)
  //          {
  //              case 1:
  //                  addr = new IPAddress(_buffer.ReadBytes(4));
  //                  break;
  //              case 3:
  //                  int hostLength = (int)_buffer.ReadByte();
  //                  host = Encoding.ASCII.GetString(_buffer.ReadBytes(hostLength-1));
  //                  break;
  //              case 4:
  //                  addr = new IPAddress(_buffer.ReadBytes(16));
  //                  break;
  //          }
  //          int port = IPAddress.NetworkToHostOrder(Helper.ReadInt16(_buffer));

  //          switch (cmd)
  //          {
  //              case 0x01://    CONNECT
  //                  {
  //                      if (null != addr)
  //                      {
  //                          connectTo(context.Reactor, new IPEndPoint(addr, port));
  //                      }
  //                      else
  //                      {
  //                          connectTo(context.Reactor, host, port);
  //                      }
  //                      break;
  //                  }

  //              case 0x02://    BIND
  //                  {
  //                      this.ListenOn( context.Reactor, new IPEndPoint(addr, port));
  //                      break;
  //                  }

  //              case 0x03://    UDP ASSOCIATE
  //                  {
  //                      this.Associating(new IPEndPoint(addr, port));
  //                      break;
  //                  }
  //              default:
  //                  {
  //                      SendReply((int)SOCKSv5Error.NotSupportCommand);
  //                      break;
  //                  }
  //          }
  //      }

  //      public void OnTransforming(ProtocolContext context, IOBuffer IOBuffer)
  //      {
  //          _peer.Write( IOBuffer);
  //      }

  //      public void OnResolveHost(string name, IPAddress[] addr, SOCKSProtocol.DNSContext context)
  //      {
  //          connectTo(context.Core, new IPEndPoint(addr[0], context.Port));
  //      }

  //      public void OnResolveError(string name, SOCKSProtocol.DNSContext context)
  //      {
  //          SendReply((int)SOCKSv5Error.HostError);
  //          Transport.Disconnection();
  //      }

  //      public void connectTo(IReactorCore reactor, string host, int port)
  //      {
  //          IDNSResolver resolver = (IDNSResolver)reactor.GetService(typeof(IDNSResolver));
  //          resolver.ResolveHostByName<SOCKSProtocol.DNSContext>(host
  //              , new SOCKSProtocol.DNSContext( reactor, host, port )
  //              , reactor
  //              , this.OnResolveHost, this.OnResolveError);
  //      }

  //      public void connectTo(IReactorCore reactor, IPEndPoint endPoint)
  //      {
  //          reactor.ConnectWith(new TCPEndpoint(endPoint)).Connect<SOCKSv5>(this.BuildProtocol
  //              , this.OnConnectError, this);
  //      }

  //      public IProtocol BuildProtocol(ITransport transport, SOCKSv5 context)
  //      {
  //          return new SOCKSv5Outgoing(context);
  //      }

  //      public void OnConnectSuccess(SOCKSv5 context, SOCKSv5Outgoing peer, TCPEndpoint bindAddr)
  //      {
  //          context.SendReply((int)SOCKSv5Error.Success, IPAddress.Parse( bindAddr.Host  ), bindAddr.Port);
  //          _peer = peer;
  //          _dataReceived = this.OnTransforming;
  //      }

  //      public void OnConnectError(SOCKSv5 context, Exception exception)
  //      {
  //          context.SendReply((int)SOCKSv5Error.Error);
  //          context.Transport.Disconnection();
  //      }

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

  //      public void SendReply(int reply)
  //      {
  //          SendReply(reply, 5, IPAddress.Any, 0);
  //      }

  //      public void SendReply(int reply, IPAddress addr, int port)
  //      {
  //          SendReply(reply, 5, addr, port);
  //      }

  //      public void SendReply(int reply, int version, IPAddress ip, int port)
  //      {
  //          /// +----+-----+-------+------+----------+----------+
  //          /// |VER | REP |  RSV  | ATYP | BND.ADDR | BND.PORT |
  //          /// +----+-----+-------+------+----------+----------+
  //          /// | 1  |  1  | X'00' |  1   | Variable |    2     |
  //          /// +----+-----+-------+------+----------+----------+

  //          IOBuffer buffer = new IOBuffer( 100 );
  //          buffer.Append((byte)version);
  //          buffer.Append((byte)reply);
  //          buffer.Append((byte)0);

  //          byte[] addr = ip.GetAddressBytes();

  //          if (4 == addr.Length)
  //          {
  //              buffer.WriteByte((byte)1);
  //          }
  //          else if (16 == addr.Length)
  //          {
  //              buffer.WriteByte((byte)4);
  //          }
  //          else
  //          {
  //              throw new RuntimeError("取地址出错，发现[{0}]不是4或16字节", ip);
  //          }
  //          
  //          buffer.WriteBytes(addr);

  //          Helper.Write(buffer, IPAddress.HostToNetworkOrder( (short)port) );
  //          Transport.Write( buffer );
  //      }
		//private:
		//	
  //      IProtocol* _peer = null;
  //      DataReceived _dataReceived = null;
  //      ICredentialPolicy _credentialPolicy = null;
  //      Proxy _server = null;
  //  };

}

_jingxian_end
