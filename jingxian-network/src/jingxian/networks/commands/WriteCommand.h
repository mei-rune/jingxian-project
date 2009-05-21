
#ifndef _WriteCommand_H_
#define _WriteCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"

_jingxian_begin

class WriteCommand : public ICommand
{

public:
	write_request()
	{
	}

	void init(  )
	{
	}



	void on_complete(size_t bytes_transferred,
		int success,
		const void *completion_key,
		u_long error = 0)
	{
		WriteError exception = null;
		try
		{
			if (!success)
			{
				exception = new WriteError(error);
			}
			else if (0 == bytes_transferred)
			{
				exception = new WriteError(new SocketException((int)SocketError.Shutdown), "¶Á0¸ö×Ö½Ú!");
			}
			else
			{
				_transport.OnWrite(bytes_transferred, _byteBuffer);
				return;
			}
		}
		catch (WriteError err)
		{
			exception = err;
		}
		catch (Exception e)
		{
			exception = new WriteError(e, e.Message);
		}
		_transport.OnException(exception);
	}

	void post()
	{
		if (_transport.Socket.Write(bytePointer
			, _byteBuffer.Count
			, out bytesTransferred
			, NativeOverlapped))
			return;

		int errCode = Marshal.GetLastWin32Error();
		if ((int)SocketError.IOPending == errCode)
			return;

		throw new WriteError(errCode);
	}
private:
	connected_socket* _transport;
	std::vector<iovec> _iovec;
};

_jingxian_end

#endif //_WriteCommand_H_
