
#ifndef _ICredentialPolicy_H_
#define _ICredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/BaseProtocol.h"

_jingxian_begin


namespace AuthenticationStatus
{
	enum Type
	{
		Success = 0x00 // �ɹ�
		,
		Error = 0x01   // ʧ��
		, 
		Pending = 0xFF // ��û�������Ȩ����
	};
}

namespace proxy
{
	class ICredentialPolicy
	{
	public:

		virtual ~ICredentialPolicy(){}

		virtual int authenticationType() = 0;

		virtual size_t onReceived(ProtocolContext& context) = 0;

		/**
		* ��������֤��������ɣ���Ϊ true������Ϊ false
		*/
		virtual bool isComplete() = 0;
	};

	class ICredentials
	{
	public:
		virtual ~ICredentials(){}

		virtual ICredentialPolicy* GetCredential(const std::vector<int>& authTypes) = 0;
	};
}

_jingxian_end

#endif // _ICredentialPolicy_H_