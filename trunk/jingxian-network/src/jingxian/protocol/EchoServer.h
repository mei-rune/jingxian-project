
#ifndef _EchoServer_H_
#define _EchoServer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <list>
# include "jingxian/directory.h"
# include "jingxian/AbstractServer.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/protocol/EchoProtocol.h"



_jingxian_begin


class EchoServer : public AbstractServer
{
public:
	EchoServer(IOCPServer& core)
		: AbstractServer( &core )
	{
		if(!this->initialize(_T("tcp://0.0.0.0:6543")))
		{
			LOG_FATAL(log(), _T("³õÊ¼Ê§°Ü"));
			return;
		}
		acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, &core);
	}


	void OnComplete(ITransport* transport, IOCPServer* core)
	{
		transport->bindProtocol(&protocol_);
		transport->initialize();

		acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, core);
	}

	void OnError(const ErrorCode& err, IOCPServer* core)
	{
		acceptor_.accept(this, &EchoServer::OnComplete, &EchoServer::OnError, core);
	}
	virtual const tstring& toString() const
	{
		return protocol_.toString();
	}
private:

	EchoProtocol protocol_;
};

_jingxian_end

#endif //_EchoServer_H_