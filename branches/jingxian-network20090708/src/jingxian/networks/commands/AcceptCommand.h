
#ifndef _AcceptCommand_H_
#define _AcceptCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/TCPAcceptor.h"

_jingxian_begin

class AcceptCommand : public ICommand
{
public:
	AcceptCommand(TCPAcceptor* acceptor
							, OnBuildConnectionComplete onComplete
                            , OnBuildConnectionError onError
                            , void* context);

	virtual ~AcceptCommand();
	
	virtual void on_complete(size_t bytes_transferred
		, bool success
		, void *completion_key
		, u_int32_t error);

	virtual bool execute();

protected:
	
    void initializeConnection(int bytesTransferred
							  , void *completion_key);

private:
	NOCOPY(AcceptCommand);

	IOCPServer* core_;
	OnBuildConnectionComplete onSuccess_;
    OnBuildConnectionError onError_;
    void* context_;

	SOCKET listener_;
	tstring listenAddr_;
	SOCKET socket_;
	char* ptr_;
	size_t len_;
};

_jingxian_end

#endif //_AcceptCommand_H_

