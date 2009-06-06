

#ifndef _CONNECTED_SOCKET_H_
#define _CONNECTED_SOCKET_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <Winsock2.h>
# include "jingxian/IProtocol.h"
# include "jingxian/ProtocolContext.h"
# include "jingxian/networks/connection_status.h"
# include "jingxian/networks/proactor.h"
# include "jingxian/logging/logging.hpp"
# include "jingxian/networks/IOCPServer.h"

_jingxian_begin

/**
 * On开头的函数在用户直接调用的方法中不可以使用。
 * 给用户调用的方法为ITransport接口中的方法
 */
class ConnectedSocket : public ITransport
{
public:

	ConnectedSocket(IOCPServer* core, SOCKET socket);

	virtual ~ConnectedSocket( );

	/**
	 * @implements bindProtocol
	 */
    virtual void bindProtocol(IProtocol* protocol);

	/**
	 * @implements startReading
	 */
    virtual void startReading();

	/**
	 * @implements stopReading
	 */
    virtual void stopReading();

	/**
	 * @implements write
	 */
    virtual void write(char* buffer);
	
	/**
	 * @implements write
	 */
    virtual void write(char* buffer, int offest, int length);
	
	/**
	 * @implements write
	 */
    virtual void write(Buffer& buffer);

	/**
	 * @implements disconnection
	 */
    virtual void disconnection();
	
	/**
	 * @implements disconnection
	 */
    virtual void disconnection(const tstring& error);

	/**
	 * @implements host
	 */
    virtual const IEndpoint& host() const;

	/**
	 * @implements peer
	 */
    virtual const IEndpoint& peer() const;

	/**
	 * @implements timeout
	 */
    virtual time_t timeout() const;

	/**
	 * @implements toString
	 */
	virtual const tstring& toString() const;

	void onConnected();
	void onDisconnected(int error, const tstring& description);
	void setPeer( sockaddr* addr)
	{
		peer_.
	}
	void setHost( sockaddr* addr);
	void initialize();

private:

	/// iocp对象的引用
	IOCPServer* core_;
	/// socket 对象
	SOCKET socket_;
	/// 本地地址
	NetAddress host_;
	/// 远程地址
	NetAddress peer_;
	/// 本对象当前所处的状态
	connection_status::type state_;
	/// 协议处理器
	std::auto_ptr<IProtocol> protocol_;
	/// 对象的上下文
	//ProtocolContext* context_;
	/// 日志对象
	ITracer* tracer_;
	tstring toString_;

	//bool _writing = false;
	//LinkedList<ByteBuffer> _writebuf = new LinkedList<ByteBuffer>();
	//int _writeBufferedSize = 0; //_writebuf内所有byte的总和

	//bool _isDisconnectingOnEmpty = false;
	//Exception _disconnectingArg = null;


	//bool _isShutdownOnEmpty = false;
	//SocketShutdown _shutdownArg = SocketShutdown.Both;

	//bool _read_shutdown = false;
	//bool _write_shutdown = false;

	////暂停数据时读来的数据临时存放位置
	//LinkedList<ByteBuffer> _readBuffer = new LinkedList<ByteBuffer>();
	//bool _reading = false;

	//// 没有取到数据时触发timeout异常的时间间隔
	//TimeSpan _timeout = IOCPCore.TIMEOUT_INTERVAL;
	//// 上一次触发timeout异常的时间
	//DateTime _triggeredTime = DateTime.Now;
};

_jingxian_end

#endif // _CONNECTED_SOCKET_H_