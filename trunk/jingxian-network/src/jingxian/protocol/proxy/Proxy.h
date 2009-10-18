
#ifndef _Proxy_H_
#define _Proxy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <list>
# include "jingxian/directory.h"
# include "jingxian/AbstractServer.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/protocol/proxy/ICredentialPolicy.h"
# include "jingxian/protocol/proxy/Credentials.h"
# include "jingxian/protocol/proxy/BaseCredentialPolicy.h"
# include "jingxian/protocol/proxy/NullCredentialPolicy.h"
# include "jingxian/protocol/proxy/config/Configuration.h"
# include "jingxian/protocol/proxy/SOCKSv5Protocol.h"



_jingxian_begin

namespace proxy
{
	struct BindPort
	{
		bool IsUsed;
		int Port;

		BindPort()
		{
			IsUsed = false;
			Port = 0;
		}
	};

	class BindPorts
	{
	public:
		BindPorts(int begin, int end)
			: _begin(begin)
			, _end(end)
			, _position(-1)
		{
			if (begin > end)
				ThrowException1(IllegalArgumentException, _T("begin ���ܱ� end ��Ҫ��!"));

			//_ports = new BindPort[ Math.Min( 1000,_end - _begin  ) ];

			BindPort bindPort;
			for (int i = _begin; i <= _end; ++ i)
			{
				bindPort.IsUsed = false;
				bindPort.Port = i;

				_ports.push_back(bindPort);
			}
		}

		int GetPort()
		{
			for (size_t i = 0; i < _ports.size(); ++ i)
			{
				if (!_ports[++_position % _ports.size()].IsUsed)
					return _ports[_position % _ports.size()].Port;
			}
			return 0;
		}

		void ReleasePort( int port)
		{
			if( _begin > port || _end < port )
				return;

			_ports[ port - _begin ].IsUsed = false;
		}

	private:

		int _begin;
		int _end;

		std::vector<BindPort> _ports;
		int _position;
	};

	class Proxy  : public AbstractServer
	{
	public:
		Proxy(IOCPServer& core, const tstring& addr)
			: AbstractServer( &core )
		{
			_toString = _T("socks ����");
			if(!this->initialize(addr))
			{
				LOG_FATAL(log(), _T("��ʼ�������ʧ��"));
				return;
			}

			//_allowedIPs = ParseIPSeg( config.AllowedIPs );
			//_blockingIPs = ParseIPSeg(config.BlockingIPs );


			path_ = simplify(combinePath(getApplicationDirectory(), _T("log")));
			if(!existDirectory(path_))
				createDirectory(path_);

			path_ = combinePath(path_, _T("proxy"));
			if(!existDirectory(path_))
				createDirectory(path_);

			if(!existDirectory(combinePath(path_, _T("session"))))
				createDirectory(combinePath(path_, _T("session")));

			_credentials.policies().push_back(new NullCredentialPolicyFactory(this));
			_credentials.policies().push_back(new BaseCredentialPolicyFactory(this, AuthenticationType::BASE, _T("BASE"), _T("")));
			_credentials.policies().push_back(new BaseCredentialPolicyFactory(this, AuthenticationType::GSSAPI, _T("GSSAPI"), _T("")));

			acceptor_.accept(this, &Proxy::onComplete, &Proxy::onError, &core);
		}

		void onComplete(ITransport* transport, IOCPServer* core)
		{
			std::auto_ptr<SOCKSv5Protocol> protocol(new SOCKSv5Protocol(this));
			transport->bindProtocol(protocol.get());
			protocol.release();

			acceptor_.accept(this, &Proxy::onComplete, &Proxy::onError, core);
		}

		void onError(const ErrorCode& err, IOCPServer* core)
		{
			acceptor_.accept(this, &Proxy::onComplete, &Proxy::onError, core);
		}

		//bool IsBlockingIP(const tstring& ip)
		//{
		//	if (null == _blockingIPs)
		//		return false;

		//	foreach (IPSeg seg in _blockingIPs)
		//	{
		//		if (seg.In(ip)) return true;
		//	}
		//	return false;
		//}


		//bool IsAllowedIP(IPAddress ip)
		//{
		//    if (IsBlockingIP(ip))
		//        return false;

		//    if (null == _allowedIPs)
		//        return true;

		//    foreach (IPSeg seg in _allowedIPs)
		//    {
		//        if (seg.In(ip)) return true;
		//    }
		//    return false;
		//}

		const tstring& basePath() const
		{
			return path_;
		}

		proxy::Credentials&  credentials()
		{
			return _credentials;
		}

	private:
		proxy::Credentials _credentials;
		tstring path_;
	};
}

_jingxian_end

#endif // _Proxy_H_