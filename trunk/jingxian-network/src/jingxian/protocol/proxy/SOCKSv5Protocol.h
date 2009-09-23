
#ifndef _SOCKSv5Protocol_H_
#define _SOCKSv5Protocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/buffer/OutBuffer.h"
# include "jingxian/protocol/BaseProtocol.h"
# include "jingxian/protocol/proxy/SOCKSv5Incoming.h"
# include "jingxian/protocol/proxy/SOCKSv5Outgoing.h"
# include "jingxian/protocol/proxy/ICredentialPolicy.h"

_jingxian_begin

namespace proxy
{
	class Proxy;

	class SOCKSv5Protocol : public BaseProtocol
	{
	public :
		SOCKSv5Protocol(Proxy* server);

		virtual void onConnected(ProtocolContext& context);
		virtual size_t onReceived(ProtocolContext& context);	
		virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason);

		virtual void writeIncoming(const std::vector<io_mem_buf>& buf);
		virtual void writeOutgoing(const std::vector<io_mem_buf>& buf);

		void disconnection();

		//void OnError(ProtocolContext context, IOBuffer IOBuffer);
		//void OnHello(ProtocolContext context, IOBuffer IOBuffer);
		//void OnAuthenticating(ProtocolContext context, IOBuffer IOBuffer);
		//void SendCredentialReply(ProtocolContext context, int version, int authenticationType);
		//void OnCommand(ProtocolContext context, IOBuffer IOBuffer);
		//void OnTransforming(ProtocolContext context, IOBuffer IOBuffer);
		//void OnResolveHost(string name, IPAddress[] addr, SOCKSProtocol.DNSContext context);
		//void OnResolveError(string name, SOCKSProtocol.DNSContext context);
		//void connectTo(IReactorCore reactor, string host, int port);
		//void connectTo(IReactorCore reactor, IPEndPoint endPoint);
		//IProtocol BuildProtocol(ITransport transport, SOCKSv5 context);
		//void OnConnectSuccess(SOCKSv5 context, SOCKSv5Outgoing peer, TCPEndpoint bindAddr);
		//void OnConnectError(SOCKSv5 context, Exception exception);
		//void ListenOn(IReactorCore reactor, IPEndPoint endPoint);
		//IProtocol BuildIncomingProtocol(ITransport transport, SOCKSv5 context, Endpoint expect);
		//void OnBindSuccess(SOCKSv5Incoming incoming);
		//void OnBindTimeout( IAcceptor acceptor);
		//void OnBindError(Exception e);
		//void Associating(IPEndPoint endPoint);
		//void SendReply(int reply);
		//void SendReply(int reply, IPAddress addr, int port);
		//void SendReply(int reply, int version, IPAddress ip, int port);
	private:
		Proxy* _server;
		proxy::ICredentialPolicy* _credentialPolicy;
		SOCKSv5Outgoing outgoing;
		SOCKSv5Incoming incoming;
	};
}

_jingxian_end

#endif //_SOCKSv5Protocol_H_