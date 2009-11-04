
#ifndef _Connector_H_
#define _Connector_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <functional>
# include "jingxian/string/string.h"
# include "jingxian/IReactorCore.h"

_jingxian_begin

class Connector
{
public:
    template<typename F1, typename F2, typename T>
    class closure
    {
    public:
        closure(const F1& func1, const F2& func2, T context)
                : function1_(func1)
                , function2_(func2)
                , context_(context)
        {
        }

        static void OnComplete(ITransport* transport, void* context)
        {
            std::auto_ptr<closure> self(static_cast<closure*>(context));
            self->function1_(transport, self->context_);
        }

        static void OnError(const ErrorCode& err, void* context)
        {
            std::auto_ptr<closure> self(static_cast<closure*>(context));
            self->function2_(err, self->context_);
        }

    private:
        F1 function1_;
        F2 function2_;
        T context_;
    };

    template<typename C, typename F1, typename F2, typename T>
    class closure_0
    {
    public:

        closure_0(C c, const F1& func1, const F2& func2, T context)
                : c_(c)
                , function1_(func1)
                , function2_(func2)
                , context_(context)
        {
        }

        static void OnComplete(ITransport* transport, void* context)
        {
            std::auto_ptr<closure_0> self(static_cast<closure_0*>(context));
            (self->c_->*(self->function1_))(transport, self->context_);
        }

        static void OnError(const ErrorCode& err, void* context)
        {
            std::auto_ptr<closure_0> self(static_cast<closure_0*>(context));
            (self->c_->*(self->function2_))(err, self->context_);
        }

    private:
        C c_;
        F1 function1_;
        F2 function2_;
        T context_;
    };

    Connector(IReactorCore* core, const tchar* host)
            : core_(core)
            , host_(host)
    {
    }

    virtual ~Connector()
    {
        reset(null_ptr);
    }

    void reset(IReactorCore* core)
    {
        core_ = core;
    }

    /**
    * 发起一个监听请求
    */
    template< typename F1, typename F2, typename T>
    static void connectWith(IReactorCore* core
                            , const char* host
                            , F1 onComplete
                            , F2 onError
                            , T context)
    {
        typedef closure<F1, F2, T> closure_type;
        core->connectWith(host
                          , closure_type::OnComplete
                          , closure_type::OnError
                          , new closure_type(onComplete, onError, context));
    }

    /**
     * 发起一个监听请求
     */
    template<typename C, typename F1, typename F2, typename T>
    static void connectWith(IReactorCore* core
                            , const char* host
                            , C c
                            , F1 onComplete
                            , F2 onError
                            , T context)
    {
        typedef closure_0<C, F1, F2, T> closure_type;
        core->connectWith(host
                          , closure_type::OnComplete
                          , closure_type::OnError
                          , new closure_type(c, onComplete, onError, context));
    }


    /**
     * 发起一个监听请求
     */
    template< typename F1, typename F2, typename T>
    void connectWith(F1 onComplete
                     , F2 onError
                     , T context)
    {
        typedef closure<F1, F2, T> closure_type;
        core_->connectWith(host_.c_str()
                           , closure_type::OnComplete
                           , closure_type::OnError
                           , new closure_type(onComplete, onError, context));
    }

    /**
     * 发起一个监听请求
     */
    template<typename C, typename F1, typename F2, typename T>
    void connectWith(C c
                     , F1 onComplete
                     , F2 onError
                     , T context)
    {
        typedef closure_0<C, F1, F2, T> closure_type;
        core_->connectWith(host_.c_str()
                           , closure_type::OnComplete
                           , closure_type::OnError
                           , new closure_type(c, onComplete, onError, context));
    }


    /**
     * 发起一个监听请求
     */
    virtual void connectWith(OnBuildConnectionComplete onComplete
                             , OnBuildConnectionError onError
                             , void* context)
    {
        core_->connectWith(host_.c_str(), onComplete, onError, context);
    }

    virtual void close()
    {
        reset(null_ptr);
    }

    bool isNull() const
    {
        return null_ptr == core_;
    }

    /**
    * 取得地址的描述
    */
    virtual const tstring& toString() const
    {
        return core_->toString();
    }
private:
    NOCOPY(Connector);
    IReactorCore* core_;
    tstring host_;
};


template<typename T, typename CONTEXT>
class ConnectProxy
{
public:

    typedef void (T::*ONCOMPLETE)(ITransport*, CONTEXT);
    typedef void (T::*ONERROR)(const ErrorCode&, CONTEXT);

    ConnectProxy(T* t, const tstring& host, IReactorCore* core, ONCOMPLETE onComplete, ONERROR onError, CONTEXT context)
            : t_(t)
            , host_(host)
            , core_(core)
            , function1_(onComplete)
            , function2_(onError)
            , context_(context)
    {
    }

    void connectWith()
    {
        core_->connectWith(host_.c_str()
                           , ConnectProxy::OnComplete
                           , ConnectProxy::OnError
                           , this);
    }

    void shutdown()
    {
        t_ = null_ptr;
    }

    const tstring& host() const
    {
        return host_;
    }

    static void OnComplete(ITransport* transport, void* context)
    {
        std::auto_ptr<ConnectProxy> self(static_cast<ConnectProxy*>(context));
        if (null_ptr != self->t_)
            (self->t_->*(self->function1_))(transport, self->context_);
    }

    static void OnError(const ErrorCode& err, void* context)
    {
        std::auto_ptr<ConnectProxy> self(static_cast<ConnectProxy*>(context));
        if (null_ptr != self->t_)
            (self->t_->*(self->function2_))(err, self->context_);
    }

private:
    NOCOPY(ConnectProxy);
    T* t_;
    tstring host_;
    IReactorCore* core_;
    ONCOMPLETE function1_;
    ONERROR function2_;
    CONTEXT context_;
};

_jingxian_end

#endif //_Connector_H_ 