
#ifndef _IDictionary_H_
#define _IDictionary_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include "jingxian/string/string.h"

_jingxian_begin

class IDictionary
{
public:
    virtual ~IDictionary(void) {}

    virtual bool has(const tstring& key) const = 0;

    virtual bool getBoolean(const tstring& key, bool defaultValue = 0) const = 0;

    virtual int8_t getInt8(const tstring& key, int8_t defaultValue = 0) const = 0;

    virtual int16_t getInt16(const tstring& key, int16_t defaultValue = 0) const = 0;

    virtual int32_t getInt32(const tstring& key, int32_t defaultValue = 0) const = 0;

    virtual int64_t getInt64(const tstring& key, int64_t defaultValue = 0) const = 0;

    virtual const tstring& getString(const tstring& key, const tstring& defaultValue = _T("")) const = 0;

    virtual void setBoolean(const tstring& key, bool value) = 0;

    virtual void setInt8(const tstring& key, int8_t value) = 0;

    virtual void setInt16(const tstring& key, int16_t value) = 0;

    virtual void setInt32(const tstring& key, int32_t value) = 0;

    virtual void setInt64(const tstring& key, int64_t value) = 0;

    virtual void setString(const tstring& key, const tchar* buf, size_t len) = 0;

    virtual void dump(tostream& target) const = 0;
};

inline tostream& operator<<(tostream& target, const IDictionary& dict)
{
    dict.dump(target);
    return target;
}

_jingxian_end

#endif //_IDictionary_H_