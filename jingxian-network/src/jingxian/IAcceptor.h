
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
	 * 关闭接受器
	 */
	virtual void close() = 0;

	/**
	* 取得地址的描述
	*/
	virtual const tstring& toString() const = 0;
};

class Acceptor : public IAcceptor
{
public:
	template<typename F1,typename F2, typename T>
	class closure
	{
	public:

		closure( const F1& func1, const F2& func2, T* context)
			: function1_(func1)
			, function2_(func2)
			, context_(context)
		{
		}

		static void OnSuccess(ITransport* transport, void* context)
		{
			std::auto_ptr<closure> self(static_cast<closure*>(context));
			self->_function1(transport, context_);
		}

		static void OnError(const ErrorCode& exception, void* context)
		{
			std::auto_ptr<closure> self(static_cast<closure*>(context));
			self->_function1(transport, context_);
		}

	private:
		F1 function1_;
		F2 function2_;
		T* context_;
	};

	Acceptor()
		: acceptor_(null_ptr)
	{
	}

	Acceptor(IAcceptor* acceptor)
		: acceptor_(acceptor)
	{
	}

	virtual ~Acceptor()
	{
		reset(null_ptr);
	}

	void reset( IAcceptor* acceptor)
	{
		if(!is_null(acceptor_))
			acceptor_->close();
		acceptor_=acceptor;
	}

    virtual  time_t timeout () const
	{
		return acceptor_->timeout();
	}

	/**
	 * 监听的地址
	 */
    virtual const tstring& bindPoint() const
	{
		return acceptor_->bindPoint();
	}

	/**
	 * 是不是处理监听状态
	 */
	virtual bool isListening() const
	{
		return acceptor_->isListening();
	}

	/**
	 * 发起一个监听请求
	 */
	template< typename F1,typename F2, typename T> 
    void accept(F1 onSuccess
                            , F2 onError
                            , T* context)
	{
		typedef closure<F1,F2,T> closure_type;
		acceptor_->accept(closure_type::OnSuccess
			, closure_type::OnSuccess
			, new closure_type(onSuccess, onError, context));
	}

	/**
	* 取得地址的描述
	*/
	virtual const tstring& toString() const
	{
		return acceptor_->toString();
	}
private:
	NOCOPY(Acceptor);

	IAcceptor* acceptor_;
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