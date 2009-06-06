

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
 * On��ͷ�ĺ������û�ֱ�ӵ��õķ����в�����ʹ�á�
 * ���û����õķ���ΪITransport�ӿ��еķ���
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

	/// iocp���������
	IOCPServer* core_;
	/// socket ����
	SOCKET socket_;
	/// ���ص�ַ
	NetAddress host_;
	/// Զ�̵�ַ
	NetAddress peer_;
	/// ������ǰ������״̬
	connection_status::type state_;
	/// Э�鴦����
	std::auto_ptr<IProtocol> protocol_;
	/// �����������
	//ProtocolContext* context_;
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

	////��ͣ����ʱ������������ʱ���λ��
	//LinkedList<ByteBuffer> _readBuffer = new LinkedList<ByteBuffer>();
	//bool _reading = false;

	//// û��ȡ������ʱ����timeout�쳣��ʱ����
	//TimeSpan _timeout = IOCPCore.TIMEOUT_INTERVAL;
	//// ��һ�δ���timeout�쳣��ʱ��
	//DateTime _triggeredTime = DateTime.Now;
};

_jingxian_end

#endif // _CONNECTED_SOCKET_H_