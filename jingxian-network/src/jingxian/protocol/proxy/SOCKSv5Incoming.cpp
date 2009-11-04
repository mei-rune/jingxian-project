
# include "pro_config.h"
# include "jingxian/protocol/proxy/SOCKSv5Incoming.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"
# include "jingxian/protocol/proxy/Proxy.h"
# include "jingxian/buffer/OutBuffer.h"
# include "jingxian/directory.h"

_jingxian_begin

namespace proxy
{

SOCKSv5Incoming::SOCKSv5Incoming()
        : socks_(null_ptr)
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
    for (std::vector<io_mem_buf>::const_iterator it = buffers.begin()
            ; it != buffers.end()
            ; ++ it)
    {
        out.writeBlob(it->buf, it->len);
#ifdef DUMPFILE
        (*os) << std::string(it->buf, it->len);
        os->flush();
#endif
    }
}

void SOCKSv5Incoming::disconnection()
{
    if (null_ptr == transport_)
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
#ifdef DUMPFILE
    for (std::vector<io_mem_buf>::const_iterator it = context.inMemory().begin()
            ; it != context.inMemory().end()
            ; ++ it)
    {
        *is << std::string(it->buf, it->len);
        is->flush();
    }
#endif

    socks_->writeOutgoing(context.inMemory());
    return context.inBytes();
}



void SOCKSv5Incoming::onConnected(ProtocolContext& context)
{
    BaseProtocol::onConnected(context);
    transport_ = &context.transport();

#ifdef DUMPFILE
    static int id = 0;
    os.reset(new std::ofstream(toNarrowString(simplify(combinePath(combinePath(socks_->internalCore()->basePath(), _T("session")), concat<tstring>(_T("incoming_os_"), ::toString(++id), _T(".txt"))))).c_str()));
    *os <<  toNarrowString(context.transport().toString())  << std::endl;

    is.reset(new std::ofstream(toNarrowString(simplify(combinePath(combinePath(socks_->internalCore()->basePath(), _T("session")), concat<tstring>(_T("incoming_is_"), ::toString(id), _T(".txt"))))).c_str()));
    *is  <<  toNarrowString(context.transport().toString()) << std::endl;
#endif
}

void SOCKSv5Incoming::onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
{
    transport_ = null_ptr;
    context.transport().bindProtocol(null_ptr);
    socks_->onDisconnected(context, errCode, reason);
}

}

_jingxian_end