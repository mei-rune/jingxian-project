
#ifndef _proxy_config_Credential_H_
#define _proxy_config_Credential_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"

_jingxian_begin

namespace proxy
{
namespace config
{
struct Credential
{
    int AuthenticationType;
    tstring Name;
    tstring Description;
};
}
}

_jingxian_end

#endif // _proxy_config_Credential_H_
