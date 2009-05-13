using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public class Disposable : IDisposable
    {
        bool _isDisposed;
        object _synchRoot = new object();

        public void Dispose()
        {
            lock (_synchRoot)
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected bool IsDisposed
        {
            get { return _isDisposed; }
        }

        protected virtual void CheckNotDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
