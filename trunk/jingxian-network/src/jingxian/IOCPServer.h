
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
	* 取得地址的描述
	*/
	virtual const tstring& toString() const;

private:
	
	NOCOPY(IOCPServer);

	proactor _proactor;

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