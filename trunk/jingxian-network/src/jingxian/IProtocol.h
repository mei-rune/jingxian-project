
#ifndef _IProtocol_H_
#define _IProtocol_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/exception.hpp"
# include "jingxian/Buffer/IBuffer.H"


_jingxian_begin

class ProtocolContext;

class IProtocol
{
public:

	virtual ~IProtocol(){}

    /**
     * ��ָ����ʱ�����û���յ��κ�����
     * 
     * @param[ in ] context �Ự��������
	 */
    virtual void onTimeout(ProtocolContext& context) = 0;

    /**
     * ���Ự�����󣬱����á�
     * 
     * @param[ in ] context �Ự��������
	 */
    virtual void onConnected(ProtocolContext& context) = 0;

    /**
     * ���Ự�رպ󣬱����á�
     * 
     * @param[ in ] context �Ự��������
     * @param[ in ] errCode �رյ�ԭ��,Ϊ0�Ǳ�ʾ�����ر�
     * @param[ in ] reason �رյ�ԭ������
	 */
    virtual void onDisconnected(ProtocolContext& context, errcode_t errCode, const tstring& reason) = 0;

    /**
     * �����µ���Ϣ����ʱ�������á�
     * 
     * @param[ in ] context �Ự��������
     * @param[ in ] buffer �����µ�����Ϣ�Ļ�����
	 */
    virtual void onReceived(ProtocolContext& context) = 0;

    /**
     * �����´�������ȡ���ݵĻ�����
     * 
     * @param[ in ] context �Ự��������
     * @param[ in ] lastBuffer ��һ��������ȡ���ݵĻ�����
     * @param[ in ] len ��һ��������ȡ���ݵĻ������Ĵ�С
	 */
	virtual buffer_chain_t* createBuffer(const ProtocolContext& context, buffer_chain_t* lastBuffer, size_t len) = 0;

	/**
	* ȡ�õ�ַ������
	*/
	virtual const tstring& toString() const = 0;
};

class IProtocolFactory
{
public:

	virtual ~IProtocolFactory(){}

	/**
	 * ���� IProtocol ����
	 */
	virtual IProtocol* createProtocol() = 0;

	/**
	* ȡ�õ�ַ������
	*/
	virtual const tstring& toString() const = 0;
};

inline tostream& operator<<( tostream& target, const IProtocolFactory& protocolFactory )
{
	target << protocolFactory.toString();
	return target;
}

inline tostream& operator<<( tostream& target, const IProtocol& protocol )
{
	target << protocol.toString();
	return target;
}

_jingxian_end

#endif // _IProtocol_H_