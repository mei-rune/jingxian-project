
# include "pro_config.h"
# include "jingxian/networks/commands/AcceptCommand.h"

_jingxian_begin

AcceptCommand::AcceptCommand(TCPAcceptor* acceptor)
: acceptor_(acceptor)
, socket_(acceptor_->createSocket())
, ptr_(malloc(1024))
, len_(1024)
{
}

AcceptCommand::~AcceptCommand()
{
	free( ptr_ );
	ptr_ = null_ptr;
	acceptor_->releaseSocket(_socket, false);
}

void AcceptCommand::on_complete(size_t bytes_transferred
								, int success
								, void *completion_key
								, u_int32_t error)
{
	acceptor_->on_complete(bytes_transferred
								, success
								, completion_key
								, error);
}

bool AcceptCommand::execute()
{
	int bytesTransferred;
	if (base_socket::__acceptex(acceptor_->handle()
		, socket_
		, ptr_
		, 0 //_byteBuffer.Space - (HazelAddress.MaxSize + 4) * 2 
		//����Ϊ0,������д��������Ӵ���accept�У���Ϊ�ͻ���ֻ
		//�������ӣ�û�з������ݡ�
		, sizeof(sockaddr) + 4
		, sizeof(sockaddr) + 4
		, &bytesTransferred
		, this ))
		return true;

	if (WSA_IO_PENDING == ::WSAGetLastError())
		return true;

	return false;
}

_jingxian_end
