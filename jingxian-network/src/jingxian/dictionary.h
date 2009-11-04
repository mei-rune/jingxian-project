
#ifndef _Dictionary12_H_
#define _Dictionary12_H_

#include "jingxian/config.h"

#if !defined (JINGXIAN_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* JINGXIAN_LACKS_PRAGMA_ONCE */

// Include files
# include <hash_map>
# include "IDictionary.h"

_jingxian_begin

class Dictionary : public IDictionary
{
public:

    typedef stdext::hash_map<tstring, tstring> container_type;
    typedef container_type::const_iterator const_iterator;
    typedef container_type::iterator iterator;

    virtual ~Dictionary(void);

    virtual bool has(const tstring& key) const;

    virtual bool getBoolean(const tstring& key, bool defaultValue) const;

    virtual int8_t getInt8(const tstring& key, int8_t defaultValue) const;

    virtual int16_t getInt16(const tstring& key, int16_t defaultValue) const;

    virtual int32_t getInt32(const tstring& key, int32_t defaultValue) const;

    virtual int64_t getInt64(const tstring& key, int64_t defaultValue) const;

    virtual const tstring& getString(const tstring& key, const tstring& defaultValue) const;


    virtual void setBoolean(const tstring& key, bool value);

    virtual void setInt8(const tstring& key, int8_t value);

    virtual void setInt16(const tstring& key, int16_t value);

    virtual void setInt32(const tstring& key, int32_t value);

    virtual void setInt64(const tstring& key, int64_t value);

    virtual void setString(const tstring& key, const tchar* buf, size_t len);

    virtual void dump(tostream& target) const;

private:

    stdext::hash_map<tstring, tstring> _container;
};

_jingxian_end

#endif //_IDictionary_H_