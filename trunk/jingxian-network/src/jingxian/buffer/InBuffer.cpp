# include "pro_config.h"
# include "jingxian/Buffer/InBuffer.h"
# ifdef _GOOGLETEST_
#include <gtest/gtest.h>
#endif

_jingxian_begin

InBuffer::InBuffer(const std::vector<io_mem_buf>* buf, size_t totalLength)
        : memory_(null_ptr)
        , totalLength_(0)
        , readLength_(0)
        , current_(0)
        , currentPtr_(null_ptr)
        , currentLength_(0)
{
    reset(buf, totalLength);
}

//InBuffer::InBuffer(LPWSABUF ptr, size_t count, size_t totalLength)
//: totalLength_(totalLength)
//, readLength_(0)
//, current_(0)
//, currentPtr_(null_ptr)
//, currentLength_(0)
//{
//  if(is_null(ptr))
//    ThrowException1(ArgumentNullException, "ptr");
//  if(0 == count)
//  {
//    totalLength_ = 0;
//    return;
//  }
//
//  for(size_t i =0; i < count; ++i)
//  {
//    memory_.push_back(ptr[i]);
//  }
//
//  currentPtr_ = memory_[current_].buf;
//  currentLength_ = memory_[current_].len;
//}

InBuffer::~InBuffer(void)
{
}

void InBuffer::reset(const std::vector<io_mem_buf>* iobuf, size_t totalLength)
{
    if (is_null(iobuf))
        ThrowException1(ArgumentNullException, _T("iobuf"));

    memory_ = iobuf;

    if (-1 == totalLength)
    {
        totalLength = 0;

        for (std::vector<io_mem_buf>::const_iterator it = iobuf->begin()
                ; it != iobuf->end(); ++ it)
        {
            totalLength += it->len;
        }
    }

    totalLength_ = totalLength;
    readLength_ = 0;
    current_ = 0;

    currentPtr_ = (*memory_)[current_].buf;
    currentLength_ = (*memory_)[current_].len;

    transcationDatas_.clear();
}

int InBuffer::beginTranscation()
{
    TranscationData data;
    data.current_ = this->current_;
    data.currentLength_ = this->currentLength_;
    data.currentPtr_ = this->currentPtr_;
    data.readLength_ = this->readLength_;
    data.exceptionStyle_ = this->exceptionStyle_;
    data.errno_ = this->errno_;

    transcationDatas_.push_back(data);
    return (int)transcationDatas_.size();
}

void InBuffer::rollbackTranscation(int id)
{
    if (id <= 0 || id > (int)transcationDatas_.size())
        ThrowException(OutOfRangeException);

    TranscationData& data = transcationDatas_[id-1];

    this->current_ = data.current_;
    this->currentLength_ = data.currentLength_;
    this->currentPtr_ = data.currentPtr_;
    this->readLength_ = data.readLength_;
    this->exceptionStyle_ = data.exceptionStyle_;
    this->errno_ = data.errno_;

    transcationDatas_.resize(id - 1);
}

void InBuffer::commitTranscation(int id)
{
    size_t index = id;
    if (index <= 0 || index > transcationDatas_.size())
        ThrowException(OutOfRangeException);

    for (size_t i = transcationDatas_.size(); i >= index; --i)
    {
        TranscationData& data = transcationDatas_[i-1];

        this->current_ = data.current_;
        this->currentLength_ = data.currentLength_;
        this->currentPtr_ = data.currentPtr_;
        this->readLength_ = data.readLength_;
        this->exceptionStyle_ = data.exceptionStyle_;
        this->errno_ = data.errno_;
    }

    transcationDatas_.resize(index - 1);
}

bool InBuffer::readBoolean()
{
    return 0 != readInt8();
}

int8_t InBuffer::readInt8()
{
    int8_t value = 0;
    readBlob((char*)&value, sizeof(value));
    return value;
}

int16_t InBuffer::readInt16()
{
    int16_t value = 0;
    readBlob((char*)&value, sizeof(value));
    return value;
}

int32_t InBuffer::readInt32()
{
    int32_t value = 0;
    readBlob((char*)&value, sizeof(value));
    return value;
}

int64_t InBuffer::readInt64()
{
    int64_t value = 0;
    readBlob((char*)&value, sizeof(value));
    return value;
}

