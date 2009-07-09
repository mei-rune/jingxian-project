
# include "pro_config.h"
# include "jingxian/networks/ConnectedSocket.h"
# include "jingxian/networks/commands/DisconnectCommand.h"

_jingxian_begin

ConnectedSocket::ConnectedSocket(IOCPServer* core
								 , SOCKET socket
								 , const tstring& host
								 , const tstring& peer)
: core_(core)
, socket_(socket)
, host_(host)
, peer_(peer)
, state_(connection_status::connected)
, timeout_(3*1000)
, protocol_(0)
, context_(core, this)
, isInitialize_(false)
, reading_(false)
, tracer_(0)
{
	tracer_ = logging::makeTracer( _T("ConnectedSocket[") + host_ + _T("-") + peer_ + _T("]"));
	
	if(null_ptr!=tracer_ && tracer_->isTraceEnabled())
	{
		StringStream oss;
		oss << _T("创建 ConnectedSocket 对象成功");
		tracer_->trace( logging::Both, oss, __FILE__, __LINE__);
	}
}

ConnectedSocket::~ConnectedSocket( )
{	
	if(null_ptr!=tracer_ && tracer_->isTraceEnabled())
	{
		StringStream oss;
		oss << _T("销毁 ConnectedSocket 对象成功");
		tracer_->trace( logging::Both, oss, __FILE__, __LINE__);
	}
}

void ConnectedSocket::bindProtocol(IProtocol* protocol)
{
	protocol_ = protocol;
}

void ConnectedSocket::initialize()
{
	if( isInitialize_ )
		return;

	protocol_->onConnected( context_ );
	startReading();
	isInitialize_ = true;
}

void ConnectedSocket::startReading()
{
	reading_ = true;
	doRead();
}

void ConnectedSocket::stopReading()
{
	reading_ = false;
}

