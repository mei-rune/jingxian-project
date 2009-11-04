
#ifndef _linklist_h_
#define _linklist_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

_jingxian_begin

template<typename T>
class linklist
{
public:
    linklist()
            : head_(0)
            , tail_(0)
            , length_(0)
    {
    }

    /**
    * 析构函数
    */
    ~linklist()
    {
        while (null_ptr != head_)
        {
            T* current = head_;
            head_ = (head_)->_next;

            freebuffer(current);
        }
    }

    T* head()
    {
        return head_;
    }

    const T* head() const
    {
        return head_;
    }

    T* tail()
    {
        return is_null(head_) ? null_ptr : tail_;
    }

    const T* tail() const
    {
        return is_null(head_) ? null_ptr : tail_;
    }

    /**
    * 向尾部添加一个空闲的内存块
    */
    void push(T* newbuf)
    {
        newbuf->_next = NULL;
        if (is_null(head_))
            head_ = newbuf;
        else
            tail_->_next = newbuf;
        tail_ = newbuf;
        ++ length_;
    }

    /**
    * 向头部取一个内存块
    */
    T* pop()
    {
        T* current = head_;

        if (null_ptr != head_)
        {
            -- length_;
            head_ = head_->_next;
        }
        else
        {
            length_ = 0;
            tail_ = NULL;
        }
        return current;
    }

    /**
    * 用于遍历buf,
    */
    T* next(T* current)
    {
        return is_null(current) ? head_ : current->_next;
    }

    /**
    * 用于遍历buf,v
    */
    const T* next(const T* current) const
    {
        return is_null(current) ? head_ : current->_next;
    }

    /**
    * 是否为空
    */
    bool empty() const
    {
        return is_null(head_);
    }

    size_t size() const
    {
        return length_;
    }

protected:
    T* head_;
    T* tail_;
    size_t length_;
private:
    NOCOPY(linklist);
};



_jingxian_end

#endif //_linklist_h_