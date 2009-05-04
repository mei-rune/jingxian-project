using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.threading
{

    /// <summary>
    /// 线程接口
    /// </summary>
    public interface ICompletionPort
    {
        /// <summary>
        /// 将执行方法发送到线程等待队列
        /// </summary>
        /// <param name="runner">执行方法</param>
        void Send(IRunnable runner);


        /// <summary>
        /// 将执行方法发送到线程等待队列
        /// </summary>
        /// <param name="runner">执行方法</param>
        void Send(Handler handler);


        /// <summary>
        /// 将执行方法送到线程等待队列。
        /// </summary>
        /// <param name="callback">执行方法。</param>
        /// <param name="context">传递给执行方法的参数。</param>
        void Send<T>(Handler<T> callback, T context);
    }
}
