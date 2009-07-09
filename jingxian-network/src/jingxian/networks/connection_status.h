

#ifndef _CONNECTION_STATUS_H_
#define _CONNECTION_STATUS_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

_jingxian_begin

namespace connection_status
{
	/**
	* ���Ӷ����״̬
	*/
	enum type
	{
		listening,
		connecting,
		connected,
		disconnecting,
		disconnected
	};
}

_jingxian_end

#endif // _CONNECTION_STATUS_H_