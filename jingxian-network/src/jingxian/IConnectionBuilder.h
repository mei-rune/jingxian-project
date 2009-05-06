
#ifndef CONNECTOR_H
#define CONNECTOR_H

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.hpp"
# include "Exception.hpp"

_jingxian_begin

class IEndpoint;

typedef IProtocol* (*BuildProtocol)( ITransport* transport, void* context);
typedef void (*OnConnectError)( const Exception& exception, void* context);

class IConnectionBuilder
{
public:

	virtual ~IConnectionBuilder() {};
    /**
     * ����һ������
     * 
     * @param[ in ] endPoint ����Ŀ���ַ
     * @param[ in ] buildProtocol �����ӳɹ�ʱ�����ñ�ί�д���һ��������
     * @param[ in ] throwError  �����ӷ���������ʼ����������������ʱ��
	 * �ñ�ί�У������쳣������������ConnectError��InitializeError
     * @param[ in ] context �������ӵ�������
     * @exception ConnectError �����ӷ�������ʱ�����ô�����ί��ʱ������</exception>
     * @exception InitializeError ��ʼ����������������ʱ�����ô�����ί��ʱ������</exception>
	 */
    virtual void connect(const tchar* endPoint
                       , BuildProtocol buildProtocol
                       , OnConnectError onConnectError
                       , void* context ) = 0;

	/**
	 * ȡ�õ�ַ������
	 */
	virtual const tstring& toString() const = 0;
};

inline tostream& operator<<( tostream& target, const IConnectionBuilder& connectionBuilder )
{
	target << connectionBuilder.toString();
	return target;
}


_jingxian_end

#endif // CONNECTOR_H