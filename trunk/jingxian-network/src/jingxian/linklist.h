
#ifndef _linklist_h_
#define _linklist_h_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files

_jingxian_begin

template<typename T>
class linklist_traits
{
public:
	static void setNext(T* rhs, T* lhs)
	{
		rhs->_next = lhs;
	}

	static T* getNext(T* t)
	{
		return t->_next;
	}

	static const T* getNext(const T* t)
	{
		return t->_next;
	}
};

template<typename T, typename TRAITS = linklist_traits<T> >
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
     * ��������
     */
    ~linklist()
    {
        while (null_ptr != head_)
        {
            T* current = head_;
			head_ = TRAITS::getNext(head_);

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
     * ��β�����һ�����е��ڴ��
     */
    void push(T* newbuf)
    {
        TRAITS::setNext(newbuf, NULL);
        if (is_null(head_))
            head_ = newbuf;
        else
            TRAITS::setNext(tail_, newbuf);
        tail_ = newbuf;
        ++ length_;
    }

    /**
     * ��ͷ��ȡһ���ڴ��
     */
    T* pop()
    {
        T* current = head_;

        if (null_ptr != head_)
        {
            -- length_;
            head_ = TRAITS::getNext(head_);
        }
        else
        {
            length_ = 0;
            tail_ = NULL;
        }
        return current;
    }

    /**
     * ���ڱ���buf,
     */
    T* next(T* current)
    {
        return is_null(current) ? head_ : TRAITS::getNext(current);
    }

    /**
     * ���ڱ���buf,v
     */
    const T* next(const T* current) const
    {
        return is_null(current) ? head_ : TRAITS::getNext(current);
    }

    /**
     * �Ƿ�Ϊ��
     */
    bool empty() const
    {
        return is_null(head_);
    }

	/**
	 * �ڵ����
	 */
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