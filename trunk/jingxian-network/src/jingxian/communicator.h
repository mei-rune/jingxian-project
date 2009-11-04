
#ifndef _communicator_h_
#define _communicator_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/buffer.h"
# include "jingxian/endpoint.h"
# include "jingxian/exception.h"
# include "jingxian/IConcurrentPort.h"

_jingxian_begin

enum SystemEvent
{
    CtrlC = 0,
    CtrlBreak = 1,
    CtrlClose = 2,
    CtrlLogoff = 5,
    CtrlShutdown = 6
}

//public interface ICommunicatingWalker
//{
//    void OnBeginWalk(ICommunicator reactorCore);
//    void OnEndWalk(ICommunicator reactorCore);

//    void OnBeginWalk(IReactorCore reactorCore);
//    void OnEndWalk(IReactorCore reactorCore);

//    void OnSession(IReactorCore reactorCore, ISession session);
//    void OnListenPort(IReactorCore reactorCore, IAcceptor listeningPort);
//}

class communicator : public IConcurrentPort
{
public:

    /**
     * 开始运行直到调用Interrupt才返回
     */
    virtual void runForever() = 0;

    /**
     * 停止运行
     */
    virtual void interrupt() = 0;

    /**
     * 引发一个异常事件
     */
    virtual void send(const Exception& e) = 0;

    /**
     * 引发一个系统事件
     */
    virtual void send(const SystemEvent& e) = 0;
};

_jingxian_end

#endif //_communicator_h_