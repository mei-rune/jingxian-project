
#ifndef _CreateProcessCommand_H_
#define _CreateProcessCommand_H_

# include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/commands/ICommand.h"
# include "jingxian/networks/IOCPServer.h"

_jingxian_begin

class CreateProcessCommand : public ICommand
{
public:
    CreateProcessCommand(IOCPServer* core
                   , const tstring& fileName
				   , const tstring& cmdLine
                   , OnBuildConnectionComplete onComplete
                   , OnBuildConnectionError onError
                   , void* context);

    virtual ~CreateProcessCommand();

    virtual void on_complete(size_t bytes_transferred
                             , bool success
                             , void *completion_key
                             , errcode_t error);

    virtual bool execute();

    void onResolveComplete(const tstring& name, const tstring& port, const IPHostEntry& hostEntry);
    void onResolveError(const tstring& name, const tstring& port, errcode_t err);

private:
    NOCOPY(CreateProcessCommand);

    void dnsQuery(const tchar* name, const tchar* port);
    bool execute(const struct sockaddr* addr, int len);

    IOCPServer* core_;
    OnBuildConnectionComplete onComplete_;
    OnBuildConnectionError onError_;
    void* context_;

	tstring fileName_;
	tstring commandLine_;
};

_jingxian_end

#endif //_CreateProcessCommand_H_
