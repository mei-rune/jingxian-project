
# include "pro_config.h"
# include "jingxian/networks/commands/WriteCommand.h"

_jingxian_begin

void WriteCommand::on_complete(size_t bytes_transferred,
							   int success,
							   void *completion_key,
							   u_int32_t error)
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
			exception = new WriteError(new SocketException((int)SocketError.Shutdown), "��0���ֽ�!");
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

void WriteCommand::execute()
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

_jingxian_end
