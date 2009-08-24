

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
# include "jingxian/logging/logging.hpp"
# include "jingxian/networks/IOCPServer.h"
# include "jingxian/networks/InternalBuffer.h"
# include "jingxian/networks/IncomingBuffer.h"
# include "jingxian/networks/OutgoingBuffer.h"

_jingxian_begin

class ProtocolContextEx : public ProtocolContext
{
public:
	ProtocolContextEx(IReactorCore* core
		, ITransport* transport)
		: ProtocolContext(core, transport)
	{
	}

	void inBuffer(IInBuffer* inBuffer)
	{
		inBuffer_ = inBuffer;
	}
	
	void outBuffer(IOutBuffer* outBuffer)
	{
		outBuffer_ = outBuffer_;
	}
};

/**
 * On��ͷ�ĺ������û�ֱ�ӵ��õķ����в�����ʹ�á�
 * ���û����õķ���ΪITransport�ӿ��еķ���
 */
class ConnectedSocket : public ITransport
{
public:

	ConnectedSocket(IOCPServer* core
								 , SOCKET socket
								 , const tstring& host
								 , const tstring& peer);

	virtual ~ConnectedSocket( );

	/**
	 * @implements initialize
	 */
	virtual void initialize();

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
    virtual void write(buffer_chain_t* buffer);

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
    virtual const tstring& host() const;

	/**
	 * @implements peer
	 */
    virtual const tstring& peer() const;

	/**
	 * @implements timeout
	 */
    virtual time_t timeout() const;

	/**
	 * @implements toString
	 */
	virtual const tstring& toString() const;


	SOCKET handle(){return socket_;}
	ITracer* tracer(){return tracer_;}

	
	void onWrite(size_t bytes_transferred);
	void onRead(size_t bytes_transferred);
	void onError(transport_mode::type mode, errcode_t error, const tstring& description);
	void onDisconnected(errcode_t error, const tstring& description);

private:
	NOCOPY(ConnectedSocket);

	void doRead();
	void doWrite();
	void doDisconnect(transport_mode::type mode, errcode_t error, const tstring& description);


	/// iocp���������
	IOCPServer* core_;
	/// socket ����
	SOCKET socket_;
	/// ���ص�ַ
	tstring host_;
	/// Զ�̵�ַ
	tstring peer_;
	/// ������ǰ������״̬
	connection_status::type state_;
	/// ��ʱʱ��
	time_t timeout_;
	/// Э�鴦����
	IProtocol* protocol_;
	/// �����������
	ProtocolContextEx context_;
	/// �Ƿ��ѳ�ʼ��
	bool isInitialize_;
	///��ͣ����ʱ������������ʱ���λ��

	bool stopReading_;
	/// ��ʾ����һ��������,����û�з���
	bool reading_;
	IncomingBuffer incoming_;

	/// ��ʾ����һ��д����,����û�з���
	bool writing_;
	OutgoingBuffer outgoing_;

	/// ��־����
	ITracer* tracer_;
	tstring toString_;

	//bool _writing = false;
	//LinkedList<ByteBuffer> _writebuf = new LinkedList<ByteBuffer>();
	//int _writeBufferedSize = 0; //_writebuf������byte���ܺ�

	//bool _isDisconnectingOnEmpty = false;
	//Exception _disconnectingArg = null;


	//bool _isShutdownOnEmpty = false;
	//SocketShutdown _shutdownArg = SocketShutdown.Both;

	//bool _read_shutdown = false;
	//bool _write_shutdown = false;


	//// û��ȡ������ʱ����timeout�쳣��ʱ����
	//TimeSpan _timeout = IOCPCore.TIMEOUT_INTERVAL;
	//// ��һ�δ���timeout�쳣��ʱ��
	//DateTime _triggeredTime = DateTime.Now;
};

_jingxian_end

#endif // _CONNECTED_SOCKET_H_