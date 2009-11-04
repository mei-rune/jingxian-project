
#ifndef _replace_functions_hpp_
#define _replace_functions_hpp_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <functional>
# include "jingxian/string/string_traits.h"

_jingxian_begin

/**
 * ��ָ���ַ������Դ�С�����ַ���
 * @param[ in ] s �ַ���
 * @param[ in ] what �ַ���
 * @param[ in ] len what�ַ����ĳ���
 * @param[ in ] offest ƫ�ƣ���0��ʼ
 * @return �ҵ���������0��ʼ��λ�ã�û�з���npos
 * @remark ���what�ֿ�ʱ����npos
 */
template<typename stringT>
inline typename stringT::size_type case_find (const stringT	&s,
        typename stringT::value_type const *what,
        typename stringT::size_type		len,
        typename stringT::size_type		offset  = 0 )
{
    if ( (s.size () - offset) < len)
        return stringT::npos;

    if ( NULL == what )
        return stringT::npos;

    typename stringT::value_type const *begin = s.c_str ();
    typename stringT::value_type const *end = begin + s.size ();
    typename stringT::value_type const *p = begin + offset;

    for ( ; p <= end-len; ++p)
        if (! string_traits<typename stringT::value_type>::strnicmp(p, what, len))
            return p - begin;

    return stringT::npos;
}

/**
 * ��with���ַ����滻s�ַ�����ָ��ƫ�ƺ������ָ����what���ַ���
 * @param[ in ] s �ַ���
 * @param[ in ] offset ƫ����
 * @param[ in ] what ���ַ���
 * @param[ in ] with ���ַ���
 * @param[ in ] casesensitive �Ƿ����ִ�Сд��( true ���֣� false ������ )
 * @return �����滻����ַ�����
 */
template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::size_type			offset,
                             const typename stringT::value_type*	what,
                             typename stringT::size_type	whatlen,
                             const typename stringT::value_type* with,
                             typename stringT::size_type withlen,
                             bool casesensitive = true )
{
    if ( casesensitive )
    {
        while ( stringT::npos != (offset = case_find(s, what, whatlen, offset ) ))
        {
            s.replace( offset, whatlen, with , withlen );
            offset += ( withlen + 1 );
        }
    }
    else
    {
        while ( stringT::npos != (offset = s.find( what, offset, whatlen ) ))
        {
            s.replace( offset, whatlen, with , withlen );
            offset += ( withlen + 1 );
        }
    }
    return s;
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::size_type			offset,
                             typename stringT::value_type		what,
                             typename stringT::value_type const * with,
                             typename stringT::size_type len,
                             bool casesensitive = true )
{
    return replace_all( s, offset, &what, 1, with, len, casesensitive );
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::size_type			offset,
                             typename stringT::value_type		what,
                             typename stringT::value_type const * with,
                             bool casesensitive )
{
    return replace_all( s, offset,
                        &what, 1,
                        with, string_traits<typename stringT::value_type>::strlen( with ),
                        casesensitive );
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::size_type			offset,
                             typename stringT::value_type what,
                             const stringT	&with,
                             bool casesensitive  = true)
{
    return replace_all( s, offset,
                        what,
                        c_str_ptr(with),with.size(),
                        casesensitive );
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::size_type			offset,
                             typename stringT::value_type const * what,
                             typename stringT::value_type const * with,
                             bool casesensitive  = true)
{
    return replace_all( s, offset,
                        what,string_traits<typename stringT::value_type>::strlen( what ),
                        with, string_traits<typename stringT::value_type>::strlen( with ),
                        casesensitive );
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::size_type			offset,
                             const stringT& what,
                             const stringT& with,
                             bool casesensitive  = true)
{
    return replace_all( s, offset,
                        c_str_ptr(what),what.size( ),
                        c_str_ptr(with), with.size( ),
                        casesensitive );
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             typename stringT::value_type const * what,
                             typename stringT::value_type const * with,
                             bool casesensitive  = true)
{
    return replace_all( s, 0,
                        what,string_traits<typename stringT::value_type>::strlen( what ),
                        with, string_traits<typename stringT::value_type>::strlen( with ),
                        casesensitive );
}

template<typename stringT>
inline stringT& replace_all (stringT	&s,
                             const stringT	&what,
                             const stringT	&with,
                             bool casesensitive  = true)
{
    return replace_all (s, 0,
                        c_str_ptr( what ), what.size(),
                        c_str_ptr( with ), with.size(),
                        casesensitive );
}

_jingxian_end

#endif /* _replace_functions_hpp_ */