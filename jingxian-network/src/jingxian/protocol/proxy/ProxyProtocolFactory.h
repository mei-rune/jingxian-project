
#ifndef _ProxyProtocolFactory_H_
#define _ProxyProtocolFactory_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <map>
# include <list>
# include "jingxian/protocol/proxy/ICredentialPolicy.h"
# include "jingxian/protocol/proxy/Credentials.h"
# include "jingxian/protocol/proxy/config/Configuration.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"



_jingxian_begin

namespace proxy
{

class ProxyProtocolFactory  : public IProtocolFactory
{
public:
    ProxyProtocolFactory(const tstring& basePath);

    virtual IProtocol* createProtocol(ITransport* transport, IReactorCore* core);
	
	virtual bool configure(configure::Context& context, const tstring& t);

    const tstring& basePath() const;

    proxy::Credentials&  credentials();

    virtual const tstring& toString() const;

private:
	NOCOPY(ProxyProtocolFactory);
    tstring toString_;
    proxy::Credentials credentials_;
    tstring path_;
	std::map<tstring, tstring> users_;
};
}

_jingxian_end

#endif // _ProxyProtocolFactory_H_