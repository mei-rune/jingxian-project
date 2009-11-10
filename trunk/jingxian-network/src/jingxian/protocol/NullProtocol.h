
#ifndef _NullProtocol_H_
#define _NullProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"

_jingxian_begin

class NullProtocol : public BaseProtocol
{
public:
    NullProtocol(bool unBind)
            : BaseProtocol(_T("NullProtocol"))
			, unBind_(unBind)
    {
    }

    virtual void onConnected(ProtocolContext& context)
    {
        context.transport().disconnection();
    }

    virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason)
    {
        LOG_INFO(logger_, _T("NullProtocol 新连接到来 - ") << context.transport().peer());

        if (unBind_)
            context.transport().bindProtocol(null_ptr);
    }

private:
    NOCOPY(NullProtocol);
    bool unBind_;
};

_jingxian_end

#endif //_NullProtocol_H_