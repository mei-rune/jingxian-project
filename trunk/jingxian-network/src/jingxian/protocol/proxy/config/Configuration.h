
#ifndef _proxy_config_Configuration_H_
#define _proxy_config_Configuration_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/config/Credential.h"

_jingxian_begin

namespace proxy
{
namespace config
{
struct Configuration
{
    std::vector<Credential> Credentials;
    std::vector<tstring> AllowedIPs;
    std::vector<tstring> BlockingIPs;
};
}
}

_jingxian_end

#endif // _proxy_config_Configuration_H_
