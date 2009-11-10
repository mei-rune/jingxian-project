
#ifndef _ListenPort_H_
#define _ListenPort_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/IReactorCore.h"

_jingxian_begin

class ListenPort
{
public:
    ListenPort(IReactorCore* core, IProtocolFactory* protocolFactory, IAcceptor* acceptor);

    virtual ~ListenPort();

    bool start();

    void stop();

    void onComplete(ITransport* transport, IReactorCore* core);

    void onError(const ErrorCode& err, IReactorCore* core);

    bool isPending() const;

    virtual const tstring& toString() const;

private:
    NOCOPY(ListenPort);
    IReactorCore* reactor_;
    IProtocolFactory* protocolFactory_;
    Acceptor acceptor_;
    int errorCount_;
    bool isPending_;
	logging::logger logger_;
    tstring toString_;
};

inline tostream& operator<<(tostream& target, const ListenPort& server)
{
    target << server.toString();
    return target;
}

inline tostream& operator<<(tostream& target, const ListenPort* server)
{
    if (null_ptr == server)
    {
        target << _T("<null>");
        return target;
    }

    target << *server;
    return target;
}

_jingxian_end

#endif //_ListenPort_H_