
#ifndef _TCPAcceptor_H_
#define _TCPAcceptor_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/TCPEndpoint.h"
# include "jingxian/networks/sockets/base_socket.h"
# include "jingxian/networks/proactor.h"

_jingxian_begin

class TCPAcceptor : public IAcceptor
{
public:
	
	TCPAcceptor(proactor* core, IProtocolFactory* protocolFactory, const tchar* endpoint);
	
	virtual ~TCPAcceptor();

	/**
	 * @implements timeout
	 */
    virtual time_t timeout() const;

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
	NOCOPY(TCPAcceptor);

	proactor* proactor_;
	IProtocolFactory* protocolFactory_;
	base_socket socket_;
	TCPEndpoint endpoint_;
	tstring toString_;
};


class TCPAcceptorFactory : public IAcceptorFactory
{
public:

	TCPAcceptorFactory(proactor* core);

	virtual ~TCPAcceptorFactory();

	/**
	* @implements createAcceptor
	 */
	virtual IAcceptor* createAcceptor(const tchar* endPoint, IProtocolFactory* protocolFactory);

	/**
	* @implements toString
	 */
	virtual const tstring& toString() const;
	
private:
	NOCOPY(TCPAcceptorFactory);
	
	proactor* proactor_;
	tstring toString_;	
};

_jingxian_end

#endif //_TCPAcceptor_H_ 