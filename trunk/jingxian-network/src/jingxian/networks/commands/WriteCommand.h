
#ifndef _WriteCommand_H_
#define _WriteCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/ConnectedSocket.h"

_jingxian_begin

class WriteCommand : public ICommand
{
public:
	WriteCommand(ConnectedSocket* transport
		, const iovec* iovec
		, size_t len);

	virtual ~WriteCommand();
	
	virtual void on_complete(size_t bytes_transferred
		, bool success
		, void *completion_key
		, errcode_t error);

	virtual bool execute();

private:
	NOCOPY(WriteCommand);

	ConnectedSocket* transport_;
	const iovec* iovec_;
	size_t len_;
};

_jingxian_end

#endif //_WriteCommand_H_
