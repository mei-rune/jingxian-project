
# include "pro_config.h"
# include "jingxian/protocol/proxy/SOCKSv5Outgoing.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"

_jingxian_begin

namespace proxy
{

SOCKSv5Outgoing::SOCKSv5Outgoing()
: socks_(null_ptr)
, transport_(null_ptr)
{
}

void SOCKSv5Outgoing::initialize(SOCKSv5Protocol* socks)
{
	socks_ = socks;
}


void SOCKSv5Outgoing::write(const std::vector<io_mem_buf>& bufs)
{
	transport_->
}

size_t SOCKSv5Outgoing::onReceived(ProtocolContext& context)
{
	socks_->writeIncoming(context.inMemory());
	return context.inBytes();
}

void SOCKSv5Outgoing::onConnected(ProtocolContext& context)
{
	onConnected(context);
	transport_ = &(context.transport());
	socks_->OnConnectSuccess(_socks, this, context.transport().host());
}

void SOCKSv5Outgoing::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
	socks_->disconnection();
}

}

_jingxian_end
