
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
	AcceptCommand(TCPAcceptor* acceptor);

	virtual ~AcceptCommand();
	
	virtual void on_complete(size_t bytes_transferred
		, int success
		, void *completion_key
		, u_int32_t error);

	virtual void execute();

private:
	NOCOPY(AcceptCommand);

	TCPAcceptor* acceptor;
	SOCKET socket_;
	char* ptr_;
	size_t len_;
}

_jingxian_end

#endif //_AcceptCommand_H_

