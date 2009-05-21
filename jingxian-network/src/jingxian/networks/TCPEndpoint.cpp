
# include "pro_config.h"
# include "jingxian/networks/TCPEndpoint.h"

_jingxian_begin

const tstring TCPEndpoint::_protocol=_T("tcp");

TCPEndpoint::TCPEndpoint(const tchar* endpoint)
: _address( endpoint )
, toString_(_protocol + _T("://"))
{
	toString_ += _address.toString();
}

TCPEndpoint::~TCPEndpoint()
{
}

const tstring& TCPEndpoint::protocol() const
{
	return _protocol;
}

const void* TCPEndpoint::addr() const
{
	return _address.addr();
}

size_t TCPEndpoint::size (void) const
{
	return _address.size();
}

int TCPEndpoint::compareTo(const IEndpoint& endpoint) const
{
	int result = _protocol.compare( endpoint.protocol() );
	return ( 0 != result )? result :
		memcmp( this->addr(), endpoint.addr(), this->size() );
}

const tstring& TCPEndpoint::toString() const
{
	return toString_;
}

_jingxian_end
