using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.threading
{
    /// <summary>
    /// 统一的线对象执行接口
    /// </summary>
    public interface IRunnable
    {
        void Run();
    }

    /// <summary>
    /// 统一的线对象执行接口
    /// </summary>
    public interface IRunnable<T>
    {
        void Run(T args);
    }
}
