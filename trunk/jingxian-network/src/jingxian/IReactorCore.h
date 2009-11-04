
#ifndef _IReactorCore_H_
#define _IReactorCore_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/logging/logging.h"
# include "jingxian/IConcurrentPort.h"
# include "jingxian/ITransport.h"
# include "jingxian/IProtocol.h"
# include "jingxian/IConnectionBuilder.h"
# include "jingxian/IAcceptor.h"
# include "jingxian/ProtocolContext.h"
# include "jingxian/IDNSResolver.h"

_jingxian_begin

class IReactorCore : public IConcurrentPort
{
public:
    virtual ~IReactorCore() {};

    /**
    * 创建一个连接
    * @param[ in ] endPoint 连接的目标地址
    * @param[ in ] buildProtocol 成功连接时创建协议处理器的回调函数
    * @param[ in ] onConnectError 连接失败时错误处理的回调函数
    * @param[ in ] context 回调函数的上下文
    * @remarks 回调函数可能会在调用线程中直接被调用
    */
    virtual void connectWith(const tchar* endPoint
                             , OnBuildConnectionComplete onComplete
                             , OnBuildConnectionError onError
                             , void* context) = 0;

    /**
    * 创建一个监听服务
    *
    * @param[ in ] endPoint 监听的地址
    * @param[ in ] protocolFactory 协议处理器工厂
    * @return 成功进返回创建的对象，失败时返回 NULL
    * @remarks 不要尝试删除返回的对象，该对象已被 IReactorCore 持有。
    */
    virtual bool listenWith(const tchar* endPoint, IProtocolFactory* protocolFactory) = 0;

    /**
     * 开始运行直到调用Interrupt才返回
     */
    virtual void runForever() = 0;

    /**
     * 停止运行
     */
    virtual void interrupt() = 0;

    /**
     * 是不是在运行中
     */
    virtual bool isRunning() const = 0;

    /**
     * 将句柄绑定到本端口
     */
    virtual bool bind(HANDLE systemHandler, void* completion_key) = 0;

    /**
     * 取得 dns 解析接口
     */
    virtual IDNSResolver& resolver() = 0;
};

_jingxian_end

#endif // _IReactorCore_H_