
# include "pro_config.h"
# include "jingxian/Buffer/OutgoingBuffer.h"

_jingxian_begin

OutgoingBuffer::OutgoingBuffer(size_t capacity)
: capacity_(capacity)
, ptr_(0)
, readPtr_(0)
, length_(0)
, readBytes_(0)
{
	if( capacity_ <= 0)
		capacity_ = 10;

	ptr = (databuffer_t**)malloc(sizeof(databuffer_t*) * capacity_);
	memset(ptr_, 0, sizeof(databuffer_t*) * capacity_));

	readPtr_ = (LPWSABUF)malloc(sizeof(WSABUF) * capacity_);
	memset(readPtr_, 0, sizeof(WSABUF) * capacity_));
}

OutgoingBuffer::~OutgoingBuffer()
{
	for(size_t i = 0; i < length_; ++i)
	{
		free_databuffer(ptr_[i]);
	}
	::free(ptr_);
	::free(readPtr_);

	ptr_ = null_ptr;
	length_ = 0;
	readBytes_ = 0;
}

void OutgoingBuffer::Push(databuffer_t* newbuf)
{
	if( capacity_ <= length_ )
	{
		capacity_ += 10;

		ptr_ = (databuffer_t**)realloc( ptr_,sizeof(databuffer_t*)* capacity_);
		memset( ptr_ + length_, 0, sizeof(databuffer_t*)*(capacity_-length_));

		readPtr_ = (LPWSABUF)realloc( readPtr_,sizeof(WSABUF)*capacity_);
		memset( readPtr_ + length_, 0, sizeof(WSABUF)*(capacity_-length_));
	}

	ptr_[length_] = newbuf;
	writePtr_[writeLength_].buf = newbuf->ptr;
	writePtr_[writeLength_].len = newbuf->capacity;
	writeBytes_ += newbuf->capacity;

	++ length_;
	++ writeLength_;

	assert((writeLength_+readLength_) == length_);
}

databuffer_t* OutgoingBuffer::Pop()
{
	if(length_<=0)
		return null_ptr;

	assert((writeLength_ + readLength_) == length_);

	--length_;
	memmove(ptr_, ptr_ + 1,sizeof(databuffer_t*) * length_);

	if(readLength_ > 0 )
	{
		readBytes_ -= readPtr_[0].len;
		-- readLength_;
		memmove(readPtr_, readPtr_ + 1, sizeof(WSABUF)*readLength_);
		memset( readPtr_ + readLength_, 0, sizeof(WSABUF));
	}
	else if(writeLength_ > 0)
	{
		writeBytes_ -= writePtr_[0].len;
		--writeLength_;
		memmove(writePtr_, writePtr_ + 1, sizeof(WSABUF)*writeLength_);
		memset(writePtr_ + writeLength_, 0, sizeof(WSABUF));
	}

	assert((writeLength_ + readLength_) == length_);
}

LPWSABUF OutgoingBuffer::GetReadBuffer(size_t* len)
{
	if(null_ptr != len)
		*len = readLength_;

	return readPtr_;
}

size_t OutgoingBuffer::ReadBytes(size_t len)
{
	if( readBytes_ <= len )
	{
		memset(readPtr_, 0, sizeof(WSABUF)*readLength_);
		size_t old = readBytes_;
		readBytes_ = 0;
		return old;
	}

	size_t exceptLenght = len;
	int count = 0;
	do
	{
		if(readPtr_[count].len > exceptLenght)
		{
			readPtr_[count].len -= exceptLenght;
			readPtr_[count].buf += exceptLenght;
			exceptLenght = 0;
		}
		else
		{
			readPtr_[count].buf += readPtr_[count].len;
			exceptLenght -= readPtr_[count].len;
			readPtr_[count].len = 0;
		}
	}
	while(exceptLenght > 0);
	
	readBytes_ -= len;
	return len;
}

size_t OutgoingBuffer::TotalReadBytes() const
{
	return readBytes_;
}

LPWSABUF OutgoingBuffer::GetWriteBuffer(size_t* len)
{	
	if(null_ptr != len)
		*len = writeLength_;

	return writePtr_;
}

size_t OutgoingBuffer::WriteBytes(size_t len)
{
	if( writeBytes_ <= len )
	{
		memset(writePtr_, 0, sizeof(WSABUF)*writeLength_);
		size_t old = writeBytes_;
		writeBytes_ = 0;
		return old;
	}

	size_t exceptLenght = len;
	int count = 0;
	do
	{
		if(writePtr_[count].len > exceptLenght)
		{
			writePtr_[count].len -= exceptLenght;
			writePtr_[count].buf += exceptLenght;
			exceptLenght = 0;
		}
		else
		{
			writePtr_[count].buf += writePtr_[count].len;
			exceptLenght -= writePtr_[count].len;
			writePtr_[count].len = 0;
		}
	}
	while(exceptLenght > 0);
	writeBytes_ -= len;
	return len;
}

size_t OutgoingBuffer::TotalWriteBytes() const
{
	return writeBytes_;
}

_jingxian_end
