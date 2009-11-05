
# include "pro_config.h"
# include "jingxian/directory.h"
# include "jingxian/protocol/NullProtocol.h"
# include "jingxian/networks/ProcessPipe.h"
# include "jingxian/networks/commands/DisconnectCommand.h"
# include "jingxian/networks/commands/ReadCommand.h"
# include "jingxian/networks/commands/WriteCommand.h"

_jingxian_begin

//ProcessPipe::ProcessPipe(IOCPServer* core
//                                 , SOCKET sock
//                                 , const tstring& host
//                                 , const tstring& peer)
//        : core_(core)
//        , socket_(sock)
//        , host_(host)
//        , peer_(peer)
//        , state_(connection_status::connected)
//        , timeout_(3*1000)
//        , protocol_(null_ptr)
//        , isInitialize_(false)
//        , stopReading_(false)
//        , reading_(false)
//        , writing_(false)
//        , shutdowning_(false)
//        , isPosition_(false)
//        , tracer_(0)
//{
//    toString_ = concat<tstring>(_T("ProcessPipe[")
//                                , host_
//                                , _T(" - ")
//                                , peer_
//                                , _T(" - ")
//                                , ::toString(sock)
//                                , _T("]"));
//
//    tracer_ = logging::makeTracer(_T("jingxian.connection.tcpConnection")
//                                  , host_
//                                  , peer_
//                                  , ::toString(sock));
//    TP_CRITICAL(tracer_, transport_mode::Both
//                , _T("创建 ProcessPipe 对象成功"));
//
//    context_.initialize(core, this);
//    incoming_.initialize(this);
//    outgoing_.initialize(this);
//}
//
//ProcessPipe::~ProcessPipe( )
//{
//    if ( INVALID_SOCKET != socket_)
//    {
//        ::closesocket(socket_);
//        socket_ = INVALID_SOCKET;
//    }
//
//    if (isPosition_)
//    {
//        core_->removeSession(sessionPosition_);
//        isPosition_ = false;
//    }
//
//    TP_CRITICAL(tracer_, transport_mode::Both
//                , _T("销毁 ProcessPipe 对象成功"));
//    delete tracer_;
//    tracer_ = null_ptr;
//}
//
//IProtocol* ProcessPipe::bindProtocol(IProtocol* protocol)
//{
//    IProtocol* old = protocol_;
//    protocol_ = protocol;
//    return old;
//}
//
//void ProcessPipe::initialize()
//{
//    if ( isInitialize_ )
//        return;
//
//#ifdef DUMPFILE
//    tstring logPath = combinePath(core_->basePath(), _T("log"));
//    if (!existDirectory(logPath))
//        createDirectory(logPath);
//
//    os.reset( new std::ofstream(toNarrowString(simplify (combinePath(logPath,concat<tstring>(_T("raw_out_"), ::toString(socket_), _T(".txt"))))).c_str()));
//
//
//    is.reset( new std::ofstream(toNarrowString(simplify (combinePath(logPath,concat<tstring>(_T("raw_in_"), ::toString(socket_), _T(".txt"))))).c_str()));
//
//#endif
//
//    if (null_ptr == protocol_)
//    {
//        static NullProtocol nullProtocol(true);
//        protocol_ = &nullProtocol;
//    }
//
//    protocol_->onConnected( context_ );
//    isInitialize_ = true;
//    startReading();
//
//    if (!isPosition_)
//    {
//        sessionPosition_ = core_->addSession( this );
//        isPosition_ = true;
//    }
//}
//
//void ProcessPipe::startReading()
//{
//    if ( connection_status::connected != state_)
//    {
//        TP_TRACE(tracer_, transport_mode::Receive
//                 , _T("尝试读数据时连接已断开"));
//        return;
//    }
//
//    TP_TRACE(tracer_, transport_mode::Receive
//             , _T("启动读线程!"));
//    stopReading_ = false;
//    doRead();
//}
//
//void ProcessPipe::stopReading()
//{
//    stopReading_ = true;
//}
//
//void ProcessPipe::write(buffer_chain_t* buffer)
//{
//    if (is_null(buffer))
//        ThrowException1(ArgumentNullException, _T("buffer"));
//
//    outgoing_.send(buffer);
//    doWrite();
//}
//
//
//void ProcessPipe::writeBatch(buffer_chain_t** buffers, size_t len)
//{
//    if (is_null(buffers))
//        ThrowException1(ArgumentNullException, _T("buffers"));
//
//    for (size_t i = 0; i < len; ++i)
//    {
//        outgoing_.send(buffers[i]);
//    }
//
//    if (0 != len)
//        doWrite();
//}
//
//void ProcessPipe::disconnection()
//{
//    disconnection(_T("用户主动关闭连接"));
//}
//
//void ProcessPipe::disconnection(const tstring& error)
//{
//    doDisconnect(transport_mode::Both, 0, error);
//}
//
//void ProcessPipe::doRead()
//{
//    if (reading_)
//    {
//        TP_TRACE(tracer_, transport_mode::Receive
//                 , _T("尝试读数据时发现正在读取中"));
//        return;
//    }
//
//    if ( stopReading_)
//    {
//        TP_CRITICAL(tracer_, transport_mode::Receive
//                    , _T("尝试读数据时发现用户主动停止读数据"));
//        return;
//    }
//
//    if (shutdowning_)
//    {
//        tstring err = concat<tstring>(_T("尝试读数据时连接已断开 - ")
//                                      , disconnectReason_);
//        TP_CRITICAL(tracer_, transport_mode::Receive, err);
//        doDisconnect(transport_mode::Receive, 0, err);
//        return;
//    }
//
//    std::auto_ptr<ICommand> command(incoming_.makeCommand());
//    if (is_null(command))
//    {
//        tstring err = _T("尝试读数据时创建读请求失败");
//        TP_CRITICAL(tracer_, transport_mode::Receive, err);
//        doDisconnect(transport_mode::Receive, 0, err);
//        return;
//    }
//
//    if (!command->execute())
//    {
//        DWORD errCode = ::WSAGetLastError();
//        tstring err = ::concat<tstring>(_T("尝试读数据时发送读请求失败 - ")
//                                        ,lastError(errCode));
//        TP_CRITICAL(tracer_, transport_mode::Receive, err);
//        doDisconnect(transport_mode::Receive, errCode, err);
//        return;
//    }
//
//    TP_TRACE(tracer_, transport_mode::Receive, _T("发送读请求 - ")
//             << ((int)command.get()));
//    reading_ = true;
//    command.release();
//}
//
//void ProcessPipe::doWrite()
//{
//    if (writing_)
//    {
//        TP_TRACE(tracer_, transport_mode::Send
//                 , _T("尝试写数据时发现正在发送中"));
//        return;
//    }
//
//    if (shutdowning_)
//    {
//        tstring err = concat<tstring>(_T("尝试写数据时连接已断开 - ")
//                                      , disconnectReason_);
//        TP_CRITICAL(tracer_, transport_mode::Send, err);
//        doDisconnect(transport_mode::Send, 0, err);
//        return;
//    }
//
//    std::auto_ptr<ICommand> command(outgoing_.makeCommand());
//    if (is_null(command))
//    {
//        tstring err = _T("数据发送完毕! ");
//        TP_TRACE(tracer_, transport_mode::Send, err);
//        return;
//    }
//
//    if (!command->execute())
//    {
//        DWORD errCode = ::WSAGetLastError();
//        tstring err = ::concat<tstring>(_T("尝试写数据时发送写请求失败 - ")
//                                        ,lastError(errCode));
//        TP_CRITICAL(tracer_, transport_mode::Receive, err);
//        doDisconnect(transport_mode::Send, errCode, err);
//        return;
//    }
//
//    TP_TRACE(tracer_, transport_mode::Send, _T("发送写数据请求 - ")
//             << ((int)command.get()));
//    writing_ = true;
//    command.release();
//}
//
//void ProcessPipe::doDisconnect(transport_mode::type mode
//                                   , errcode_t error
//                                   , const tstring& description)
//{
//    if ( connection_status::connected != state_)
//    {
//        TP_TRACE(tracer_, mode, _T("尝试断开时发现已发出断开请求"));
//        return;
//    }
//
//    if (writing_)
//    {
//        assert( transport_mode::Send != mode);
//        shutdowning_ = true;
//        disconnectReason_ = description;
//
//        if ( INVALID_SOCKET != socket_ )
//            ::shutdown(socket_,  SD_BOTH);
//
//        TP_TRACE(tracer_, mode,_T("准备断开连接时发现写请未返回"));
//        return;
//    }
//
//    if (reading_)
//    {
//        assert( transport_mode::Receive != mode);
//        shutdowning_ = true;
//        disconnectReason_ = description;
//
//        if ( INVALID_SOCKET != socket_ )
//            ::shutdown(socket_,  SD_BOTH);
//
//        TP_TRACE(tracer_, mode, _T("准备断开连接时发现读请未返回"));
//        return;
//    }
//
//    std::auto_ptr<ICommand> command(new DisconnectCommand(this
//                                    , (WSAESHUTDOWN == error && disconnectReason_.empty())?disconnectReason_:description));
//    if (!command->execute())
//    {
//        TP_FATAL(tracer_, mode, _T("准备断开连接时发送连接请求失败"));
//        return;
//    }
//
//    state_ = connection_status::disconnecting;
//    TP_TRACE(tracer_, mode , _T("发送断开请求,") << description);
//    command.release();
//}
//
//void ProcessPipe::onRead(const ICommand& command, size_t bytes_transferred)
//{
//    TP_TRACE(tracer_, transport_mode::Receive, _T("读请求 '")<< (int)&command <<_T("' 成功返回!"));
//
//    reading_ = false;
//
//#ifdef DUMPFILE
//    int rawLen = bytes_transferred;
//    ReadCommand* readCmd = (ReadCommand*)&command;
//    for (std::vector<io_mem_buf>::const_iterator it = readCmd->iovec().begin()
//            ; it != readCmd->iovec().end() && rawLen > 0
//            ; ++ it )
//    {
//        int len = 0;
//        if ( it->len > rawLen)
//        {
//            len = rawLen;
//            rawLen = 0;
//        }
//        else
//        {
//            len = it->len;
//            rawLen -= it->len;
//        }
//
//        for (int i = 0; i < len; ++ i)
//        {
//            if ((unsigned)(it->buf[i] + 1) > 256)
//                (*is) << "<" << (int)it->buf[i] << ">";
//            else if ( ::isalpha(it->buf[i]) )
//                (*is) << it->buf[i];
//            else  if ( ::isdigit(it->buf[i]) )
//                (*is) << it->buf[i];
//            else if (_T('\r') == it->buf[i])
//                (*is) << "<\\r>";
//            else if (_T('\n') == it->buf[i])
//                (*is) << "<\\n>" << std::endl;
//            else if (_T('\t') == it->buf[i])
//                (*is) << "\t<\\t>";
//            else if (::isprint(it->buf[i]))
//                (*is) << "<" << (int)it->buf[i] << ":" << it->buf[i] << ">";
//            else
//                (*is) << "<" << (int)it->buf[i] << ">";
//        }
//        is->flush();
//    }
//#endif
//
//    if (!incoming_.increaseBytes(bytes_transferred))
//    {
//        tstring err = _T("计算接收字节数时发生错误");
//        TP_FATAL(tracer_, transport_mode::Receive, err);
//        doDisconnect(transport_mode::Receive, 0, err);
//        return;
//    }
//
//    try
//    {
//        std::vector<io_mem_buf> ioBuf;
//        incoming_.dataBuffer(ioBuf);
//        context_.inMemory(&ioBuf, -1);
//
//        size_t readLen = protocol_->onReceived( context_ );
//        if (!incoming_.decreaseBytes(readLen))
//        {
//            tstring err = _T("计算用户读字节数时发生错误");
//            TP_FATAL(tracer_, transport_mode::Receive, err);
//            doDisconnect(transport_mode::Receive, 0, err);
//            return;
//        }
//
//        if ( connection_status::connected != state_)
//            return;
//
//        //int count = 0;
//        //Buffer<buffer_chain_t>& dataBuffer = context_.GetOutBuffer().rawBuffer();
//        //buffer_chain_t* ptr = null_ptr;
//        //while(null_ptr != (ptr = dataBuffer.pop()))
//        //{
//        //	outgoing_.send(ptr);
//        //	++ count;
//        //}
//        //if( 0!= count)
//        //	doWrite();
//    }
//    catch (const Exception& ex)
//    {
//        tstring err = ::concat<tstring>(_T("计算用户读字节数时发生异常 - "), toTstring(ex.what()));
//        TP_FATAL(tracer_, transport_mode::Receive, _T("计算用户读字节数时发生异常 ") << ex);
//        doDisconnect(transport_mode::Receive, 0, err);
//        return;
//    }
//    catch (const std::exception& e)
//    {
//        tstring err = ::concat<tstring>(_T("计算用户读字节数时发生异常 - "), toTstring(e.what()));
//        TP_FATAL(tracer_, transport_mode::Receive, err);
//        doDisconnect(transport_mode::Receive, 0, err);
//        return;
//    }
//    doRead();
//}
//
//void ProcessPipe::onWrite(const ICommand& command, size_t bytes_transferred)
//{
//    TP_TRACE(tracer_, transport_mode::Send, _T("写请求 '")<< (int)&command <<_T("' 成功返回!"));
//
//
//#ifdef DUMPFILE
//    int rawLen = bytes_transferred;
//    WriteCommand* writeCmd = (WriteCommand*)&command;
//    for (std::vector<io_mem_buf>::const_iterator it = writeCmd->iovec().begin()
//            ; it != writeCmd->iovec().end() && rawLen > 0
//            ; ++ it )
//    {
//        int len = 0;
//        if ( it->len > rawLen)
//        {
//            len = rawLen;
//            rawLen = 0;
//        }
//        else
//        {
//            len = it->len;
//            rawLen -= it->len;
//        }
//
//        for (int i = 0; i < len; ++ i)
//        {
//            if ((unsigned)(it->buf[i] + 1) > 256)
//                (*os) << "<" << (int)it->buf[i] << ">";
//            else if ( ::isalpha(it->buf[i]) )
//                (*os) << it->buf[i];
//            else  if ( ::isdigit(it->buf[i]) )
//                (*os) << it->buf[i];
//            else if (_T('\r') == it->buf[i])
//                (*os) << "<\\r>";
//            else if (_T('\n') == it->buf[i])
//                (*os) << "<\\n>" << std::endl;
//            else if (_T('\t') == it->buf[i])
//                (*os) << "\t<\\t>";
//            else if (::isprint(it->buf[i]))
//                (*os) << "<" << (int)it->buf[i] << ":" << it->buf[i] << ">";
//            else
//                (*os) << "<" << (int)it->buf[i] << ">";
//        }
//        os->flush();
//    }
//#endif
//
//    writing_ = false;
//    outgoing_.clearBytes(bytes_transferred);
//    doWrite();
//}
//
//void ProcessPipe::onError(const ICommand& command
//                              , transport_mode::type mode
//                              , errcode_t error
//                              , const tstring& description)
//{
//    switch ( mode )
//    {
//    case transport_mode::Receive:
//        TP_TRACE(tracer_, transport_mode::Receive, _T("读请求 '")
//                 << (int)&command
//                 <<_T("' 错误返回,")
//                 << description);
//        reading_ = false;
//        break;
//    case transport_mode::Send:
//        TP_TRACE(tracer_, transport_mode::Send, _T("写请求 '")
//                 << (int)&command
//                 << _T("' 错误返回,")
//                 << description);
//        writing_ = false;
//        break;
//    default:
//        assert(false);
//        break;
//    }
//
//    doDisconnect(mode, error, description);
//}
//
//const tstring& ProcessPipe::host() const
//{
//    return host_;
//}
//
//const tstring& ProcessPipe::peer() const
//{
//    return peer_;
//}
//
//time_t ProcessPipe::timeout() const
//{
//    return timeout_;
//}
//
//const tstring& ProcessPipe::toString() const
//{
//    return toString_;
//}
//
//void ProcessPipe::onDisconnected(const ICommand& command
//                                     , errcode_t error
//                                     , const tstring& description)
//{
//    TP_TRACE(tracer_, transport_mode::Both , _T("断开请求 '")
//             << (int)&command
//             <<_T("' 返回!"));
//
//    state_ = connection_status::disconnected;
//    protocol_->onDisconnected(context_,error, description);
//}
//
//buffer_chain_t* ProcessPipe::allocateProtocolBuffer()
//{
//    return protocol_->createBuffer(context_);
//}
//
_jingxian_end