void ConnectedSocket::doRead()
{
	size_t len =0;
	for(std::vector<databuffer_t*>::iterator it = readingBuffer_.begin()
		; it != readingBuffer_.end() ; ++ it)
	{
		len += GETFREELENGTH(*it);
	}

	if( len <= 0 )
		readingBuffer_.push_back(protocol_->createBuffer(context_
			, &(readingBuffer_[0])
			, readingBuffer_.size());

	
	
}

void ConnectedSocket::write(char* buffer, int length)
{
	ThrowException( NotImplementedException );
}

void ConnectedSocket::disconnection()
{
	disconnection(_T("用户主动关闭连接"));
}

void ConnectedSocket::disconnection(const tstring& error)
{
	std::auto_ptr< ICommand> command(new DisconnectCommand(this));
	if(!command->execute())
	{
		int code = ::WSAGetLastError();
		onDisconnected(code, concat<tstring>(_T("断开连接时发生错误 - "), lastError(code)));
		return ;
	}

	command.release();
}

const tstring& ConnectedSocket::host() const
{
	return host_;
}

const tstring& ConnectedSocket::peer() const
{
	return peer_;
}

time_t ConnectedSocket::timeout() const
{
	return timeout_;
}

const tstring& ConnectedSocket::toString() const
{
	return toString_;
}

void ConnectedSocket::onDisconnected(errcode_t error, const tstring& description)
{
	protocol_->onDisconnected(context_,error, description);
}



//protected void internalRead(ByteBuffer byteBuffer)
//{
//    ReadError error = null;
//    try
//    {
//        while (0 != _readBuffer.Count)
//        {
//            ByteBuffer firstBuffer = _readBuffer.First.Value;
//            _readBuffer.RemoveFirst();
//            _protocol.OnReceived(_context, firstBuffer);
//            if (!_reading)
//                return;
//        }

//        doRead(byteBuffer);
//        return;
//    }
//    catch (ReadError err)
//    {
//        error = err;
//    }
//    catch (Exception e)
//    {
//        error = new ReadError(e, e.Message);
//    }
//    OnException(error);
//}

//protected void doRead(ByteBuffer byteBuffer)
//{
//    _trace.Info(TransportWay.Receive ,"开始读数据");

//    ByteBuffer readBuffer = (null != _protocol) ?
//        (_protocol.CreateBuffer(byteBuffer) ?? new ByteBuffer(BaseProtocol.BUFFEREDSIZE)) : new ByteBuffer(BaseProtocol.BUFFEREDSIZE );

//    new ReadFileOp(this, readBuffer).Read();
//}

//protected void cbRead(ByteBuffer byteBuffer)
//{
//    _port.Send<ByteBuffer>(this.internalRead, byteBuffer);
//}

//public void StartReading()
//{
//    if (_state != ConnectionStatus.connected)
//    {
//        _trace.Info(TransportWay.Receive, "通道已经关闭!");
//        throw new RuntimeError("通道已经关闭!");
//    }

//    if (_reading)
//    {
//        _trace.Info(TransportWay.Receive, "通道已经在读数据!");
//        return;
//    }

//    if (null == _protocol)
//        throw new RuntimeError("没有初始化protocol对象!");

//    ReadError error = null;
//    try
//    {
//        startReading(null);
//        return;
//    }
//    catch (ReadError err)
//    {
//        error = err;
//    }
//    catch (Exception exception)
//    {
//        error = new ReadError(exception, exception.Message);
//    }

//    _port.Send<ReadError>(this.OnException, error);
//}

//public void startReading(ByteBuffer byteBuffer)
//{
//    _reading = true;
//    if (0 != _readBuffer.Count)
//    {
//        cbRead(byteBuffer);
//    }
//    else
//    {
//        doRead(byteBuffer);
//    }
//}

//public void StopReading()
//{

//    _trace.Info(TransportWay.Receive, "停止读数据!");
//    _reading = false;
//}

//internal void OnRead(ByteBuffer byteBuffer)
//{
//    if (_state != ConnectionStatus.connected)
//    {
//        _reading = false;
//        _trace.Info(TransportWay.Receive, "读数据完成，但通道已经被关闭!");
//        return;
//    }


//    _trace.Info(TransportWay.Receive, "读数据成功!");

//    _trace.WriteInboundBuffer(byteBuffer);

//    _triggeredTime = DateTime.Now;

//    if (_reading)
//    {
//        _protocol.OnReceived(_context, byteBuffer);

//        if (_state != ConnectionStatus.connected)
//        {
//            _trace.Info(TransportWay.Receive, "处理数据完成，通道被关闭，不再读数据!");
//            return;
//        }
//        startReading( byteBuffer );
//    }
//    else
//    {
//        _trace.Info(TransportWay.Receive, "读数据成功，但已经暂停读数据!");
//        _readBuffer.AddLast(byteBuffer);
//    }
//}

//internal void OnException(ReadError error)
//{
//    _reading = false;
//    if (_state != ConnectionStatus.connected)
//    {
//        _trace.Error(TransportWay.Receive, "读数据时发生错误时,发现已关闭!", error);
//        return;
//    }

//    _trace.Error(TransportWay.Receive, tstring.Format( "读数据时发生错误 - {0}!", error.Message), error );
//    close(error);
//}



//public void Write(byte[] data)
//{
//    if (null == data || 0 == data.Length)
//        return;

//    Write(new ByteBuffer(data, 0, data.Length));
//}


//public void Write(byte[] data, int offest, int length)
//{
//    if (null == data || 0 == data.Length)
//        return;

//    if (data.Length < (offest + length))
//        return;

//    Write(new ByteBuffer(data, offest, length));
//}

//public void Write(ByteBuffer[] byteBuffers)
//{
//    if (null == byteBuffers || 0 == byteBuffers.Length)
//        return;
//    foreach (ByteBuffer byteBuffer in byteBuffers)
//        Write(byteBuffer);
//}

//public void Write(ByteBuffer byteBuffer)
//{
//    if (null == byteBuffer || 0 == byteBuffer.Count)
//        return;

//    if (ConnectionStatus.connected != _state
//        && ConnectionStatus.disconnecting != _state)
//    {
//        _trace.Info(TransportWay.Send, "写数据时通道已经关闭!");
//        return;
//    }


//    if (null == _protocol)
//        throw new RuntimeError("没有初始化protocol对象!");

//    _writebuf.AddLast(byteBuffer);
//    _writeBufferedSize += byteBuffer.Count;

//    if (_writing)
//        return;

//    _trace.Info(TransportWay.Send, "开始写数据!");

//    WriteError error;
//    try
//    {
//        startWriting();
//        return;
//    }
//    catch (WriteError err)
//    {
//        error = err;
//    }
//    catch (Exception exception)
//    {
//        error = new WriteError(exception, exception.Message);
//    }

//    _port.Send<WriteError>(this.OnException, error);
//}

//protected void startWriting()
//{
//    _writing = true;
//    ByteBuffer byteBuffer = _writebuf.First.Value;
//    _trace.WriteOutboundBuffer(byteBuffer);

//    new WriteRequest(this, byteBuffer).Write();
//}

//protected void stopWriting()
//{
//    _trace.Info(TransportWay.Send, "停止写数据!");
//    _writing = false;
//}

//internal void OnWrite(int bytes_transferred, ByteBuffer byteBuffer)
//{
//    _writeBufferedSize -= bytes_transferred;
//    byteBuffer.Begin += bytes_transferred;

//    _triggeredTime = DateTime.Now;

//    if (0 < byteBuffer.Count)//没有传输完全部
//    {
//        startWriting();
//        return;
//    }

//    if (0 > byteBuffer.Count)
//    {
//        _writing = false;

//        _trace.Error(TransportWay.Send, "写数据时发现错误 - 一次传输的字节数不能大于总字节数!");
//        throw new WriteError("一次传输的字节数不能大于总字节数!");
//    }

//    _writebuf.RemoveFirst();

//    if (0 != _writebuf.Count)
//    {
//        startWriting();
//        return;
//    }
//    _writing = false;
//    _trace.Info(TransportWay.Send, "写数据完成 - buffer已经为空!");

//    if (_isDisconnectingOnEmpty)
//    {
//        _isDisconnectingOnEmpty = false;
//        _isShutdownOnEmpty = false;
//        doClose(_disconnectingArg);
//    }
//    else if (_isShutdownOnEmpty)
//    {
//        _isShutdownOnEmpty = false;
//        _socket.Shutdown(SocketShutdown.Send);
//    }
//}

//internal void OnException(WriteError error)
//{
//    _writing = false;
//    if (_state != ConnectionStatus.connected
//        && _state != ConnectionStatus.disconnecting)
//    {
//        _trace.Error(TransportWay.Send, "写数据时发生错误,但通道已关闭!", error);
//        return;
//    }

//    _trace.Error(TransportWay.Send,  tstring.Format( "写数据时发生错误,关闭通道 - {0}!", error.Message), error );
//    doClose(error);
//}



//protected void cbDisconnection(Exception e)
//{
//    if (_isDisconnectingOnEmpty)
//    {
//        _trace.Info(TransportWay.Receive, "尝试写数据完成时关闭通道 - 已经在等待了!");
//        return;
//    }

//    _trace.Info(TransportWay.Receive, "尝试写数据完成时关闭通道!");

//    _isDisconnectingOnEmpty = true;
//    _state = ConnectionStatus.disconnecting;
//    _disconnectingArg = e;
//}

//public void OnDisconnection(Exception error)
//{
//    doClose(error);
//}

//protected void doClose(Exception exception)
//{
//    _trace.Info(TransportWay.Both, "开始关闭通道!");

//    _state = ConnectionStatus.disconnected;

//    if (null == _socket)
//        return;

//    IProtocol protocol = _protocol;
//    _protocol = null;

//    try
//    {
//        if (null != protocol)
//            _port.Disconnection(this, protocol, exception);
//    }
//    catch( Exception e)
//    {
//        _trace.Info(TransportWay.Both, "开始关闭通道时, 触发系统断开事件发生错误!", e );
//    }
//    if (_read_shutdown)
//    {
//        if (!_write_shutdown)
//            _socket.Shutdown(SocketShutdown.Send);
//    }
//    else if (_write_shutdown)
//    {
//        _socket.Shutdown(SocketShutdown.Send);
//    }
//    else
//    {
//        _socket.Shutdown(SocketShutdown.Receive);
//    }
//    _read_shutdown = true;
//    _write_shutdown = true;

//    _socket.Close();
//    _socket = null;
//    try
//    {
//        if (null != protocol)
//            protocol.OnDisconnection(_context, exception);
//    }
//    catch (Exception e)
//    {
//        _trace.Info(TransportWay.Both, "开始关闭通道时, 触发断开事件发生错误!", e);
//    }

//    _trace.Info(TransportWay.Both, "关闭通道完成!");
//}

//public void close(Exception exception)
//{
//    _state = ConnectionStatus.disconnecting;
//    if (_writing)
//    {
//        cbDisconnection(exception);
//    }
//    else
//    {
//        _trace.Info(TransportWay.Both, "尝试关闭通道!");
//        doClose(exception);
//    }
//}

//public void Disconnection()
//{
//    Disconnection(new ConnectionDone("关闭通道!"));
//}
//public void Disconnection(Exception error)
//{
//    if (_state != ConnectionStatus.connected)
//    {
//        _trace.Info(TransportWay.Both, "尝试关闭通道时发现已经关闭通道了!");
//        return;
//    }

//    StopReading();

//    _state = ConnectionStatus.disconnecting;

//    if (_writing)
//    {
//        cbDisconnection(error);
//    }
//    else
//    {
//        _trace.Info(TransportWay.Both, "尝试关闭通道!");

//        _port.Send<Exception>(this.doClose, error);
//    }
//}

//protected void cbShutdown(SocketShutdown arg)
//{
//    if (_isDisconnectingOnEmpty)
//    {
//        _trace.Info(TransportWay.Both,"尝试写数据完成时Shutdown端口错误! - 已经在等待了!");
//        return;
//    }

//    _trace.Info(TransportWay.Both, "尝试写数据完成时Shutdown端口错误!");

//    _isShutdownOnEmpty = true;
//    _shutdownArg = arg;
//}

//public void Shutdown()
//{
//    Shutdown(true, true);
//}

//public void Shutdown(bool write, bool read)
//{
//    if (ConnectionStatus.connected != _state)
//        return;

//    if (read && !_read_shutdown)
//    {
//        _read_shutdown = true;

//        _trace.Info(TransportWay.Both, "Shutdown端口收功能!");
//        _socket.Shutdown(SocketShutdown.Receive);
//    }

//    if (write && !_write_shutdown)
//    {
//        _write_shutdown = true; // don't need to keep "we are shutting write side down", right?
//        if (_writing)
//        {
//            cbShutdown(SocketShutdown.Send);
//            //_isShutdownOnEmpty = true;
//        }
//        else
//        {
//            _trace.Info(TransportWay.Both, "Shutdown端口发功能!");
//            _socket.Shutdown(SocketShutdown.Send);
//        }
//    }
//}



//public IEndpoint Host
//{
//    get
//    {
//        //if (null != _socket)
//        //    return _socket.LocalEndPoint;

//        return _host;
//    }
//}

//public IEndpoint Peer
//{
//    get
//    {
//        //if (null != _socket)
//        //    return _socket.RemoteEndPoint;
//        return _peer;
//    }
//}

//public void BindProtocol( IProtocol protocol)
//{
//    if (null == protocol)
//        throw new ArgumentNullException("protocol");
//    //if (null == context)
//    //    throw new ArgumentNullException("context");

//    //_context = context;

//    //if (null == _context.Trace)
//    //    _context.Trace = _trace;
//    //_context.Transport = this;
//    //if (null == _context.Reactor)
//    //    _context.Reactor = _port;

//    _protocol = protocol;
//}


//public bool NoDelay
//{
//    get { return _socket.NoDelay; }
//    set { _socket.NoDelay = value; }
//}

//public bool KeepAlive
//{
//    get { return _socket.KeepAlive; }
//    set { _socket.KeepAlive = value; }
//}

//public int SendBufferSize
//{
//    get { return _socket.SendBufferSize; }
//    set { _socket.SendBufferSize = value; }
//}

//public int ReceiveBufferSize
//{
//    get { return _socket.ReceiveBufferSize; }
//    set { _socket.ReceiveBufferSize = value; }
//}

//public IPEndPoint LocalEndPoint
//{
//    get { return _socket.LocalEndPoint; }
//}

//public IPEndPoint RemoteEndPoint
//{
//    get { return _socket.RemoteEndPoint; }
//}

//public void OnTimeout()
//{
//    if (ConnectionStatus.connecting != _state)
//        return;

//    if (null != _protocol)
//        _protocol.OnTimeout(_context);
//}

//public TimeSpan Timeout
//{
//    get { return _timeout; }
//}

//public DateTime TriggeredTime
//{
//    get { return _triggeredTime; }
//    set { _triggeredTime = value; }
//}

_jingxian_end