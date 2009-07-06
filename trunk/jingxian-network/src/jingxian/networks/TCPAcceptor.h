
#ifndef _TCPAcceptor_H_
#define _TCPAcceptor_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/sockets/BaseSocket.h"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/networks/TCPFactory.h"


_jingxian_begin

class TCPAcceptor : public IAcceptor
{
public:
	
	TCPAcceptor(IOCPServer* core, const tchar* endpoint);
	
	virtual ~TCPAcceptor();

	/**
	 * @implements timeout
	 */
    virtual time_t timeout() const;

	/**
	 * @implements bindPoint
	 */
    virtual const tstring& bindPoint() const;

	/**
	 * @implements isListening
	 */
	virtual bool isListening() const;

	/**
	 * @implements stopListening
	 */
    virtual void stopListening();

	/**
	 * @implements startListening
	 */
    virtual bool startListening();

	/**
	* @implements toString
	*/
	virtual const tstring& toString() const;

private:
	NOCOPY(TCPAcceptor);

	friend class AcceptCommand;

	TCPFactory& tcpFactory(){ return server_->tcpFactory(); }
	SOCKET handle() { return socket_.handle(); }
	IOCPServer* nextCore(){ return server_; }
	ILogger* logger(){ return logger_; }

	void onException( int error, const tstring& description);
	bool doAccept();

	IOCPServer* server_;
	BaseSocket socket_;
	tstring endpoint_;
	connection_status::type status_;
	ILogger* logger_;
	tstring toString_;
};


class TCPAcceptorFactory : public IAcceptorFactory
{
public:

	TCPAcceptorFactory(IOCPServer* core);

	/**
	* @implements ~TCPAcceptorFactory
	 */
	virtual ~TCPAcceptorFactory();

	/**
	* @implements createAcceptor
	 */
	virtual IAcceptor* createAcceptor(const tchar* endPoint);

	/**
	* @implements toString
	 */
	virtual const tstring& toString() const;
	
private:
	NOCOPY(TCPAcceptorFactory);
	
	IOCPServer* server_;
	tstring toString_;	
};

_jingxian_end

#endif //_TCPAcceptor_H_ 