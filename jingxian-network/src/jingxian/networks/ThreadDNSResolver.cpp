
# include "pro_config.h"
# include <Ws2tcpip.h>
# include "jingxian/networks/ThreadDNSResolver.h"
# include "jingxian/IReactorCore.h"
# include "jingxian/threading/thread.h"

_jingxian_begin

ThreadDNSResolver::ThreadDNSResolver()
: core_(null_ptr)
{
}

ThreadDNSResolver::~ThreadDNSResolver(void)
{
}

void ThreadDNSResolver::initialize(IReactorCore* core)
{
	core_ = core;
}

class ResolveErrorTask : public IRunnable
{
public:
	ResolveErrorTask(void* context, const tstring& name, int result, ResolveError onError)
		: context_(context)
		, name_(name)
		, result_(result)
		, onError_(onError)
	{
	}

	virtual ~ResolveErrorTask()
	{
	}

	virtual void run()
	{
		onError_(name_, result_, context_);
	}
private:
	void* context_;
	tstring name_;
	int result_;
	ResolveError onError_;
};

class ResolveCompleteTask : public IRunnable
{
public:
	ResolveCompleteTask(void* context, const tstring& name, ResolveComplete onComplete)
		: context_(context)
		, name_(name)
		, onComplete_(onComplete)
	{
	}

	virtual ~ResolveCompleteTask()
	{
	}

	virtual void run()
	{
		onComplete_(name_, hostEntry_, context_);
	}
	
	IPHostEntry& entry()
	{
		return hostEntry_;
	}
private:
	void* context_;
	tstring name_;
	IPHostEntry hostEntry_;
	ResolveComplete onComplete_;
};

void dnsQuery(const tstring& name
		, void* context
		, IReactorCore* port
		, ResolveComplete onComplete
		, ResolveError onError)
{
	// NOTICE: 因为是在另一个线程中, 在这里调用 port 时可能会因为 port 被析构而发生崩溃.

#ifdef  _UNICODE
	PADDRINFOW res = NULL;
	int result = GetAddrInfoW(name.c_str(), NULL, NULL, &res);
#else
	struct addrinfo* res = NULL;
	int result = getaddrinfo(host.c_str(), NULL, NULL, &res);
#endif
	if(0 != result)
	{
		std::auto_ptr<ResolveErrorTask> ptr(new ResolveErrorTask(context, name, result, onError));
		if(port->send(ptr.get()))
			ptr.release();
		else
			assert(false);
		return ;
	}

	std::auto_ptr<ResolveCompleteTask> ptr(new ResolveCompleteTask(context, name, onComplete));
	ptr->entry().HostName = name;

#ifdef  _UNICODE
	PADDRINFOW next = res;
#else
	struct addrinfo* next = res;
#endif
	while(null_ptr != next)
	{
		if(null_ptr != next->ai_addr && 0 < next->ai_addrlen)
		{
			ptr->entry().AddressList.push_back(HostAddress(next->ai_addr, next->ai_addrlen));
		}

		if(null_ptr != next->ai_canonname)
		{
			ptr->entry().Aliases.push_back(next->ai_canonname);
		}

		next = next->ai_next;
	}

#ifdef  _UNICODE
	FreeAddrInfoW(res);
#else
	freeaddrinfo(res);
#endif
	if(port->send(ptr.get()))
		ptr.release();
	else
		assert(false);
}

void ThreadDNSResolver::ResolveHostByName(const tstring& name
		, void* context
		, ResolveComplete callback
		, ResolveError onError
		, int timeout)
{
	create_thread(&dnsQuery, name, context, core_, callback, onError);
}

_jingxian_end