
#ifndef _TCPAcceptor_H_
#define _TCPAcceptor_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/IReactorCore.h"

_jingxian_begin

class TCPAcceptor
{
public:
	
	TCPAcceptor();
	
	virtual ~TCPAcceptor()

	/**
	 * @implements timeout
	 */
    virtual  time_t timeout () const;

	/**
	 * @implements bindPoint
	 */
    virtual const IEndpoint& bindPoint() const;

	/**
	 * @implements stopListening
	 */
    virtual void stopListening();

	/**
	 * @implements startListening
	 */
    virtual bool startListening();

	/**
	 * @implements protocolFactory
	 */
    virtual IProtocolFactory& protocolFactory();

	/**
	 * @implements misc
	 */
    virtual IDictionary& misc();

	/**
	 * @implements misc
	 */
	virtual const IDictionary& misc() const;

	/**
	* @implements toString
	*/
	virtual const tstring& toString() const;
	
private:
	NO_COPY(TCPAcceptor );
	
	TCPEndpoint _endpoint;
	
	tstring _toString;
};


class TCPAcceptorFactory
{
public:

	virtual ~IAcceptorFactory(){}

	/**
	* @implements createAcceptor
	 */
	virtual IAcceptor* createAcceptor(const tchar* endPoint);

	/**
	* @implements toString
	 */
	virtual const tstring& toString() const;
	
private:
	NO_COPY(TCPAcceptor );
	
	TCPEndpoint _endpoint;
	
	tstring _toString;	
};

_jingxian_end

#endif //_TCPAcceptor_H_ 