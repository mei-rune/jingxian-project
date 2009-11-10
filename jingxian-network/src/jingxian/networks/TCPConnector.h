
#ifndef _TCPConnector_H_
#define _TCPConnector_H_

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

class TCPConnector : public IConnectionBuilder
{
public:

    TCPConnector(IOCPServer* core);

    /**
     * @implements ~TCPConnector
     */
    virtual ~TCPConnector();

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
    NOCOPY(TCPConnector);

    friend class ConnectCommand;

    IOCPServer* nextCore()
    {
        return core_;
    }
   

    IOCPServer* core_;
    logging::logger logger_;
    tstring toString_;
};

_jingxian_end

#endif //_TCPConnector_H_ 