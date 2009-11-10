
#ifndef __connection_base_h_
#define __connection_base_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */


// Include files
#include <list>

#ifndef LOCK
#define LOCK() 
#endif

template<class T>
class _connection_base
{
public:
	virtual ~_connection_base(){}
};

template<>
class _connection_base<void ()>
{
public:
	virtual ~_connection_base(){}
	virtual void call() = 0;
};

template<typename result_type>
class _connection_base<result_type ()>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call() = 0;
};

template<typename arg1_type>
class _connection_base<void (arg1_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type) = 0;
};

template<typename result_type, typename arg1_type>
class _connection_base<result_type (arg1_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type) = 0;
};

template<class arg1_type, class arg2_type>
class _connection_base<void (arg1_type, arg2_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type>
class _connection_base<result_type (arg1_type, arg2_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type) = 0;
};

template<class arg1_type, class arg2_type, class arg3_type>
class _connection_base<void (arg1_type, arg2_type, arg3_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type, arg3_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type, class arg3_type>
class _connection_base<result_type (arg1_type, arg2_type, arg3_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type, arg3_type) = 0;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type>
class _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type, arg3_type, arg4_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type, class arg3_type, class arg4_type>
class _connection_base<result_type (arg1_type, arg2_type, arg3_type, arg4_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type, arg3_type, arg4_type) = 0;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type>
class _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type, arg3_type, arg4_type, 
		arg5_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type>
class _connection_base<result_type (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type, arg3_type, arg4_type, 
		arg5_type) = 0;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type>
class _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type, arg3_type, arg4_type, arg5_type,
		arg6_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type>
class _connection_base<result_type (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type, arg3_type, arg4_type, arg5_type,
		arg6_type) = 0;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type, class arg7_type>
class _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type, arg3_type, arg4_type, arg5_type,
		arg6_type, arg7_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type, class arg7_type>
class _connection_base<result_type (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type, arg3_type, arg4_type, arg5_type,
		arg6_type, arg7_type) = 0;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type, class arg7_type, class arg8_type>
class _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type, arg8_type)>
{
public:
	virtual ~_connection_base(){}
	virtual void call(arg1_type, arg2_type, arg3_type, arg4_type, arg5_type,
		arg6_type, arg7_type, arg8_type) = 0;
};

template<typename result_type, class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type, class arg7_type, class arg8_type>
class _connection_base<result_type (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type, arg8_type)>
{
public:
	virtual ~_connection_base(){}
	virtual result_type call(arg1_type, arg2_type, arg3_type, arg4_type, arg5_type,
		arg6_type, arg7_type, arg8_type) = 0;
};

template<class dest_type, class args_type>
class _connection : public _connection_base<args_type>
{
public:

};

template<class dest_type>
class _connection<dest_type, void ()> : public _connection_base<void ()>
{
public:
	_connection()
		: m_pobject(NULL)
		, m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)())
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call()
	{
		(m_pobject->*m_pmemfun)();
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)();
};


template<class dest_type, class result_type>
class _connection<dest_type, result_type ()> : public _connection_base<result_type ()>
{
public:
	_connection()
		: m_pobject(NULL)
		, m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)())
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call()
	{
		return (m_pobject->*m_pmemfun)();
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)();
};

template<class dest_type, class arg1_type>
class _connection<dest_type, void (arg1_type)> 
	: public _connection_base<void (arg1_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1)
	{
		(m_pobject->*m_pmemfun)(a1);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type);
};

template<class dest_type, class result_type, class arg1_type>
class _connection<dest_type, result_type (arg1_type)> 
	: public _connection_base<result_type (arg1_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1)
	{
		return (m_pobject->*m_pmemfun)(a1);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type);
};

template<class dest_type, class arg1_type, class arg2_type>
class _connection<dest_type, void (arg1_type, arg2_type)> 
	: public _connection_base<void (arg1_type, arg2_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2)
	{
		(m_pobject->*m_pmemfun)(a1, a2);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type);
};

template<class dest_type, class result_type, class arg1_type, class arg2_type>
class _connection<dest_type, result_type (arg1_type, arg2_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2)
	{
		return (m_pobject->*m_pmemfun)(a1, a2);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type);
};

template<class dest_type, class arg1_type, class arg2_type, class arg3_type>
class _connection<dest_type, void (arg1_type, arg2_type, arg3_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2, arg3_type a3)
	{
		(m_pobject->*m_pmemfun)(a1, a2, a3);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type);
};

template<class dest_type, class result_type, class arg1_type, class arg2_type, class arg3_type>
class _connection<dest_type, result_type (arg1_type, arg2_type, arg3_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type, arg3_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2, arg3_type a3)
	{
		return (m_pobject->*m_pmemfun)(a1, a2, a3);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type);
};

template<class dest_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type>
class _connection<dest_type, void (arg1_type, arg2_type, arg3_type, arg4_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2, arg3_type a3, 
		arg4_type a4)
	{
		(m_pobject->*m_pmemfun)(a1, a2, a3, a4);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type,
		arg4_type);
};

