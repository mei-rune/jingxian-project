
#ifndef _IAcceptor_H_
#define _IAcceptor_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "buffer.h"
# include "jingxian/string/string.hpp"
# include "Dictionary.h"

_jingxian_begin

class ErrorCode
{
public:
	ErrorCode( const tchar* err);
	ErrorCode( int success, int code);

	virtual ~ErrorCode();

	bool isSuccess() const;
	int getCode() const;
	const tstring& toSting() const;
private:
	bool isSuccess_;
	int code_;
	tstring err_;
};

typedef IProtocol* (*BuildProtocol)( ITransport* transport, void* context);
typedef void (*OnConnectError)( const ErrorCode& exception,  void* context);

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
	 * ֹͣ����
	 */
    virtual void accept( ) = 0;

	/**
	* ȡ�õ�ַ������
	*/
	virtual const tstring& toString() const = 0;
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