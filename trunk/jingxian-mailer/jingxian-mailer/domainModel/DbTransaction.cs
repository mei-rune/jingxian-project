using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.domainModel
{
    using IBatisNet.DataMapper;

    internal class DbTransaction : IDbTransaction
    {
        DbSession _dbSession;
        ISqlMapSession _mappedSession;
        string _name = null;//"jingxian.domainModel.DbTransaction";
        bool _isCommit;
        bool _isOwnerSession;

        public DbTransaction(DbSession session)
            : this(session, System.Data.IsolationLevel.Unspecified )
        {
        }

        public DbTransaction(DbSession session, System.Data.IsolationLevel isolationLevel)
        {
            _dbSession = session;
            _name = "jingxian.domainModel.DbTransaction" + _dbSession.GetMapper().Id;

            ISqlMapSession mappedSession = _dbSession.GetMapper().LocalSession;
            if (null == mappedSession)
            {
                _mappedSession = _dbSession.GetMapper().BeginTransaction(isolationLevel);
                _name = "jingxian.domainModel.DbTransaction" + _dbSession.GetMapper().Id;
                _isOwnerSession = true;
            }
            else if (!mappedSession.IsTransactionStart) 
            {
                mappedSession.BeginTransaction(isolationLevel);
                _mappedSession = mappedSession;
                _isOwnerSession = false;
            }

            _isCommit = false;

            //_dbSession.GetMapper().BeginTransaction(isolationLevel);
        }

        public IDbSession Session
        {
            get { return _dbSession; }
        }

        public void Commit()
        {
            _isCommit = true;

            if (null == _mappedSession)
                return;

            object isCommit = System.Runtime.Remoting.Messaging.CallContext.GetData(_name);
            if (null != isCommit)
            {
                if (!(bool)isCommit)
                    throw new System.ApplicationException();
            }

            if (_isOwnerSession)
                _dbSession.GetMapper().CommitTransaction();
            else
                _mappedSession.CommitTransaction();
        }

        public void Dispose()
        {
            if( _isCommit )
                return;

            if (null == _mappedSession)
            {
                System.Runtime.Remoting.Messaging.CallContext.SetData(_name, false);
            }
            else
            {

                if (_isOwnerSession)
                    _dbSession.GetMapper().RollBackTransaction();
                else
                    _mappedSession.RollBackTransaction();
                System.Runtime.Remoting.Messaging.CallContext.SetData(_name, null);
            }
        }
    }
}
