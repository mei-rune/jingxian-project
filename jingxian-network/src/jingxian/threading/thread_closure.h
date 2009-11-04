
#ifndef THREAD_CLOSURE_H
#define THREAD_CLOSURE_H

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

#ifdef JINGXIAN_MT

// Include files
# ifdef CPP_TR1
# ifdef HAS_BOOST
# include "boost/tr1/memory.h"
# include "boost/tr1/type_traits.h"
# else
# include <tr1/memory>
# include <tr1/type_traits.h>
# endif
# endif

_jingxian_begin

template< typename R, typename T>
R _cast(T t)
{
    return t;
}

#ifdef CPP_TR1
template< typename R>
R _cast(std::tr1::shared_ptr< typename std::tr1::remove_pointer<R>::type > t)
{
    return t.get();
}
#endif

template<typename F>
class thread_closure_0
{
public:

    thread_closure_0(const F& f, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_0 *self = static_cast<thread_closure_0*>(c);
        try
        {
            self->_function();
        }
        catch (...)
        {
            delete self;
            throw;
        }

        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
};

template<typename F, typename P>
class thread_closure_1
{
public:

    thread_closure_1(const F& f, const P& x, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
            , arg1(x)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_1 *self = static_cast<thread_closure_1*>(c);
        try
        {
            self->_function(self->arg1);
        }
        catch (...)
        {
            delete self;
            throw;
        }
        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
    P arg1;
};

template<typename F, typename P1, typename P2>
class thread_closure_2
{
public:

    thread_closure_2(const F& f, P1 x1, P2 x2, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
            , arg1(x1)
            , arg2(x2)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_2 *self = static_cast<thread_closure_2*>(c);
        try
        {
            self->_function(self->arg1, self->arg2);
        }
        catch (...)
        {
            delete self;
            throw;
        }
        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
    P1 arg1;
    P2 arg2;
};

template<typename F, typename P1, typename P2, typename P3>
class thread_closure_3
{
public:

    thread_closure_3(const F& f, P1 x1, P2 x2, P3 x3, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
            , arg1(x1)
            , arg2(x2)
            , arg3(x3)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_3 *self = static_cast<thread_closure_3*>(c);
        try
        {
            self->_function(self->arg1, self->arg2, self->arg3);
        }
        catch (...)
        {
            delete self;
            throw;
        }
        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
    P1 arg1;
    P2 arg2;
    P3 arg3;
};

template<typename F, typename P1, typename P2, typename P3, typename P4>
class thread_closure_4
{
public:

    thread_closure_4(const F& f, P1 x1, P2 x2, P3 x3, P4 x4, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
            , arg1(x1)
            , arg2(x2)
            , arg3(x3)
            , arg4(x4)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_4 *self = static_cast<thread_closure_4*>(c);
        try
        {
            self->_function(self->arg1, self->arg2, self->arg3, self->arg4);
        }
        catch (...)
        {
            delete self;
            throw;
        }
        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
    P1 arg1;
    P2 arg2;
    P3 arg3;
    P4 arg4;
};

template<typename F, typename P1, typename P2, typename P3, typename P4, typename P5>
class thread_closure_5
{
public:

    thread_closure_5(const F& f, P1 x1, P2 x2, P3 x3, P4 x4, P5 x5, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
            , arg1(x1)
            , arg2(x2)
            , arg3(x3)
            , arg4(x4)
            , arg5(x5)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_5 *self = static_cast<thread_closure_5*>(c);
        try
        {
            self->_function(self->arg1, self->arg2, self->arg3, self->arg4, self->arg5);
        }
        catch (...)
        {
            delete self;
            throw;
        }
        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
    P1 arg1;
    P2 arg2;
    P3 arg3;
    P4 arg4;
    P5 arg5;
};

template<typename F, typename P1, typename P2, typename P3, typename P4, typename P5, typename P6>
class thread_closure_6
{
public:

    thread_closure_6(const F& f, P1 x1, P2 x2, P3 x3, P4 x4, P5 x5, P6 x6, const tchar* nm = 0)
            : _function(f)
            , _name((0 == nm) ? _T("") : nm)
            , arg1(x1)
            , arg2(x2)
            , arg3(x3)
            , arg4(x4)
            , arg5(x5)
            , arg6(x6)
    {
    }

    static void start_routine(void* c)
    {
        thread_closure_6 *self = static_cast<thread_closure_6*>(c);
        try
        {
            self->_function(self->arg1, self->arg2, self->arg3, self->arg4, self->arg5, self->arg6);
        }
        catch (...)
        {
            delete self;
            throw;
        }
        delete self;
    }

    const tstring& name() const
    {
        return _name;
    }
private:
    F _function;
    tstring _name;
    P1 arg1;
    P2 arg2;
    P3 arg3;
    P4 arg4;
    P5 arg5;
    P6 arg6;
};

_jingxian_end

#endif //JINGXIAN_MT

#endif // THREAD_CLOSURE_H