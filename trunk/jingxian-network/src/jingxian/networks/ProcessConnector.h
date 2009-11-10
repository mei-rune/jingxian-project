
#ifndef _ProcessConnector_H_
#define _ProcessConnector_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/networking.h"
# include "jingxian/networks/IOCPServer.h"


_jingxian_begin

class ProcessConnector : public IConnectionBuilder
{
public:

    ProcessConnector(IOCPServer* core);

    /**
     * @implements ~ProcessConnector
     */
    virtual ~ProcessConnector();

    /**
     * @implements connect
     */
    virtual void connect(const tchar* endPoint
                         , OnBuildConnectionComplete onComplete
                         , OnBuildConnectionError onError
                         , void* context);

    /**
     * @implements toString
     */
    virtual const tstring& toString() const;

private:
    NOCOPY(ProcessConnector);

    friend class ConnectCommand;

    IOCPServer* nextCore()
    {
        return core_;
    }

    logging::logger& log()
    {
        return logger_;
    }

    IOCPServer* core_;
	logging::logger logger_;
    tstring toString_;
};

_jingxian_end

#endif //_ProcessConnector_H_ 