template<class dest_type, class result_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type>
class _connection<dest_type, result_type (arg1_type, arg2_type, arg3_type, arg4_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type, arg3_type, arg4_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2, arg3_type a3, 
		arg4_type a4)
	{
		return (m_pobject->*m_pmemfun)(a1, a2, a3, a4);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type,
		arg4_type);
};

template<class dest_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type>
class _connection<dest_type, void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5)
	{
		(m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type);
};

template<class dest_type, class result_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type>
class _connection<dest_type, result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5)
	{
		return (m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type);
};

template<class dest_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type, class arg6_type>
class _connection<dest_type, void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6)
	{
		(m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5, a6);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type, arg6_type);
};

template<class dest_type, class result_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type, class arg6_type>
class _connection<dest_type, result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6)
	{
		return (m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5, a6);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type, arg6_type);
};

template<class dest_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type, class arg6_type
, class arg7_type>
class _connection<dest_type, void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type, arg7_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7)
	{
		(m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5, a6, a7);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type, arg6_type, arg7_type);
};

template<class dest_type, class result_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type, class arg6_type
, class arg7_type>
class _connection<dest_type, result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type, arg7_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7)
	{
		return (m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5, a6, a7);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type, arg6_type, arg7_type);
};

template<class dest_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type, class arg6_type
, class arg7_type, class arg8_type>
class _connection<dest_type, void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type, arg8_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type, arg8_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, void (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type, 
		arg7_type, arg8_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7, arg8_type a8)
	{
		(m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5, a6, a7, a8);
	}

private:
	dest_type* m_pobject;
	void (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type, arg6_type, arg7_type, arg8_type);
};


template<class dest_type, class result_type, class arg1_type, class arg2_type
, class arg3_type, class arg4_type, class arg5_type, class arg6_type
, class arg7_type, class arg8_type>
class _connection<dest_type, result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type, arg8_type)> 
	: public _connection_base<result_type (arg1_type, arg2_type, arg3_type
	, arg4_type, arg5_type, arg6_type, arg7_type, arg8_type)>
{
public:
	_connection()
		:m_pobject(NULL)
		,m_pmemfun(NULL)
	{
	}

	_connection(dest_type* pobject, result_type (dest_type::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type, 
		arg7_type, arg8_type))
		: m_pobject(pobject)
		, m_pmemfun(pmemfun)
	{
	}

	virtual ~_connection()
	{
	}

	virtual result_type call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7, arg8_type a8)
	{
		return (m_pobject->*m_pmemfun)(a1, a2, a3, a4, a5, a6, a7, a8);
	}

private:
	dest_type* m_pobject;
	result_type (dest_type::* m_pmemfun)(arg1_type, arg2_type, arg3_type, arg4_type,
		arg5_type, arg6_type, arg7_type, arg8_type);
};


template<class T>
class signal
{
};

template<>
class signal<void ()> : public _connection_base<void ()>
{
public:
	
	typedef signal<void ()> this_type;
	typedef _connection_base<void ()> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{
	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)())
	{
		LOCK();
		_connection<desttype, void ()>* conn = 
			new _connection<desttype, void ()>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call()
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call();

			it = itNext;
		}
		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call();

			it = itNext;
		}
	}

	void operator()()
	{
		this->call();
	}

protected:
		connections_list m_owner_slots;
		connections_list m_slots;
};

template<typename arg1_type>
class signal<void (arg1_type)> : public _connection_base<void (arg1_type)>
{
public:
	
	typedef signal<void (arg1_type)> this_type;
	typedef _connection_base<void (arg1_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type)>* conn = 
			new _connection<desttype, void (arg1_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1);

			it = itNext;
		}
		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1);

			it = itNext;
		}
	}

	void operator()(arg1_type a1)
	{
		this->call(a1);
	}
protected:
		connections_list m_owner_slots;
		connections_list m_slots;
};

template<class arg1_type, class arg2_type>
class signal<void (arg1_type, arg2_type)> 
	: public _connection_base<void (arg1_type, arg2_type)>
{
public:
	
	typedef signal<void (arg1_type, arg2_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type)>* conn = new
			_connection<desttype, void (arg1_type, arg2_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2);

			it = itNext;
		}
		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2)
	{
		this->call(a1, a2);
	}
protected:
		connections_list m_owner_slots;  
		connections_list m_slots; 
};

template<class arg1_type, class arg2_type, class arg3_type>
class signal<void (arg1_type, arg2_type, arg3_type)> 
	: public _connection_base<void (arg1_type, arg2_type, arg3_type)>
{
public:
	
	typedef signal<void (arg1_type, arg2_type, arg3_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type, arg3_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type, arg3_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type, arg3_type)>* conn = 
			new _connection<desttype, void (arg1_type, arg2_type, arg3_type)>(pclass,
			pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2, arg3_type a3)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3);

			it = itNext;
		}
		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2, arg3_type a3)
	{
		this->call(a1, a2, a3);
	}
