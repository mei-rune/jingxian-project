
#ifndef _IDNSResolver_H_
#define _IDNSResolver_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <vector>
# include <winsock2.h>


_jingxian_begin

class HostAddress
{
public:
    HostAddress()
            : addrlen_(0)
    {
    }

    HostAddress(const struct sockaddr* addr
                , size_t addrlen)
            : addrlen_(0)
    {
        copyFrom(addr, addrlen);
    }

    HostAddress(const HostAddress& addr)
            : addrlen_(0)
    {
        copyFrom(addr);
    }

    HostAddress& operator=(const HostAddress& addr)
    {
        copyFrom(addr);
        return *this;
    }

    ~HostAddress()
    {
    }

    void copyFrom(const struct sockaddr* addr
                  , size_t addrlen)
    {
        if (0 == addrlen)
        {
            addrlen_ = 0;
            return;
        }

        assert(addrlen <= sizeof(SOCKADDR_STORAGE));
        memcpy(&addr_, addr, addrlen);
        addrlen_ = addrlen;
    }

    void copyFrom(const HostAddress& addr)
    {
        this->copyFrom(addr.ptr(), addr.len());
    }

    struct sockaddr* ptr()
    {
        return (struct sockaddr*)&addr_;
    }

    const struct sockaddr* ptr() const
    {
        return (const struct sockaddr*)&addr_;
    }

    size_t len() const
    {
        return addrlen_;
    }

private:
    SOCKADDR_STORAGE addr_;
    size_t addrlen_;
};

struct IPHostEntry
{
    //获取或设置与主机关联的 IP 地址列表。
    std::vector<HostAddress> AddressList;
    //获取或设置与主机关联的别名列表。
    std::vector<tstring> Aliases;
    //获取或设置主机的 DNS 名称。
    tstring HostName;
};

typedef void (*ResolveComplete)(const tstring& name, const tstring& port, const IPHostEntry& hostEntry, void* context);
typedef void (*ResolveError)(const tstring& name, const tstring& port, errcode_t err, void* context);

class IDNSResolver
{
public:

    virtual ~IDNSResolver() {}

    /**
     * 开始异步请求关于指定 DNS 主机名的 IPHostEntry 信息。
     */
    virtual void ResolveHostByName(const tchar* name
                                   , const tchar* port
                                   , void* context
                                   , ResolveComplete callback
                                   , ResolveError onError
                                   , int timeout) = 0;
};



_jingxian_end

#endif // _IDNSResolver_H_
