
#ifndef _AbstractServer_h_
#define _AbstractServer_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "IReactorCore.h"

_jingxian_begin

class AbstractServer
{
public:

	AbstractServer(IReactorCore* reactor)
		:reactor_(reactor)
		, logger_(null_ptr)
	{
	}

	bool initialize(const tchar* addr)
	{
		acceptor_.reset(reactor_->listenWith(addr));
		return !acceptor_.isNull();
	}

	bool initialize(const tstring& addr)
	{
		return initialize(addr.c_str());
	}
	
	virtual ~AbstractServer()
	{
		if(null_ptr != logger_)
		{
			delete logger_;
			logger_ = null_ptr;
		}

		acceptor_.close();
	}

	
	ILogger* log()
	{
		if(is_null(logger_))
			logger_ = logging::makeLogger(toString());

		return logger_;
	}

	/**
	 * ȡ�õ�ַ������
	 */
	virtual const tstring& toString() const
	{
		return _toString;
	}
protected:
	NOCOPY(AbstractServer);
	tstring _toString;
	IReactorCore* reactor_;
	Acceptor acceptor_;
	ILogger* logger_;
};

inline tostream& operator<<( tostream& target, const AbstractServer& server )
{
	target << server.toString();
	return target;
}

_jingxian_end

#endif //_AbstractServer_h_