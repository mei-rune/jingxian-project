
# include "pro_config.h"
# include "jingxian/protocol/proxy/SOCKSv5Outgoing.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"
# include "jingxian/buffer/OutBuffer.h"

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

void SOCKSv5Outgoing::write(const std::vector<io_mem_buf>& buffers)
{
	OutBuffer out(transport_);
	for(std::vector<io_mem_buf>::const_iterator it = buffers.begin()
		; it != buffers.end()
		; ++ it )
	out.writeBlob(it->buf, it->len);
}

void SOCKSv5Outgoing::disconnection()
{
	if(null_ptr == transport_)
		return;
	transport_->disconnection();
	transport_ = null_ptr;
}

bool SOCKSv5Outgoing::isActive() const
{
	return null_ptr != transport_;
}

size_t SOCKSv5Outgoing::onReceived(ProtocolContext& context)
{
	socks_->writeIncoming(context.inMemory());
	return context.inBytes();
}

void SOCKSv5Outgoing::onConnected(ProtocolContext& context)
{
	BaseProtocol::onConnected(context);
	transport_ = &(context.transport());
}

void SOCKSv5Outgoing::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
	transport_ = null_ptr;
	context.transport().bindProtocol(null_ptr);
	socks_->onDisconnected(context, errCode, reason);
}

}

_jingxian_end
