using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.threading
{
    public interface ICallable<V>
    {
        V Call();
    }

    public interface ICallable<R, P>
    {
        R Call(P args);
    }
}
