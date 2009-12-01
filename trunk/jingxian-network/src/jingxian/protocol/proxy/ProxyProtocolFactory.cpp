
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
			if(3 != sa.size())
			{
				LOG_FATAL(context.logger(), _T("命令 'user' 格式不正确"));
				context.exit();
				return true;
			}

			users_[sa.ptr(1)] = sa.ptr(2);
			return true;
		}

		if(0 == string_traits<tstring::value_type>::stricmp(_T("credentialPolicy"), sa.ptr(0)))
		{  
			if(2 > sa.size())
			{
				LOG_FATAL(context.logger(), _T("命令 'credentialPolicy' 格式不正确"));
				context.exit();
				return true;
			}

			if(appendCredentialPolicy(sa))
			{
				LOG_FATAL(context.logger(), _T("命令 'credentialPolicy' 执行失败"));
				context.exit();
				return false;
			}
			return true;
		}

		return false;
	}

	bool ProxyProtocolFactory::appendCredentialPolicy(const StringArray<tstring::value_type>& sa)
	{
		if(0 == string_traits<tstring::value_type>::stricmp(_T("None"), sa.ptr(1)))
		{
			credentials_.policies().push_back(new NullCredentialPolicyFactory(this));
			return true;
		}
		if(0 == string_traits<tstring::value_type>::stricmp(_T("BASE"), sa.ptr(1)))
		{
			credentials_.policies().push_back(new BaseCredentialPolicyFactory(this, AuthenticationType::BASE, _T("BASE"), _T("")));
			return true;
		}
		//if(0 == string_traits<tstring::value_type>::stricmp(_T("GSSAPI"), ptr))
		//{
		//	credentials_.policies().push_back(new BaseCredentialPolicyFactory(this, AuthenticationType::GSSAPI, _T("GSSAPI"), _T("")));
		//	return true;
		//}
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
