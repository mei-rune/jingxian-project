
#ifndef _IOCPServer_H_
#define _IOCPServer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <hash_map>
# include "jingxian/string/string.h"
# include "jingxian/logging/logging.h"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/connection_status.h"
# include "jingxian/networks/networking.h"
# include "jingxian/networks/ThreadDNSResolver.h"

_jingxian_begin

class IOCPServer : public IReactorCore
{
public:
	IOCPServer(void);

	virtual ~IOCPServer(void);


	/**
	 * 初始化端口(如果已经初始化返回true)
     * @param[ in ] 并行线程数
	 */
	bool initialize( size_t number_of_threads );

	/**
	 * @implements connectWith
	 */
    virtual void connectWith(const tchar* endPoint
                            , OnBuildConnectionComplete onComplete
                            , OnBuildConnectionError onError
                            , void* context );
	
	/**
	 * @implements listenWith
	 */
    virtual IAcceptor* listenWith(const tchar* endPoint);

	
	/**
	 * @implements send
	 */
    virtual bool send( IRunnable* runnable );

	/**
	 * @implements runForever
	 */
	virtual void runForever();

	/**
	 * @implements interrupt
	 */
	virtual void interrupt();

	/**
	 * @implements bind
	 */
	virtual bool bind(HANDLE systemHandler, void* completion_key);

	/**
	 * @implements resolver
	 */
	virtual IDNSResolver& resolver();
	
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

private:
	NOCOPY(IOCPServer);
	
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
	int handle_events (uint32_t milli_seconds);

	void application_specific_code (ICommand *asynch_result,
		size_t bytes_transferred,
		const void *completion_key,
		errcode_t error);

	HANDLE completion_port_;
	u_long number_of_threads_;

	uint32_t _timeout;

	bool _isRunning;

	stdext::hash_map<tstring, IConnectionBuilder* > _connectionBuilders;

	stdext::hash_map<tstring, IAcceptorFactory* > _acceptorFactories;
	
	stdext::hash_map<tstring, IAcceptor* > _acceptors;

	ThreadDNSResolver resolver_;
	
	ILogger* _logger;

	tstring toString_;
};

_jingxian_end

#endif //_IOCPServer_H_