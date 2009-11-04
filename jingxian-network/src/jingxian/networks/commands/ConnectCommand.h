
#ifndef _ConnectCommand_H_
#define _ConnectCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/IOCPServer.h"

_jingxian_begin

class ConnectCommand : public ICommand
{
public:
    ConnectCommand(IOCPServer* core
                   , const tchar* host
                   , OnBuildConnectionComplete onComplete
                   , OnBuildConnectionError onError
                   , void* context);

    virtual ~ConnectCommand();

    virtual void on_complete(size_t bytes_transferred
                             , bool success
                             , void *completion_key
                             , errcode_t error);

    virtual bool execute();

    void onResolveComplete(const tstring& name, const tstring& port, const IPHostEntry& hostEntry);
    void onResolveError(const tstring& name, const tstring& port, errcode_t err);

private:
    NOCOPY(ConnectCommand);

    void dnsQuery(const tchar* name, const tchar* port);
    bool execute(const struct sockaddr* addr, int len);

    IOCPServer* core_;
    OnBuildConnectionComplete onComplete_;
    OnBuildConnectionError onError_;
    void* context_;

    tstring host_;
    SOCKET socket_;
};

_jingxian_end

#endif //_ConnectCommand_H_
