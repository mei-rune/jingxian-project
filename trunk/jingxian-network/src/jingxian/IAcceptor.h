
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
	 * ȡ�ó�ʱʱ��
	 */
    virtual  time_t timeout () const = 0;

	/**
	 * �����ĵ�ַ
	 */
    virtual const tstring& bindPoint() const = 0;

	/**
	 * �ǲ��Ǵ������״̬
	 */
	virtual bool isListening() const = 0;

	/**
	 * ����һ����������
	 */
    virtual void accept(OnBuildConnectionSuccess buildProtocol
                            , OnBuildConnectionError onConnectError
                            , void* context) = 0;

	/**
	 * �رս�����
	 */
	virtual void close() = 0;

	/**
	* ȡ�õ�ַ������
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
	 * �����ĵ�ַ
	 */
    virtual const tstring& bindPoint() const
	{
		return acceptor_->bindPoint();
	}

	/**
	 * �ǲ��Ǵ������״̬
	 */
	virtual bool isListening() const
	{
		return acceptor_->isListening();
	}

	/**
	 * ����һ����������
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
	* ȡ�õ�ַ������
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
	 * ���� IAcceptor ����
	 */
	virtual IAcceptor* createAcceptor(const tchar* endpoint) = 0;

	/**
	 * ȡ�õ�ַ������
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