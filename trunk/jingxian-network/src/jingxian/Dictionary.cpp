
# include "pro_config.h"
# include "jingxian/Dictionary.h"

_jingxian_begin

inline bool toBoolean(const tstring& s)
{
    return s == _T("true") || s == _T("1");
}

inline int8_t toInt8(const tstring& s, int8_t defaultValue)
{
    return (int8_t)string_traits<tstring::value_type>::atoi(s.c_str());
}

inline int16_t toInt16(const tstring& s, int16_t defaultValue)
{
    return (int16_t)string_traits<tstring::value_type>::atoi(s.c_str());
}

inline int32_t toInt32(const tstring& s, int32_t defaultValue)
{
    return string_traits<tstring::value_type>::atoi(s.c_str());
}

inline int64_t toInt64(const tstring& s, int64_t defaultValue)
{
    return string_traits<tstring::value_type>::atoi64(s.c_str());
}

Dictionary::~Dictionary(void)
{
}

bool Dictionary::has(const tstring& key) const
{
    return _container.find(key) != _container.end();
}

bool Dictionary::getBoolean(const tstring& key, bool defaultValue) const
{
    const_iterator it = _container.find(key);
    if (_container.end() == it)
        return defaultValue;
    return toBoolean(it->second);
}

int8_t Dictionary::getInt8(const tstring& key, int8_t defaultValue) const
{
    const_iterator it = _container.find(key);
    if (_container.end() == it)
        return defaultValue;
    return toInt8(it->second, defaultValue);
}


int16_t Dictionary::getInt16(const tstring& key, int16_t defaultValue) const
{
    const_iterator it = _container.find(key);
    if (_container.end() == it)
        return defaultValue;
    return toInt16(it->second, defaultValue);
}

int32_t Dictionary::getInt32(const tstring& key, int32_t defaultValue) const
{
    const_iterator it = _container.find(key);
    if (_container.end() == it)
        return defaultValue;
    return toInt32(it->second, defaultValue);
}

int64_t Dictionary::getInt64(const tstring& key, int64_t defaultValue) const
{
    const_iterator it = _container.find(key);
    if (_container.end() == it)
        return defaultValue;
    return toInt64(it->second, defaultValue);
}

const tstring& Dictionary::getString(const tstring& key, const tstring& defaultValue) const
{
    const_iterator it = _container.find(key);
    if (_container.end() == it)
        return defaultValue;
    return it->second;
}

void Dictionary::setBoolean(const tstring& key, bool value)
{
    _container[ key ] = toString(value);
}

void Dictionary::setInt8(const tstring& key, int8_t value)
{
    _container[ key ] = toString(value);
}

void Dictionary::setInt16(const tstring& key, int16_t value)
{
    _container[ key ] = toString(value);
}

void Dictionary::setInt32(const tstring& key, int32_t value)
{
    _container[ key ] = toString(value);
}

void Dictionary::setInt64(const tstring& key, int64_t value)
{
    _container[ key ] = toString(value);
}

void Dictionary::setString(const tstring& key, const tchar* buf, size_t len)
{
    _container[ key ] = tstring(buf, len);
}

void Dictionary::dump(tostream& target) const
{
}

_jingxian_end