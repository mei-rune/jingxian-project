
#ifndef tcp_client_h
#define tcp_client_h

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/networks/sockets/BaseSocket.h"

_jingxian_begin

class TCPClient
{
public:
  TCPClient (void);

  ~TCPClient (void);

  /**
   * �Ƿ���Զ�����
   */
  bool readable();

  /**
   * �Ƿ����д����
   */
  bool writable();

  /**
   * ����socketΪ�����������
   */
  void blocking(bool val);

  /**
   * ȡ�ñ�socket���������Ƿ�����
   */
  bool blocking() const;
  
  /**
   * ȡ�ñ�����ַ
   */
  const NetAddress& local_addr () const;

  /**
   * ȡ��Զ�̻���ַ
   */
  const tstring& remote_addr () const;

  /**
   * ȡ��socket����
   */
  BaseSocket& socket();
	
  /**
   * ȡ��socket����
   */
  const BaseSocket& socket() const;

  /**
   * ��r�뱾�����ֵ����
   */
  void swap( TCPClient& r );

  /**
   * socket�Ƿ���Ч
   */
  bool is_good() const;

  /**
   * ������
   * @param[ in,out ] buf ���ݻ�����
   * @param[ in ] n ���ݻ�������С
   * @param[ in ] flags ���ڿ��Ʊ���������Ϊ
   * @return �ɹ����ض�ȡ���ֽ������������󷵻�SOCKET_ERROR
   */
  ssize_t recv ( void *buf, size_t n, int flags = 0 );

  /**
   * ��������
   * @param[ in,out ] buf ���ݻ�����
   * @param[ in ] n ���ݻ�������С
   * @param[ in ] flags ���ڿ��Ʊ���������Ϊ
   * @return �ɹ����ط��͵��ֽ������������󷵻�SOCKET_ERROR
   */
  ssize_t send (const void *buf, size_t n, int flags = 0 );

  /**
   * ������
   * @param[ in,out ] iov ���ݻ���������
   * @param[ in ] n ���ݻ��������鳤��
   * @return �ɹ����ض�ȡ���ֽ������������󷵻�SOCKET_ERROR
   */
  ssize_t recvv (iovec* iov, size_t n );

  /**
   * ��������
   * @param[ in ] iov ���ݻ���������
   * @param[ in ] n ���ݻ��������鳤��
   * @return �ɹ����ط��͵��ֽ������������󷵻�SOCKET_ERROR
   */
  ssize_t sendv (const iovec* iov, size_t n );

  /**
   * ������,��recv��ͬ�����������ڶ���lengthָ���ĳ��Ⱥ�ŷ���
   * @param[ in,out ] buf ���ݻ�����
   * @param[ in ] n ���ݻ�������С
   */
  bool recv_n( char* buf, size_t length);
  
  /**
   * ��������,��send��ͬ�����������ڶ���lengthָ���ĳ��Ⱥ�ŷ���
   * @param[ in,out ] buf ���ݻ�����
   * @param[ in ] n ���ݻ�������С
   */
  bool send_n( const char* buf, size_t length);

  /**
   * ������,��recvv��ͬ�����������ڶ���lengthָ���ĳ��Ⱥ�ŷ���
   * @param[ in,out ] iov ���ݻ���������
   * @param[ in ] n ���ݻ��������鳤��
   */
  bool recvv_n( iovec* wsaBuf, size_t size);

  /**
   * ��������,��sendv��ͬ�����������ڶ���lengthָ���ĳ��Ⱥ�ŷ���
   * @param[ in ] iov ���ݻ���������
   * @param[ in ] n ���ݻ��������鳤��
   */
  bool sendv_n( const iovec* wsaBuf, size_t size);

  /**
   * �첽��ʽ��ȡ����
   * @param[ in,out ] buf ���ݻ�����
   * @param[ in ] n ���ݻ�������С
   * @param[ in ] overlapped �ص�IO����ָ��
   */
  bool recv (void *buf, size_t n,
                OVERLAPPED& overlapped);
  /**
   * �첽��ʽ��������
   * @param[ in,out ] buf ���ݻ�����
   * @param[ in ] n ���ݻ�������С
   * @param[ in ] overlapped �ص�IO����ָ��
   */
  bool send (const void *buf, size_t n,
                OVERLAPPED& overlapped);

  /**
   * �첽��ʽ��ȡ����
   * @param[ in,out ] iov ���ݻ���������
   * @param[ in ] n ���ݻ��������鳤��
   * @param[ in ] overlapped �ص�IO����ָ��
   */
  bool recvv (iovec* iov, size_t n,
                OVERLAPPED& overlapped);

  /**
   * �첽��ʽ��������
   * @param[ in ] iov ���ݻ���������
   * @param[ in ] n ���ݻ��������鳤��
   * @param[ in ] overlapped �ص�IO����ָ��
   */
  bool sendv (const iovec* iov, size_t n,
                 OVERLAPPED& overlapped);

  /**
   * ����һ���ļ�
   */
  bool transmit (const iopack* iov, size_t n );

  /**
   * ����һ���ļ�
   */
  bool transmit (const iopack* iov, size_t n
                , OVERLAPPED& overlapped);

  /**
   * ����һ���ļ�
   */
  bool transmit ( HANDLE hFile
				, size_t nNumberOfBytesToWrite
				, size_t nNumberOfBytesPerSend
				, io_file_buf* transmitBuffers );
  /**
   * ����һ���ļ�
   */
  bool transmit ( HANDLE hFile
				, size_t nNumberOfBytesToWrite
				, size_t nNumberOfBytesPerSend
				, io_file_buf* transmitBuffers
                , OVERLAPPED& overlapped);

  /**
   * ���ӵ�Զ������
   */
  bool connect( const NetAddress& addr );

  /**
   * �첽���ӵ�Զ������
   */
  bool connect( const NetAddress& addr, OVERLAPPED& overlapped );

  /**
   * �첽���ӵ�Զ�����������������ݣ������� ATM ��
   */
  bool TCPClient::connect( const NetAddress& addr
									  , const void* send_buffer
									  , size_t send_data_len
									  , OVERLAPPED& overlapped );

private:
  NOCOPY( TCPClient );
  NetAddress local_addr_;
  NetAddress remote_addr_;
  bool blocking_;
  BaseSocket socket_;
};

inline tostream& operator<<( tostream& target, const TCPClient& client )
{
  if( client.is_good() )
  {
	  target << _T("TCPClient[ ")
		  <<  client.socket().handle() 
		  << _T(":") << client.local_addr()
		  << _T("<==>")<< client.remote_addr()
		  << _T("|blocking=") << client.blocking()
		  << _T("]" );
  }
  else
  {
	  target << _T("TCPClient[ disconnect ]" );
  }
  
  return target;
}

#if defined (OS_HAS_INLINED)
#include "jingxian/networks/sockets/TCPClient.inl"
#endif

_jingxian_end

#endif /* tcp_client_h */