void InBuffer::readBlob(void* blob, size_t len)
{
    if (ERROR_SUCCESS != this->error())
        return ;

    if (0 >= len)
        return ;

    if (currentLength_ >= len)
    {
        memcpy(blob, currentPtr_, len);
        currentPtr_ += len;
        currentLength_ -= len;
        readLength_ += len;
        return ;
    }

    if (size() < len)
    {
        this->error(ERROR_HANDLE_EOF);
        return;
    }

    char* ptr = (char*)blob;
    size_t count = len;
    do
    {
        if (currentLength_ > count)
        {
            memcpy(ptr, currentPtr_, count);
            currentPtr_ += count;
            currentLength_ -= count;
            break;
        }

        memcpy(ptr, currentPtr_, currentLength_);
        count -= currentLength_;
        ptr += currentLength_;

        if ((*memory_).size() <= ++current_)
        {
            currentPtr_ = null_ptr;
            currentLength_ = 0;
            break;
        }
        else
        {
            currentPtr_ = (*memory_)[current_].buf;
            currentLength_ = (*memory_)[current_].len;
        }
    }
    while (0 < count);
    readLength_ += len;
}

void InBuffer::seek(int offestLen)
{
    if (0 == offestLen)
        return;

    if (0 < offestLen)
    {
        size_t offest = offestLen;

        if (size() <= offest)
        {
            currentPtr_ = null_ptr;
            currentLength_ = 0;
            current_ = (*memory_).size();
            readLength_ = totalLength_;
            return;
        }

        size_t len = offest - currentLength_;
        for (size_t i = current_ + 1; i < (*memory_).size(); ++i)
        {
            if (len < (*memory_)[i].len)
            {
                current_ = i;
                currentPtr_ = (*memory_)[i].buf + len;
                currentLength_ = (*memory_)[i].len - len;
                readLength_ += offest;
                return;
            }
            len += (*memory_)[i].len;
        }
        currentPtr_ = null_ptr;
        currentLength_ = 0;
        current_ = (*memory_).size();
        readLength_ = totalLength_;
        return ;
    }

    size_t offest = ::abs(offestLen);
    if (readLength_ <= offest)
    {
        current_ = 0;
        readLength_ = 0;

        if ((*memory_).empty())
        {
            currentPtr_ = null_ptr;
            currentLength_ = 0;
        }
        else
        {
            currentPtr_ = (*memory_)[current_].buf;
            currentLength_ = (*memory_)[current_].len;
        }
        return;
    }

    size_t len = offest - ((*memory_)[current_].len - currentLength_);
    for (size_t i = current_ - 1; i >= 0 ; --i)
    {
        if (len < (*memory_)[i].len)
        {
            current_ = i;
            currentPtr_ = (*memory_)[i].buf + ((*memory_)[i].len - len);
            currentLength_ = len;
            readLength_ -= offest;
            return;
        }
        len += (*memory_)[i].len;
    }

    current_ = 0;
    readLength_ = 0;
    if ((*memory_).empty())
    {
        currentPtr_ = null_ptr;
        currentLength_ = 0;
    }
    else
    {
        currentPtr_ = (*memory_)[current_].buf;
        currentLength_ = (*memory_)[current_].len;
    }
}

size_t InBuffer::size() const
{
    return totalLength_ - readLength_;
}

size_t InBuffer::readLength() const
{
    return readLength_;
}


size_t InBuffer::search(char ch) const
{
    char* p = (char*)::memchr(currentPtr_, ch, currentLength_);
    if (!is_null(p))
        return p - currentPtr_;

    size_t len  = currentLength_;
    for (size_t i = current_ + 1; i < (*memory_).size(); ++i)
    {
        p = (char*)::memchr((*memory_)[i].buf, ch, (*memory_)[i].len);
        if (!is_null(p))
        {
            len += (p - (*memory_)[i].buf);
            return len;
        }

        len += (*memory_)[i].len;
    }

    return buffer_type::npos;
}

size_t InBuffer::search(wchar_t ch) const
{
    return search(&ch, sizeof(ch));
}

inline size_t mem_search(const void* mem, size_t searchLen, const void* context, size_t len)
{
    for (size_t i = 0; i < searchLen; ++i)
    {
        if (!is_null(::memcmp(mem, context, len)))
            return i;
    }
    return buffer_type::npos;
}

