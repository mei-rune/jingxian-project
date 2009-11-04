
#ifndef _concat_functions_hpp_
#define _concat_functions_hpp_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <functional>
# include "jingxian/string/string_traits.h"

_jingxian_begin

template<typename S, typename S1, typename S2>
inline S concat(const S1& s1, const S2& s2)
{
    S s;
    s += s1;
    s += s2;
    return s;
}

template<typename S, typename S1, typename S2, typename S3>
inline S concat(const S1& s1, const S2& s2, const S3& s3)
{
    S s;
    s += s1;
    s += s2;
    s += s3;
    return s;
}

template<typename S, typename S1, typename S2, typename S3, typename S4>
inline S concat(const S1& s1, const S2& s2, const S3& s3, const S4& s4)
{
    S s;
    s += s1;
    s += s2;
    s += s3;
    s += s4;
    return s;
}

template<typename S, typename S1, typename S2, typename S3, typename S4, typename S5>
inline S concat(const S1& s1, const S2& s2, const S3& s3, const S4& s4, const S5& s5)
{
    S s;
    s += s1;
    s += s2;
    s += s3;
    s += s4;
    s += s5;
    return s;
}

template<typename S, typename S1, typename S2, typename S3, typename S4, typename S5, typename S6>
inline S concat(const S1& s1, const S2& s2, const S3& s3, const S4& s4, const S5& s5, const S6& s6)
{
    S s;
    s += s1;
    s += s2;
    s += s3;
    s += s4;
    s += s5;
    s += s6;
    return s;
}

template<typename S, typename S1, typename S2, typename S3, typename S4, typename S5, typename S6, typename S7>
inline S concat(const S1& s1, const S2& s2, const S3& s3, const S4& s4, const S5& s5, const S6& s6, const S7& s7)
{
    S s;
    s += s1;
    s += s2;
    s += s3;
    s += s4;
    s += s5;
    s += s6;
    s += s7;
    return s;
}

template<typename S, typename S1, typename S2, typename S3, typename S4, typename S5, typename S6, typename S7, typename S8>
inline S concat(const S1& s1, const S2& s2, const S3& s3, const S4& s4, const S5& s5, const S6& s6, const S7& s7, const S8& s8)
{
    S s;
    s += s1;
    s += s2;
    s += s3;
    s += s4;
    s += s5;
    s += s6;
    s += s7;
    s += s8;
    return s;
}

_jingxian_end

#endif /* _concat_functions_hpp_ */
