

using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public interface IComponentDescriptor
    {
        /// <summary>
        /// 组件的id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 组件的接口
        /// </summary>
        IEnumerable<Type> Services { get; }

        /// <summary>
        /// 组件的实现类型
        /// </summary>
        Type ImplementationType { get; }

        /// <summary>
        /// 生命周期
        /// </summary>
        ComponentLifestyle Lifestyle{ get; }

        /// <summary>
        /// 组件的等组
        /// </summary>
        int ProposedLevel { get; }

        /// <summary>
        /// 参数
        /// </summary>
        IEnumerable<IParameter> Parameters{ get; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        IProperties ExtendedProperties { get; }
    }
}