size_t InBuffer::search(const void* context, size_t len) const
{
    if (is_null(context) || 0 == len || size() < len)
        return buffer_type::npos;

    if (1 == len)
        return search(*(char*)context);

    // 在多个内存块中查找多个字符时,要注意一下问题,可能是下面问题
    // 1.字符出现在一个内存块中,如 {"aaaacccc", "sdfsafdddsss" } 字符串"ddd"出现在第二个块中
    // 2.字符出现在两个相邻的内存块中,如 {"aaaacccc", "sdfsafdddsss" } 字符串"ccsd"出现在二个块中
    // 3.字符出现在相邻的多个内存块中,如 {"aaaacccc","sa" "sdfsafdddsss" } 字符串"ccsasd"出现在三个块中

    // tmpbuf 用来保存一个内存块结束的len-1个字符和下一个内存块的前len-1个字符,
    std::vector<char> tmpbuf(4*len);
    // tmpbuf 中有效字符的长度.
    size_t tmpLen = 0;
    // 用来代替"&tmpBuf[0]"这样的代码
    char* tmpPtr = &tmpbuf[0];


    // 将要访问的下一个内存块
    size_t i = current_ + 1;
    // 已查找过的内存块总长度
    size_t seekLen = 0;

    // 当前正在处理的内存块
    const char* ptr = currentPtr_;
    // 当前正在处理的内存块的大小
    size_t count = currentLength_;

    do
    {

        size_t searchLen = 0;
        if (count > len)
        {
            // 保存了之前访问的内存块的结尾几个字符(注意有可能是多个内存块的内容见上面所说的第3种情况),现
            // 在将本次要搜索的内存块头部字符拷贝过来
            if (tmpLen > 0)
            {
                size_t copy = len - 1;

                if (0 != ::memcpy_s(tmpPtr + tmpLen, tmpbuf.size() - tmpLen, ptr, copy))
                    ThrowException(RuntimeException);

                searchLen = tmpLen;
                size_t index = mem_search(tmpPtr, searchLen, context, len);
                if (buffer_type::npos != index)
                {
                    seekLen += index;
                    return seekLen;
                }

                seekLen += tmpLen;
                tmpLen = 0;
            }

            // 计算当前内存块要搜索的长度,并搜索当前的内存块,
            searchLen = (count - len + 1);
            size_t index = mem_search(ptr, searchLen, context, len);
            if (buffer_type::npos != index)
            {
                seekLen += index;
                return seekLen;
            }
        }

        // 跳过本次搜索的长度,如果本次的内存块长度小于目标块的长度,则searchLen和tmpLen都为0.
        if (0 == ::memcpy_s(tmpPtr + tmpLen, tmpbuf.size() - tmpLen, ptr + searchLen, count - searchLen))
            ThrowException(RuntimeException);
        seekLen += searchLen;
        tmpLen += (count - searchLen);

        // 处理连续多个内存块小于len, 但总长度大于 len的情况,否则会产生下列情况
        // 如目标长度len为3,连续 60 个内存块的长度为2,这样不处理时就会导致 tmpPtr
        // 指向的内存不足.
        if (tmpLen > len)
        {
            // 计算当前内存块要搜索的长度,并搜索当前的内存块,
            searchLen = (tmpLen - len + 1);
            size_t index = mem_search(tmpPtr, searchLen, context, len);
            if (buffer_type::npos != index)
            {
                seekLen += index;
                return seekLen;
            }

            if (0 == ::memmove_s(tmpPtr, tmpbuf.size(), tmpPtr + searchLen, tmpLen - searchLen))
                ThrowException(RuntimeException);

            tmpLen  -= searchLen;
            seekLen += searchLen;
        }

        if ((*memory_).size() <= i)
            return buffer_type::npos;

        ptr = (*memory_)[i].buf;
        count = (*memory_)[i].len;
        ++i;
    }
    while (true);
    return buffer_type::npos;
}

size_t InBuffer::searchAny(const char* charset) const
{
    if (is_null(charset))
        return buffer_type::npos;

    size_t charsetLen = string_traits<char>::strlen(charset);
    if (1 == charsetLen)
        return search(*charset);

    for (size_t i = 0; i < currentLength_; ++i)
    {
        if (!is_null(::memchr(charset, currentPtr_[i], charsetLen)))
            return i;
    }

    size_t len  = currentLength_;
    for (size_t i = current_ + 1; i < (*memory_).size(); ++i)
    {
        const char* ptr = (*memory_)[i].buf;
        int count = (*memory_)[i].len;

        for (int j = 0; j < count; ++j)
        {
            if (!is_null(::memchr(charset, ptr[j], charsetLen)))
            {
                len += j;
                return len;
            }
        }
        len += (*memory_)[i].len;
    }
    return buffer_type::npos;
}

size_t InBuffer::searchAny(const wchar_t* charset) const
{
    ThrowException(NotImplementedException);
}

const std::vector<io_mem_buf>& InBuffer::rawBuffer() const
{
    return *memory_;
}

size_t InBuffer::rawLength() const
{
    return totalLength_;
}


# ifdef _GOOGLETEST_
TEST(buffer, bufferSearch)
{
    //TODO: 请写用例
}
#endif


_jingxian_end