protected:
		connections_list m_owner_slots;   
		connections_list m_slots;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type>
class signal<void (arg1_type, arg2_type, arg3_type, arg4_type)>
	: public _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type)>
{
public:
	
	typedef signal<void (arg1_type, arg2_type, arg3_type, arg4_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type, arg3_type, arg4_type)>*
			conn = new _connection<desttype, void (arg1_type, arg2_type, arg3_type
			, arg4_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4);

			it = itNext;
		}
		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4)
	{
		this->call(a1, a2, a3, a4);
	}
protected:
		connections_list m_owner_slots;   
		connections_list m_slots;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type>
class signal<void (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)>
	: public _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)>
{
public:
	
	typedef signal<void (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type, arg3_type
			, arg4_type, arg5_type)>* conn = new _connection<desttype, 
			void (arg1_type, arg2_type, arg3_type, arg4_type, arg5_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5);

			it = itNext;
		}

		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5)
	{
		this->call(a1, a2, a3, a4, a5);
	}
protected:
		connections_list m_owner_slots;   
		connections_list m_slots;
};


template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type>
class signal<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type)>
	: public _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type)>
{
public:

	typedef signal<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type, arg3_type, arg4_type,
			arg5_type, arg6_type)>* conn = 
			new _connection<desttype, void (arg1_type, arg2_type, arg3_type,
			arg4_type, arg5_type, arg6_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5, a6);

			it = itNext;
		}

		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5, a6);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6)
	{
		this->call(a1, a2, a3, a4, a5, a6);
	}
protected:
		connections_list m_owner_slots;   
		connections_list m_slots;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type, class arg7_type>
class signal<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type)>
	: public _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type)>
{
public:
	
	typedef signal<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type, 
		arg7_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type, arg3_type, arg4_type,
			arg5_type, arg6_type, arg7_type)>* conn = 
			new _connection<desttype, void (arg1_type, arg2_type, arg3_type,
			arg4_type, arg5_type, arg6_type, arg7_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5, a6, a7);

			it = itNext;
		}

		
		it = m_slots.begin();
		itEnd = m_slots.end();

				
		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5, a6, a7);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7)
	{
		this->call(a1, a2, a3, a4, a5, a6, a7);
	}
protected:
		connections_list m_owner_slots;   
		connections_list m_slots;
};

template<class arg1_type, class arg2_type, class arg3_type, class arg4_type,
class arg5_type, class arg6_type, class arg7_type, class arg8_type>
class signal<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type, arg8_type)>
	: public _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type, arg8_type)>
{
public:

	typedef signal<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type, arg8_type)> this_type;
	typedef _connection_base<void (arg1_type, arg2_type, arg3_type, arg4_type
	, arg5_type, arg6_type, arg7_type, arg8_type)> function_type;
	typedef std::list<function_type *>  connections_list;
	typedef std::list<this_type *>  signal_list;

	signal()
	{

	}

	virtual ~signal()
	{
		disconnect_all();
	}

	void disconnect_all()
	{
		LOCK();

		for(connections_list::iterator it = m_owner_slots.begin()
			; it != m_owner_slots.end(); ++ it)
		{
			delete (*it);
		}

		m_owner_slots.clear();
		m_slots.clear();
	}

	void connect(function_type& connection)
	{
		LOCK();
		m_slots.push_back(&connection);
	}

	void disconnect(function_type& connection)
	{
		LOCK();
		m_slots.remove(&connection);
	}

	template<class desttype>
	void connect(desttype* pclass, void (desttype::*pmemfun)(arg1_type,
		arg2_type, arg3_type, arg4_type, arg5_type, arg6_type, 
		arg7_type, arg8_type))
	{
		LOCK();
		_connection<desttype, void (arg1_type, arg2_type, arg3_type, arg4_type,
			arg5_type, arg6_type, arg7_type, arg8_type)>* conn = 
			new _connection<desttype, void (arg1_type, arg2_type, arg3_type,
			arg4_type, arg5_type, arg6_type, arg7_type, 
			arg8_type)>(pclass, pmemfun);
		m_owner_slots.push_back(conn);
	}

	void call(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7, arg8_type a8)
	{
		LOCK();
		connections_list::const_iterator itNext, it = m_owner_slots.begin();
		connections_list::const_iterator itEnd = m_owner_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5, a6, a7, a8);

			it = itNext;
		}

		
		it = m_slots.begin();
		itEnd = m_slots.end();

		while(it != itEnd)
		{
			itNext = it;
			++itNext;

			(*it)->call(a1, a2, a3, a4, a5, a6, a7, a8);

			it = itNext;
		}
	}

	void operator()(arg1_type a1, arg2_type a2, arg3_type a3, arg4_type a4,
		arg5_type a5, arg6_type a6, arg7_type a7, arg8_type a8)
	{
		call(a1, a2, a3, a4, a5, a6, a7, a8);
	}
protected:
		connections_list m_owner_slots;   
		connections_list m_slots;
};

#endif // __connection_base_h_

