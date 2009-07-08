
#ifndef _ReadCommand_H_
#define _ReadCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/ConnectedSocket.h"

_jingxian_begin

class ReadCommand : public ICommand
{
public:

	ReadCommand(ConnectedSocket* transport
		, char* ptr
		, size_t len);
	
	virtual ~ReadCommand();

	virtual void on_complete(size_t bytes_transferred
		, bool success
		, void *completion_key
		, errcode_t error);

	virtual bool execute();

private:
	NOCOPY(ReadCommand);

	ConnectedSocket* transport_;
	char* ptr_;
	size_t len_;
};

_jingxian_end

#endif //_ReadCommand_H_
