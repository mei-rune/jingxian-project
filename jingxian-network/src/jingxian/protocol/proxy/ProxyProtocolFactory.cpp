
# include "pro_config.h"
# include "jingxian/directory.h"
# include "jingxian/string/string.h"
# include "jingxian/protocol/proxy/BaseCredentialPolicy.h"
# include "jingxian/protocol/proxy/NullCredentialPolicy.h"
# include "jingxian/protocol/proxy/ProxyProtocolFactory.h"



_jingxian_begin

namespace proxy
{

	ProxyProtocolFactory::ProxyProtocolFactory(const tstring& basePath)
            : toString_(_T("socks 代理"))
    {
        path_ = combinePath(basePath, _T("log"));
        if (!existDirectory(path_))
            createDirectory(path_);

        path_ = combinePath(path_, _T("proxy"));
        if (!existDirectory(path_))
            createDirectory(path_);

        if (!existDirectory(combinePath(path_, _T("session"))))
            createDirectory(combinePath(path_, _T("session")));

        credentials_.policies().push_back(new NullCredentialPolicyFactory(this));
        credentials_.policies().push_back(new BaseCredentialPolicyFactory(this, AuthenticationType::BASE, _T("BASE"), _T("")));
        credentials_.policies().push_back(new BaseCredentialPolicyFactory(this, AuthenticationType::GSSAPI, _T("GSSAPI"), _T("")));
    }

    IProtocol* ProxyProtocolFactory::createProtocol(ITransport* transport, IReactorCore* core)
    {
        return new SOCKSv5Protocol(this);
    }
	
	bool ProxyProtocolFactory::configure(configure::Context& context, const tstring& t)
	{
		StringArray<tstring::value_type> sa = split(t.c_str()
		  , _T(" \t")
		  , StringSplitOptions::RemoveEmptyEntries);

		if(0 == sa.size())
			return true;

		if(0 == string_traits<tstring::value_type>::stricmp(_T("user"), sa.ptr(0)))
		{  
			if(3 > sa.size())
			{
				LOG_FATAL(context.logger(), _T("命令 'user' 格式不正确"));
				context.exit();
				return true;
			}

			users_[sa.ptr(1)] = sa.ptr(2);
			return true;
		}

		return false;
	}

    const tstring& ProxyProtocolFactory::basePath() const
    {
        return path_;
    }

    proxy::Credentials&  ProxyProtocolFactory::credentials()
    {
        return credentials_;
    }

    const tstring& ProxyProtocolFactory::toString() const
    {
        return toString_;
    }
}

_jingxian_end
