
#ifndef _IOCPServer_H_
#define _IOCPServer_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <hash_map>
# include "jingxian/string/string.hpp"
# include "jingxian/logging/logging.hpp"
# include "jingxian/IReactorCore.h"
# include "jingxian/networks/proactor.h"

_jingxian_begin

class IOCPServer : public IReactorCore
{
public:
	IOCPServer(void);

	virtual ~IOCPServer(void);


    /**
	 * ����һ��������
	 */
    virtual void connectWith(const tchar* endPoint
                            , BuildProtocol buildProtocol
                            , OnConnectError onConnectError
                            , void* context );
	
    /**
	 * ����һ����������
	 */
    virtual IAcceptor* listenWith(const tchar* endPoint
			, IProtocolFactory* protocolFactory);

	
    /**
     * ��ִ�з������͵��̵߳ȴ�����,�Ժ�ִ��
     *
     * @param[ in ] run ִ�з���
	 */
    virtual bool send( IRunnable* runnable );

	/**
	 * ��ʼ����ֱ������Interrupt�ŷ���
	 */
	virtual void runForever();

	/**
	 * ֹͣ����
	 */
	virtual void interrupt();

	/**
	 * ������󶨵����˿�
	 */
	virtual bool bind(HANDLE systemHandler, void* completion_key);
	
	/**
	 *  ����ʱִ�еĻص�������������Լ̳б����� 
	 */
	virtual void onIdle();

	/**
	* ȡ�õ�ַ������
	*/
	virtual const tstring& toString() const;

private:
	
	NOCOPY(IOCPServer);

	proactor _proactor;

	u_int32_t _timeout;

	bool _isRunning;

	stdext::hash_map<tstring, IConnectionBuilder* > _connectionBuilders;

	stdext::hash_map<tstring, IAcceptorFactory* > _acceptorFactories;
	
	stdext::hash_map<tstring, IAcceptor* > _acceptors;
	
	ILogger* _logger;

	tstring toString_;
};

_jingxian_end

#endif //_IOCPServer_H_