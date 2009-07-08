
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
	{
	}

	bool initialize(const tchar* addr)
	{
		acceptor_.reset(reactor_->listenWith(addr));
		return !acceptor_.isNull();
	}
	
	virtual ~AbstractServer(){}

	/**
	 * 取得地址的描述
	 */
	virtual const tstring& toString() const = 0;
protected:
	IReactorCore* reactor_;
	Acceptor acceptor_;
};

inline tostream& operator<<( tostream& target, const AbstractServer& server )
{
	target << server.toString();
	return target;
}

_jingxian_end

#endif //_AbstractServer_h_