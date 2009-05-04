using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.domainModel
{
    using IBatisNet.DataMapper;

    public class DbSession : IDbSession
    {
        ISqlMapper _mapper;
        ISqlMapSession _session;

        public DbSession(ISqlMapper mapper)
        {
            _mapper = mapper;
            if (null == mapper.LocalSession)
                _session = mapper.OpenConnection();

        }

        public IBatisNet.DataMapper.ISqlMapper GetMapper()
        {
            return _mapper;
        }

        #region IDbSession 成员

        public IDbTransaction BeginTransaction()
        {
            return new DbTransaction( this );
        }

        public IDbTransaction BeginTransaction(System.Data.IsolationLevel isolationLevel)
        {
            return new DbTransaction(this, isolationLevel);
        }

        public IDictionary<K, V> QueryForDictionary<K, V>(string statementName, object parameterObject, string keyProperty)
        {
            return _mapper.QueryForDictionary<K, V>(statementName, parameterObject, keyProperty);
        }

        public System.Collections.IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty)
        {
            return _mapper.QueryForDictionary(statementName, parameterObject, keyProperty);
        }

        public IDictionary<K, V> QueryForDictionary<K, V>(string statementName, object parameterObject, string keyProperty, string valueProperty)
        {
            return _mapper.QueryForDictionary<K, V>(statementName, parameterObject, keyProperty, valueProperty);
        }

        public System.Collections.IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty, string valueProperty)
        {
            return _mapper.QueryForDictionary(statementName, parameterObject, keyProperty, valueProperty);
        }

        public System.Collections.IList QueryForList(string statementName, object parameterObject)
        {
            return _mapper.QueryForList(statementName, parameterObject);
        }

        public IList<T> QueryForList<T>(string statementName, object parameterObject)
        {
            return _mapper.QueryForList<T>(statementName, parameterObject);
        }

        public IList<T> QueryForList<T>(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            return _mapper.QueryForList<T>(statementName, parameterObject, skipResults, maxResults );
        }

        public System.Collections.IList QueryForList(string statementName, object parameterObject, int skipResults, int maxResults)
        {
            return _mapper.QueryForList(statementName, parameterObject, skipResults, maxResults);
        }

        public T QueryForObject<T>(string statementName, object parameterObject)
        {
            return _mapper.QueryForObject<T>(statementName, parameterObject);
        }

        public object QueryForObject(string statementName, object parameterObject)
        {
            return _mapper.QueryForObject(statementName, parameterObject);
        }

        public T QueryForObject<T>(string statementName, object parameterObject, T instanceObject)
        {
            return _mapper.QueryForObject<T>(statementName, parameterObject, instanceObject);
        }

        public object QueryForObject(string statementName, object parameterObject, object resultObject)
        {
            return _mapper.QueryForObject(statementName, parameterObject, resultObject);
        }

        public int Update(string statementName, object parameterObject)
        {
            return _mapper.Update(statementName, parameterObject);
        }

        public int Delete(string statementName, object parameterObject)
        {
            return _mapper.Delete(statementName, parameterObject);
        }

        public object Insert(string statementName, object parameterObject)
        {
            return _mapper.Insert(statementName, parameterObject);
        }

        public void FlushCaches()
        {
            _mapper.FlushCaches();
        }

        #endregion

        public void Close()
        {
            if (null != _session)
                _mapper.CloseConnection();
        }

        public void Dispose()
        {
            Close();
        }
    }
}
