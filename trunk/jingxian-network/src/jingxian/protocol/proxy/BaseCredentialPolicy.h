
#ifndef _BaseCredentialPolicy_H_
#define _BaseCredentialPolicy_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/protocol/proxy/AbstractCredentialPolicy.h"

_jingxian_begin

namespace proxy
{
	public class BaseCredentialPolicy : AbstractCredentialPolicy
	{
	public:
		BaseCredentialPolicy(const config.Credential& credential, Proxy* server)
			: AbstractCredentialPolicy( credential, server )
		{ 
		}

		virtual ~BaseCredentialPolicy()
		{
		}

		virtual size_t onReceived(ProtocolContext& context)
		{
			///+----+------+----------+------+----------+
			///|VER | ULEN |  UNAME   | PLEN |  PASSWD  |
			///+----+------+----------+------+----------+
			///| 1  |  1   | 1 to 255 |  1   | 1 to 255 |
			///+----+------+----------+------+----------+
			///VER     ��Э�̵ĵ�ǰ�汾��Ŀǰ��0x01
			///ULEN    UNAME�ֶεĳ���
			///UNAME   �û���
			///PLEN    PASSWD�ֶεĳ���
			///PASSWD  ���ע�������Ĵ����
			///

			IOBuffer _buffer = IOBuffer;


			if (3 > _buffer.GetLength() )
				return;

			int len = (int)_buffer[1] + 2;

			if (len > _buffer.GetLength())
				return;

			len += (int)_buffer[ len];

			if (len + 1 > _buffer.GetLength())
				return;

			int version = (int)_buffer.ReadByte();
			int ulen = (int)_buffer.ReadByte();
			string name = Encoding.UTF8.GetString(_buffer.ReadBytes(ulen));
			int plen = (int)_buffer.ReadByte();
			string password = Encoding.UTF8.GetString(_buffer.ReadBytes(plen));

			if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
			{
				sendReply(context, AuthenticationStatus.Error);
				//throw new RuntimeError("{0}����ȷ������Ϊ��!", string.IsNullOrEmpty(name) ? "�û���" : "����");
			}
			else
			{
				sendReply(context, AuthenticationStatus.Success);
			}
		}

		virtual void sendReply(ProtocolContext context, AuthenticationStatus status)
		{
			sendReply(context, (int)status);
		}

		virtual void sendReply(ProtocolContext context, int status)
		{
			///+----+--------+
			///|VER | STATUS |
			///+----+--------+
			///| 1  |   1    |
			///+----+--------+

			byte[] buffer = new byte[2];
			buffer[0] = 5;
			buffer[1] = (byte)status;

			context.Transport.Write(buffer);
			_complete = true;
		}
	};
}

_jingxian_end

#endif // _BaseCredentialPolicy_H_
