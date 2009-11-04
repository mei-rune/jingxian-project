
#ifndef _ICommand_H
#define _ICommand_H

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

_jingxian_begin

class ICommand : public OVERLAPPED
{
public:

    ICommand()
            : handle_(INVALID_HANDLE_VALUE)
    {
        Internal =  0;
        InternalHigh =  0;
        Offset =  0;
        OffsetHigh =  0;
        hEvent = 0;
    }

    virtual ~ICommand(void) {}

    HANDLE handle() const
    {
        return handle_;
    }

    virtual bool execute() = 0;

    virtual void on_complete(size_t bytes_transferred,
                             bool success,
                             void *completion_key,
                             errcode_t error = 0) = 0;

protected:
    HANDLE handle_;
};

_jingxian_end

#endif //_ICommand_H