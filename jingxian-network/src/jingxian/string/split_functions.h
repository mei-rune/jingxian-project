
#ifndef _split_functions_hpp_
#define _split_functions_hpp_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <functional>
# include <list>
# include "jingxian/string/string_traits.h"
# include "jingxian/string/stringArray.h"

_jingxian_begin

template<typename charT, typename OP >
inline StringArray<charT, OP> split_with_string(const charT* ptr
        , const charT* seps
        , size_t seps_len = -1)
{
    assert(NULL != ptr);
    assert(NULL != seps);

    if (-1 == seps_len)
        seps_len = string_traits<charT>::strlen(seps);

    std::list< stringData<charT> > tmpList;
    const charT* token = null_ptr;
    while (null_ptr != (token = string_traits<charT>::strstr(ptr, seps)))
    {
        stringData<charT> data;

        data.len = token - ptr;
        data.capacity = (data.len + 20);
        data.ptr = (charT*) OP::malloc(sizeof(charT) * data.capacity);
        memset(data.ptr, 0, data.capacity);
        string_traits<charT>::strncpy(data.ptr, data.capacity, ptr, data.len);
        tmpList.push_back(data);
        ptr = token + seps_len;
    }

    if (null_ptr != ptr)
    {
        stringData<charT> data;

        data.ptr = OP::dup(ptr);
        data.len = 0;
        data.capacity = 0;
        tmpList.push_back(data);
    }

    StringArray<charT, OP> result(tmpList.size());
    int i = 0;
    for (std::list<stringData<charT>>::iterator it = tmpList.begin(); it != tmpList.end(); it ++)
    {
        result[ i ] = *it;
        ++i;
    }
    return result;
}

template<typename charT >
inline StringArray<charT, detail::StringOp<charT> > split_with_string(const  charT* str
        , const charT* seps)
{
    return split_with_string<charT, detail::StringOp<charT> >(str, seps);
}

template<typename stringT, typename OP >
inline StringArray<typename stringT::value_type, OP> split_with_string(const  stringT& str
        , const typename stringT::value_type* seps)
{
    return split_with_string<typename stringT::value_type, OP>(str.c_str(), seps);
}

template<typename stringT >
inline StringArray<typename stringT::value_type, detail::StringOp<typename stringT::value_type> > split_with_string(const stringT& str
        , const typename stringT::value_type* seps)
{
    return split_with_string<typename stringT::value_type, detail::StringOp<typename stringT::value_type> >(str.c_str(), seps);
}

template<typename charT, typename OP >
inline StringArray<charT, OP> split(const charT* str, const charT* seps)
{
    assert(NULL != str);
    assert(NULL != seps);

    charT* ptr = OP::dup(str);
    charT* next_token = NULL;

    std::list<charT*> tmpList;
    charT* token = string_traits<charT>::strtok(ptr, seps, &next_token);
    while (token != NULL)
    {
        tmpList.push_back(OP::dup(token));
        token = string_traits<charT>::strtok(NULL, seps , &next_token);
    }

    StringArray<charT, OP> result(tmpList.size());
    int i = 0;
    for (std::list<charT*>::iterator it = tmpList.begin(); it != tmpList.end(); it ++)
    {
        result[ i ].ptr = *it;
        ++i;
    }
    return result;
}

template<typename charT >
inline StringArray<charT, detail::StringOp<charT> > split(const  charT* str
        , const charT* seps)
{
    return split<charT, detail::StringOp<charT> >(str, seps);
}

template<typename stringT, typename OP >
inline StringArray<typename stringT::value_type, OP> split(const  stringT& str
        , const typename stringT::value_type* seps)
{
    return split<typename stringT::value_type, OP>(str.c_str(), seps);
}

template<typename stringT >
inline StringArray<typename stringT::value_type, detail::StringOp<typename stringT::value_type> > split(const stringT& str
        , const typename stringT::value_type* seps)
{
    return split<typename stringT::value_type, detail::StringOp<typename stringT::value_type> >(str.c_str(), seps);
}

_jingxian_end

#endif // _split_functions_hpp_