using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IDisposer : IDisposable
    {
        void AddInstanceForDisposal(IDisposable instance);
    }


    class Disposer : Disposable, IDisposer
    {
        object _synchRoot = new object();
        Stack<WeakReference> _items = new Stack<WeakReference>();
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            lock (_synchRoot)
            {
                while (_items.Count > 0)
                {
                    WeakReference reference = _items.Pop();
                    IDisposable item = (IDisposable)reference.Target;
                    if (reference.IsAlive)
                        item.Dispose();
                }
            }
        }

        public void AddInstanceForDisposal(IDisposable instance)
        {
            Enforce.ArgumentNotNull(instance, "instance");
            CheckNotDisposed();

            lock (_synchRoot)
            {
                _items.Push(new WeakReference(instance));
            }
        }
    }
}
