
# include "pro_config.h"
# include "jingxian/protocol/proxy/SOCKSv5Incoming.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"
# include "jingxian/buffer/OutBuffer.h"

_jingxian_begin

namespace proxy
{

SOCKSv5Incoming::SOCKSv5Incoming()
:socks_(null_ptr)
, transport_(null_ptr)
{
}

void SOCKSv5Incoming::initialize(SOCKSv5Protocol* socks)
{
	socks_ = socks;
}

void SOCKSv5Incoming::write(const std::vector<io_mem_buf>& buffers)
{
	OutBuffer out(transport_);
	for(std::vector<io_mem_buf>::const_iterator it = buffers.begin()
		; it != buffers.end()
		; ++ it )
	out.writeBlob(it->buf, it->len);
}

void SOCKSv5Incoming::disconnection()
{
	if(null_ptr == transport_)
		return;
	transport_->disconnection();
	transport_ = null_ptr;
}

bool SOCKSv5Incoming::isActive() const
{
	return null_ptr != transport_;
}

size_t SOCKSv5Incoming::onReceived(ProtocolContext& context)
{
	socks_->writeOutgoing(context.inMemory());
	return context.inBytes();
}

void SOCKSv5Incoming::onConnected(ProtocolContext& context)
{
	BaseProtocol::onConnected(context);
	transport_ = &context.transport();
}

void SOCKSv5Incoming::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
	transport_ = null_ptr;
	context.transport().bindProtocol(null_ptr);
	socks_->onDisconnected(context, errCode, reason);
}

}

_jingxian_end