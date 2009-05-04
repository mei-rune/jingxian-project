using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace jingxian.domainModel
{
    public interface IDbTransaction : IDisposable
    {
        IDbSession Session { get; }
        void Commit();
    }

    public interface IDbSession : IDisposable
    {
        IDbTransaction BeginTransaction();

        IDbTransaction BeginTransaction(IsolationLevel isolationLevel);

        IDictionary<K, V> QueryForDictionary<K, V>(string statementName, object parameterObject, string keyProperty);

        IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty);

        IDictionary<K, V> QueryForDictionary<K, V>(string statementName, object parameterObject, string keyProperty, string valueProperty);

        IDictionary QueryForDictionary(string statementName, object parameterObject, string keyProperty, string valueProperty);

        IList QueryForList(string statementName, object parameterObject);

        IList<T> QueryForList<T>(string statementName, object parameterObject);

        IList<T> QueryForList<T>(string statementName, object parameterObject, int skipResults, int maxResults);

        IList QueryForList(string statementName, object parameterObject, int skipResults, int maxResults);

        T QueryForObject<T>(string statementName, object parameterObject);

        object QueryForObject(string statementName, object parameterObject);

        T QueryForObject<T>(string statementName, object parameterObject, T instanceObject);

        object QueryForObject(string statementName, object parameterObject, object resultObject);

        int Update(string statementName, object parameterObject);

        int Delete(string statementName, object parameterObject);

        object Insert(string statementName, object parameterObject);

        void FlushCaches();

        void Close();
    }
}
