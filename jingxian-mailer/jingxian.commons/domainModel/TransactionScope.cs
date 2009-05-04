using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.domainModel
{
    public class TransactionScope : IDisposable
    {
        IDbTransaction _transaction;

        public TransactionScope(IDbSession session)
        {
            _transaction = session.BeginTransaction();
        }

        public TransactionScope(IDbSession session, System.Data.IsolationLevel isolationLevel)
        {
            _transaction = session.BeginTransaction(isolationLevel);
        }

        public void VoteCommit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
