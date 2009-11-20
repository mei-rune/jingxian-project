
#ifndef _trim_functions_hpp_
#define _trim_functions_hpp_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <functional>
# include "jingxian/string/string_traits.h"

_jingxian_begin

template<typename S>
inline S& trim_left_impl(S &str
						 , typename S::value_type const *trimChars
						 , typename S::size_type _count)
{
    typename S::size_type p = str.find_first_not_of(trimChars, 0, _count);
    if (typename S::npos == p)
    {
        str.clear();
    }
    else //if( 0 != p )
    {
        str.erase(0, p);
    }
	return str;
}


template<typename S>
inline S& trim_left(S &str)
{
    typename S::value_type  s_trimChars[] =
    {
        _T(' ')
        ,   _T('\n')
        ,   _T('\r')
        ,   _T('\t')
        ,   _T('\v')
        ,   _T('\0')
    };

    return trim_left_impl(str, s_trimChars, 5);
}

template<typename S>
inline S trim_left(const S &s)
{
	S str(s);
	trim_left(str);
	return str;
}

template<   typename S >
inline S& trim_left(S &str, typename S::value_type const *trimChars)
{
    return trim_left_impl(str
		, c_str_ptr(trimChars)
		, string_traits< typename S::value_type >::strlen(trimChars));
}

template<   typename S >
inline S trim_left(const S &s, typename S::value_type const *trimChars)
{
	S str(s);
    trim_left_impl(str
		, c_str_ptr(trimChars)
		, string_traits< typename S::value_type >::strlen(trimChars));
	return str;
}

template<   typename S0, typename S1 >
inline S0& trim_left(S0 &s, const S1& trimChars)
{
    return trim_left_impl(s, c_str_ptr(trimChars), trimChars.size());
}

template<   typename S0, typename S1 >
inline S0 trim_left(const S0 &s, const S1& trimChars)
{
	S str(s);
    trim_left_impl(str, c_str_ptr(trimChars), trimChars.size());
	return str;
}

template<typename S>
inline S& trim_right_impl(S &str
						  , typename S::value_type const *trimChars
						  , typename S::size_type _count)
{
	typename S::size_type i = str.find_last_not_of(trimChars, S::npos, _count);
    if (typename S::npos == i)
        str.clear();
    else
        str.erase(i + 1);
	return str;
}

template<typename S>
inline S& trim_right(S &str)
{
    typename S::value_type  s_trimChars[] =
    {
        _T(' ')
        ,   _T('\n')
        ,   _T('\r')
        ,   _T('\t')
        ,   _T('\v')
        ,   _T('\0')
    };

    return trim_right_impl(str, s_trimChars, 5);
}

template<typename S>
inline S trim_right(const S &s)
{
	S str(s);
	trim_right(str);
	return str;
}

template<typename S>
inline S& trim_right(S &str, typename S::value_type const *trimChars)
{
    return trim_right_impl(str, trimChars
		, string_traits< typename S::value_type >::strlen(trimChars));
}

template<typename S>
inline S trim_right(const S &s, typename S::value_type const *trimChars)
{
	S str(s);
    trim_right_impl(str, trimChars
		, string_traits< typename S::value_type >::strlen(trimChars));
	return str;
}

template <typename S0, typename S1>
inline S0& trim_right(S0 &str, S1 const &trimChars)
{
    return trim_right_impl(str, c_str_ptr(trimChars), trimChars.size());
}

template <typename S0, typename S1>
inline S0 trim_right(const S0 &s, S1 const &trimChars)
{
	S0 str(s);
    trim_right_impl(str, c_str_ptr(trimChars), trimChars.size());
	return str;
}

template<typename S>
inline S& trim_all_impl(S &str
						 , typename S::value_type const *trimChars
						 , typename S::size_type _count)
{
    return trim_right_impl(trim_left_impl(str, trimChars, _count)
		, trimChars, _count);
}

template<typename S>
inline S& trim_all(S &str)
{
    typename S::value_type  s_trimChars[] =
    {
        _T(' ')
        ,   _T('\n')
        ,   _T('\r')
        ,   _T('\t')
        ,   _T('\v')
        ,   _T('\0')
    };

    return trim_all_impl(str, s_trimChars
		, sizeof(s_trimChars)/sizeof(tchar));
}

template<typename S>
inline S trim_all(const S &s)
{
	S str(s);
	trim_all(str);
	return str;
}

template<typename S>
inline S& trim_all(S &str, typename S::value_type const *trimChars)
{
    return trim_all_impl(str, trimChars
		, string_traits< typename S::value_type >::strlen(trimChars));
}

template<typename S>
inline S trim_all(const S &s, typename S::value_type const *trimChars)
{
	S str(s);
    trim_all_impl(str, trimChars
		, string_traits< typename S::value_type >::strlen(trimChars));
	return str;
}

template<typename S0, typename S1>
inline S0& trim_all(S0 &str, S1 const &trimChars)
{
    return trim_all_impl(str, c_str_ptr(trimChars), trimChars.size());
}

template<typename S0, typename S1>
inline S0 trim_all(const S0 &s, S1 const &trimChars)
{
	S str(s);
    trim_all_impl(str, c_str_ptr(trimChars), trimChars.size());
	return str;
}

_jingxian_end

#endif /* _trim_functions_hpp_ */
