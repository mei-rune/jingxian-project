
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
# include "jingxian/ISession.h"
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/connection_status.h"
# include "jingxian/networks/networking.h"
# include "jingxian/networks/ThreadDNSResolver.h"
# include "jingxian/networks/ListenPort.H"

_jingxian_begin


typedef std::list<ISession*> SessionList;

class IOCPServer : public IReactorCore
{
public:
    IOCPServer(void);

    virtual ~IOCPServer(void);


    /**
     * 初始化端口(如果已经初始化返回true)
       * @param[ in ] 并行线程数
     */
    bool initialize(size_t number_of_threads);

    /**
     * @implements connectWith
     */
    virtual void connectWith(const tchar* endPoint
                             , OnBuildConnectionComplete onComplete
                             , OnBuildConnectionError onError
                             , void* context);

    /**
     * @implements listenWith
     */
    virtual bool listenWith(const tchar* endPoint, IProtocolFactory* protocolFactory);


    /**
     * @implements send
     */
    virtual bool send(IRunnable* runnable);

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
     * @implements isRunning
     */
    virtual bool isRunning() const;

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
     * 程序运行的基本路径
     */
    const tstring& basePath() const;

    /**
     * 添加一个连接
     * @param[ in ] session 会话对象
     * @return 返回连接容器中的位置
     */
    SessionList::iterator addSession(ISession* session);

    /**
     * 删除一个连接
     * @param[ in ] it 连接容器中的位置
     */
    void removeSession(SessionList::iterator& it);

    /**
    * 取得地址的描述
    */
    virtual const tstring& toString() const;

private:
    NOCOPY(IOCPServer);

    /**
     * 关闭本对象
     */
    void close(void);

    /**
     * 发送一个已经完成的请求到完成端口
     */
    bool post(ICommand *result);

    /**
     * 获取已完成的事件,并处理这个事件
     * @return 超时返回1,获取到事件并成功处理返回0,获取失败返回-1
     */
    int handle_events(uint32_t milli_seconds);

    /**
     * 等侍未返回的 IO 请求
     */
    void wait(time_t seconds);

    /**
     * 判断是不是有未返回的 IO 请求
     */
    bool isPending();

    void application_specific_code(ICommand *asynch_result,
                                   size_t bytes_transferred,
                                   const void *completion_key,
                                   errcode_t error);

    /// 完成端口句柄
    HANDLE completion_port_;
    /// 可以并发的线程数
    u_long number_of_threads_;
    /// 是不是在运行中
    bool isRunning_;
    /// socket 连接创建工厂
    stdext::hash_map<tstring, IConnectionBuilder* > connectionBuilders_;
    /// Acceptor创建工厂
    stdext::hash_map<tstring, IAcceptorFactory* > acceptorFactories_;
    /// 正在监听的Acceptor
    stdext::hash_map<tstring, ListenPort*> listenPorts_;
    /// dns 解析类
    ThreadDNSResolver resolver_;
    /// 保存所有的 connection
    SessionList sessions_;
    /// 程序运行的基本路径
    tstring path_;
    /// 日志接口
	logging::logger logger_;
    /// 实例的描述
    tstring toString_;
};

_jingxian_end

#endif //_IOCPServer_H_