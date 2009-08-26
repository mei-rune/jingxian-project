
#ifndef _concat_functions_hpp_
#define _concat_functions_hpp_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <functional>
# include "jingxian/string/string_traits.hpp"

_jingxian_begin

template<typename S,typename S1,typename S2>
inline S concat(const S1& s1, const S2& s2)
{
	S s;
	s += s1;
	s += s2;
	return s;
}

template<typename S,typename S1,typename S2,typename S3>
inline S concat(const S1& s1, const S2& s2, const S3& s3)
{
	S s;
	s += s1;
	s += s2;
	s += s3;
	return s;
}

template<typename S,typename S1,typename S2,typename S3,typename S4>
inline S concat(const S1& s1, const S2& s2, const S3& s3, const S4& s4)
{
	S s;
	s += s1;
	s += s2;
	s += s3;
	s += s4;
	return s;
}

_jingxian_end

#endif /* _concat_functions_hpp_ */
