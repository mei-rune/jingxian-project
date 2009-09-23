
#ifndef _SOCKSv5Incoming_H_
#define _SOCKSv5Incoming_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"
# include "jingxian/buffer/OutBuffer.h"

_jingxian_begin

namespace proxy
{
	class SOCKSv5Protocol;

	class SOCKSv5Incoming : public BaseProtocol
	{
	public:
		SOCKSv5Incoming();

		void initialize(SOCKSv5Protocol* socks);

		void write(const std::vector<io_mem_buf>& bufs);

		virtual size_t onReceived(ProtocolContext& context);

		virtual void onConnected(ProtocolContext& context);

		virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason);
	private:
		SOCKSv5Protocol* socks_;
		ITransport* transport_;
	};

}

_jingxian_end

#endif //_SOCKSv5Incoming_H_