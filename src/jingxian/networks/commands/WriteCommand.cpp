
# include "pro_config.h"
# include "jingxian/networks/commands/WriteCommand.h"

_jingxian_begin
WriteCommand::WriteCommand(ConnectedSocket* transport
		, const iovec* iovec
		, size_t len)
: transport_(transport)
, iovec_(iovec)
, len_(len)
{
}

WriteCommand::~WriteCommand()
{
}

void WriteCommand::on_complete(size_t bytes_transferred,
							   int success,
							   void *completion_key,
							   u_int32_t error)
{
	//WriteError exception = null;
	//try
	//{
	//	if (!success)
	//	{
	//		exception = new WriteError(error);
	//	}
	//	else if (0 == bytes_transferred)
	//	{
	//		exception = new WriteError(new SocketException((int)SocketError.Shutdown), "¶Á0¸ö×Ö½Ú!");
	//	}
	//	else
	//	{
	//		_transport.OnWrite(bytes_transferred, _byteBuffer);
	//		return;
	//	}
	//}
	//catch (WriteError err)
	//{
	//	exception = err;
	//}
	//catch (Exception e)
	//{
	//	exception = new WriteError(e, e.Message);
	//}
	//_transport.OnException(exception);
	ThrowException( NotImplementedException );
}

bool WriteCommand::execute()
{
	//if (_transport.Socket.Write(bytePointer
	//	, _byteBuffer.Count
	//	, out bytesTransferred
	//	, NativeOverlapped))
	//	return;

	//int errCode = Marshal.GetLastWin32Error();
	//if ((int)SocketError.IOPending == errCode)
	//	return;

	//throw new WriteError(errCode);
	ThrowException( NotImplementedException );
}

_jingxian_end
