
#include "jingxian/networks/TCPAcceptor.h"

_jingxian_begin

TCPAcceptor::TCPAcceptor()
{
	
}
	
TCPAcceptor::~TCPAcceptor()
{
		
}

time_t TCPAcceptor::timeout () const
{
	
}

const IEndpoint& TCPAcceptor::bindPoint() const;

void TCPAcceptor::stopListening();

bool TCPAcceptor::startListening();

IProtocolFactory& TCPAcceptor::protocolFactory();

IDictionary& TCPAcceptor::misc();

const IDictionary& TCPAcceptor::misc() const;

const tstring& TCPAcceptor::toString() const;
	
TCPAcceptorFactory::~IAcceptorFactory()
{
	
}

	/**
	* @implements createAcceptor
	 */
	virtual IAcceptor* TCPAcceptorFactory::createAcceptor(const tchar* endPoint);

	/**
	* @implements toString
	 */
	virtual const tstring& TCPAcceptorFactory::toString() const;
	
private:
	NO_COPY(TCPAcceptor );
	
	TCPEndpoint _endpoint;
	
	tstring _toString;	
};

_jingxian_end

#endif //_TCPAcceptor_H_ 