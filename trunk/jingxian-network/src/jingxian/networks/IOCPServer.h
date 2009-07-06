
#ifndef _IOCPServer_H_
#define _IOCPServer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <hash_map>
# include "jingxian/string/string.hpp"
# include "jingxian/logging/logging.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/proactor.h"
# include "jingxian/networks/connection_status.h"
# include "jingxian/networks/sockets/basesocket.h"
# include "jingxian/networks/TCPFactory.h"


_jingxian_begin

class IOCPServer : public IReactorCore
{
public:
	IOCPServer(void);

	virtual ~IOCPServer(void);


    /**
	 * 创建一个连接器
	 */
    virtual void connectWith(const tchar* endPoint
                            , BuildProtocol buildProtocol
                            , OnConnectError onConnectError
                            , void* context );
	
    /**
	 * 创建一个监听服务
	 */
    virtual IAcceptor* listenWith(const tchar* endPoint
			, IProtocolFactory* protocolFactory);

	
    /**
     * 将执行方法发送到线程等待队列,稍后执行
     *
     * @param[ in ] run 执行方法
	 */
    virtual bool send( IRunnable* runnable );

	/**
	 * 开始运行直到调用Interrupt才返回
	 */
	virtual void runForever();

	/**
	 * 停止运行
	 */
	virtual void interrupt();

	/**
	 * 将句柄绑定到本端口
	 */
	virtual bool bind(HANDLE systemHandler, void* completion_key);
	
	/**
	 *  空闲时执行的回调函数，子类可以继承本函数 
	 */
	virtual void onIdle();

	/**
	 * 发生错误
	 */
	virtual void onExeception(int errCode, const tstring& description);

	/**
	* 取得地址的描述
	*/
	virtual const tstring& toString() const;

	
	/**
	 * 取得socket工厂
	 */
	TCPFactory& tcpFactory();

private:
	NOCOPY(IOCPServer);

	/**
	 * 初始化端口(如果已经初始化返回true)
     * @param[ in ] 并行线程数
	 */
	bool open ( size_t number_of_threads );
	
	/**
	 * 关闭本对象
	 */
	void close (void);

	/**
	 * 发送一个已经完成的请求到完成端口
	 */
	bool post(ICommand *result);

	/**
	 * 获取已完成的事件,并处理这个事件
	 * @return 超时返回1,获取到事件并成功处理返回0,获取失败返回-1
	 */
	int handle_events ( u_int32_t milli_seconds);

	void application_specific_code (ICommand *asynch_result,
		size_t bytes_transferred,
		const void *completion_key,
		u_long error);

	HANDLE completion_port_;
	u_long number_of_threads_;


	TCPFactory tcpFactory_;

	u_int32_t _timeout;

	bool _isRunning;

	stdext::hash_map<tstring, IConnectionBuilder* > _connectionBuilders;

	stdext::hash_map<tstring, IAcceptorFactory* > _acceptorFactories;
	
	stdext::hash_map<tstring, IAcceptor* > _acceptors;
	
	ILogger* _logger;

	tstring toString_;
};

_jingxian_end

#endif //_IOCPServer_H_