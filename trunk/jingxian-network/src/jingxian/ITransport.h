
#ifndef _transport_h_
#define _transport_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "buffer.h"
# include "exception.hpp"

_jingxian_begin

class IProtocol;

class ITransport
{
public:
	    virtual ~ITransport(){}

		/**
         * ָ���� @see{protocol} �ӿ������ն���������
         */
        virtual void bindProtocol(IProtocol* protocol) = 0;

        /**
         * ��ʼ������
         */
        virtual void startReading() = 0;

        /**
         * ֹͣ������
         */
        virtual void stopReading() = 0;

        /**
         * �������ݣ�ע�������첽��  )
         * @param[ in ] handle  �첽�ص��ӿ�,�����ݷ��Ͳ��������ܳɹ���ʧ�ܣ�������
         *				���óɹ�������ص�handle��onWrite�ӿ�
         * @param[ in ] buffer �����͵����ݿ�
         * @param[ in ] IOBuffer �����͵����ݿ�
         * @param[ in ] IOBuffers ��������͵����ݿ�
         * @param[ in ] offest ����ƫ��
         * @param[ in ] length ���ݳ���
         */
        virtual void write(char* buffer) = 0;
        virtual void write(char* buffer, int offest, int length) = 0;
        virtual void write(Buffer& buffer) = 0;

        /**
         * �ر�����
         */
        virtual void disconnection() = 0;
		
		/**
         * �ر�����
         */
        virtual void disconnection(const tstring& error) = 0;

        /**
         * Դ��ַ
         */
        virtual const tstring& host() const = 0;

        /**
         * Ŀ���ַ
         */
        virtual const tstring& peer() const = 0;

        /**
         * ���� @see{protocol} ��onTimeout�¼��ĳ�ʱʱ��
         */
        virtual time_t timeout() const = 0;

		/**
		 * ȡ�õ�ַ������
		 */
		virtual const tstring& toString() const = 0;
};

inline tostream& operator<<( tostream& target, const ITransport& transport )
{
	target << transport.toString();
	return target;
}


class ErrorCode
{
public:
	ErrorCode( const tchar* err)
		: isSuccess_(false)
		, code_( 0 )
		, err_( err )
	{}

	ErrorCode( int success, int code)
		: isSuccess_(success)
		, code_(code)
		, err_(lastError(code))
	{
	}

	ErrorCode( int success, int code, const tstring& err)
		: isSuccess_(success)
		, code_(code)
		, err_(err)
	{
	}

	ErrorCode( int success, int code, const tchar* err)
		: isSuccess_(success)
		, code_(code)
		, err_(err)
	{
	}

	virtual ~ErrorCode() { }

	bool isSuccess() const { return isSuccess_; }
	int getCode() const { return code_; }
	const tstring& toSting() const { return err_; }

private:
	bool isSuccess_;
	int code_;
	tstring err_;
};

typedef void (*OnBuildConnectionSuccess)( ITransport* transport, void* context);
typedef void (*OnBuildConnectionError)( const ErrorCode& exception,  void* context);

_jingxian_end

#endif //_transport_h_