
# include "pro_config.h"
# include "jingxian/networks/commands/DisconnectCommand.h"


_jingxian_begin


ConnectCommand:: ConnectCommand(ILog logger
								, Connector connector
								, BuildProtocol<T> buildProtocol
								, OnConnectError<T> throwError
								, T context
								, ByteBuffer byteBuffer)
								: base(null)
{
	_logger = logger;
	_socket = null;
	_connector = connector;
	_buildProtocol = buildProtocol;
	_throwError = throwError;
	_context = context;
	_byteBuffer = byteBuffer;
	if (null != _byteBuffer)
		PinObject(_byteBuffer.Array);
}

void ConnectCommand::Connect()
{

	_core = _connector.Core.GetNextCore();
	if (null == _socket)
	{
		_socket = _core.CreateSocket(_connector.AddressFamily
			, _connector.SocketType, _connector.ProtocolType);
	}
	if (!_core.Bind(_socket))
	{
		throw new SocketException();
	}

	int bytesTransferred;
	if (null != _byteBuffer && 0 < _byteBuffer.Count)
	{
		IntPtr bytePointer = Marshal.UnsafeAddrOfPinnedArrayElement(
			_byteBuffer.Array, _byteBuffer.Begin);
		if (_socket.ConnectEx(_connector.ConnectTo, bytePointer, _byteBuffer.Count, out bytesTransferred, this.NativeOverlapped, _core))
			return;
	}
	else
	{
		if (_socket.ConnectEx(_connector.ConnectTo, IntPtr.Zero, 0, out bytesTransferred, this.NativeOverlapped, _core))
			return;
	}
	if ((int)SocketError.IOPending == Marshal.GetLastWin32Error())
		return;

	throw new SocketException();
}

void ConnectCommand::Complete(int bytes_transferred, bool success, int error, object context)
{
	if (!success)
	{
		try
		{
			_core.ReleaseSocket(_socket, false );
			_socket = null;
		}
		catch { }
		_connector.OnError(_throwError, _context, new ConnectError(_connector.ConnectTo, new SocketException(error)));
		return;
	}

	Exception exception = null;
	try
	{
		if (null != _byteBuffer && 0 < bytes_transferred)
			_byteBuffer.Begin += bytes_transferred;


		_logger.InfoFormat("连接到[{0}]成功，开始初始化!", _connector.ConnectTo);
		_socket.SetRemote(_connector.ConnectTo, null);
		_socket.SetSockOpt(SocketOptionLevel.Socket
			, SocketOptionName.UpdateConnectContext
			, _socket.Handle);


		ConnectedSocket connectedSocket = new ConnectedSocket(_core, _socket, new ProtocolContext(null, _connector.Config));
		_socket = null;

		try
		{
			IProtocol protocol = _buildProtocol(connectedSocket, _context);
			_core.InitializeConnection(connectedSocket, protocol);

			//FIXME: 将在连接时没有发送完的数据，再发送（不能放在）
			///if (null != _byteBuffer && 0 < _byteBuffer.Count)
			///    connectedSocket.Write(_byteBuffer);

			_logger.InfoFormat("连接到[{0}]成功，初始化成功!", _connector.ConnectTo);
			return;
		}
		catch (Exception)
		{
			_socket = connectedSocket.ReleaseSocket();
		}
	}
	catch (Exception e)
	{
		exception = new InitializeError(_connector.ConnectTo,
			string.Format("初始化连接到[{0}]通道，发生错误！", _connector.ConnectTo), e);
	}
	try
	{
		_core.ReleaseSocket(_socket, false);
		_socket = null;
	}
	catch { }

	_connector.OnError(_throwError, _context,exception);

}

public override void ConnectCommand::internalDispose()
{
	base.internalDispose();

	if (null == _socket)
		return;

	try
	{
		_core.ReleaseSocket(_socket, false);
	}
	catch
	{ }
	_socket = null;
}


_jingxian_end
