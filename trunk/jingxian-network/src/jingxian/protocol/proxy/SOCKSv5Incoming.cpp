
# include "pro_config.h"
# include "jingxian/protocol/proxy/SOCKSv5Incoming.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"

_jingxian_begin

namespace proxy
{

SOCKSv5Incoming::SOCKSv5Incoming()
:_socks(null_ptr)
{
}

void SOCKSv5Incoming::initialize(SOCKSv5Protocol* socks)
{
	_socks = socks;
}

size_t SOCKSv5Incoming::onReceived(ProtocolContext& context)
{
	_socks->writeOutgoing(context.inMemory());
	return context.inBytes();
}

void SOCKSv5Incoming::onConnected(ProtocolContext& context)
{
	//ITCPTransport transport = context.Transport as ITCPTransport;
	onConnected(context);
	_socks->OnBindSuccess(this);
}

void SOCKSv5Incoming::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
	_socks->disconnection();
}

}

_jingxian_end