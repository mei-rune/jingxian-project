
# include "pro_config.h"
# include "jingxian/networks/commands/ReadCommand.h"


_jingxian_begin

ReadCommand::ReadCommand(ConnectedSocket* transport
						, char* ptr
						, size_t len )
: transport_(transport)
, ptr_(ptr)
, len_(len)
{
}

ReadCommand::~ReadCommand( )
{
}

void ReadCommand::on_complete(size_t bytes_transferred
		, bool success
		, void *completion_key
		, errcode_t error)
{
	//ReadError exception = null;
	//try
	//{
	//	if (!success)
	//	{
	//		exception = new ReadError(error);
	//	}
	//	else if (0 == bytes_transferred)
	//	{
	//		exception = new ReadError(new SocketException((int)SocketError.Shutdown), "读0个字节!");
	//	}
	//	else
	//	{
	//		_byteBuffer.End += bytes_transferred;
	//		_transport.OnRead(_byteBuffer);
	//		return;
	//	}
	//}
	//catch (ReadError err)
	//{
	//	exception = err;
	//}
	//catch (Exception e)
	//{
	//	exception = new ReadError(e, e.Message);
	//}

	//_transport.OnException(exception);
	ThrowException( NotImplementedException );
}

bool ReadCommand::execute()
{
	//if (0 == _byteBuffer.Space)
	//	throw new ArgumentException( "缓冲区为空!" );

	//IntPtr bytePointer = Marshal.UnsafeAddrOfPinnedArrayElement(
	//	_byteBuffer.Array, _byteBuffer.End);

	//int bytesTransferred = 0;
	//if (_transport.Socket.Read(bytePointer
	//	, _byteBuffer.Space
	//	, out bytesTransferred
	//	, NativeOverlapped))
	//{
	//	return;
	//}
	//int errCode = Marshal.GetLastWin32Error();
	//if ((int)SocketError.IOPending == errCode)
	//{
	//	return;
	//}

	//throw new ReadError(errCode);
	ThrowException( NotImplementedException );
}

_jingxian_end
