using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IServiceRef
    {
        /// <summary>
        /// 组件的ID 
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 组件向用户提供的接口类型
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// 组件的实际类型
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// 参数
        /// </summary>
        IProperties Parameters { get; }

        /// <summary>
        /// 服务属性
        /// </summary>
        IProperties Misc { get; }

        /// <summary>
        /// 取得服务实例
        /// </summary>
        object Get(CreationContext context);
    }
}
