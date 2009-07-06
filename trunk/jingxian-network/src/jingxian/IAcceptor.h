
#ifndef _IAcceptor_H_
#define _IAcceptor_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "buffer.h"
# include "jingxian/string/string.hpp"

_jingxian_begin

class IAcceptor
{
public:
	virtual ~IAcceptor(){}

	/**
	 * 取得超时时间
	 */
    virtual  time_t timeout () const = 0;

	/**
	 * 监听的地址
	 */
    virtual const tstring& bindPoint() const = 0;

	/**
	 * 是不是处理监听状态
	 */
	virtual bool isListening() const = 0;

	/**
	 * 发起一个监听请求
	 */
    virtual void accept(OnBuildConnectionSuccess buildProtocol
                            , OnBuildConnectionError onConnectError
                            , void* context) = 0;

	/**
	* 取得地址的描述
	*/
	virtual const tstring& toString() const = 0;
};


class IAcceptorFactory
{
public:

	virtual ~IAcceptorFactory(){}

	/**
	 * 创建 IAcceptor 对象
	 */
	virtual IAcceptor* createAcceptor(const tchar* endpoint) = 0;

	/**
	 * 取得地址的描述
	 */
	virtual const tstring& toString() const = 0;
};

inline tostream& operator<<( tostream& target, const IAcceptor& acceptor )
{
	target << acceptor.toString();
	return target;
}

inline tostream& operator<<( tostream& target, const IAcceptorFactory& acceptorFactory )
{
	target << acceptorFactory.toString();
	return target;
}

_jingxian_end

#endif //_IAcceptor_H_ 