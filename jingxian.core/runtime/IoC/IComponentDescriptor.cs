

using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public interface IComponentDescriptor
    {
        string Id { get; }

        IEnumerable<Type> Services { get; }

        Type ImplementationType { get; }

        ComponentLifestyle Lifestyle{ get; }

        IEnumerable<IParameter> Parameters{ get; }

        IProperties ExtendedProperties { get; }
    }
}
