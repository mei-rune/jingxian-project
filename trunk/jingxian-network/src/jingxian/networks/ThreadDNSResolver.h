
#ifndef _ThreadDNSResolver_H_
#define _ThreadDNSResolver_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"
# include "jingxian/IDNSResolver.h"
# include "jingxian/IReactorCore.h"

_jingxian_begin

class ThreadDNSResolver :
        public IDNSResolver
{
public:
    ThreadDNSResolver();

    void initialize(IReactorCore* core);

    virtual ~ThreadDNSResolver(void);

    virtual void ResolveHostByName(const tchar* name
                                   , const tchar* port
                                   , void* context
                                   , ResolveComplete callback
                                   , ResolveError onError
                                   , int timeout);
private:
    IReactorCore* core_;
};

_jingxian_end

#endif //_ThreadDNSResolver_H_