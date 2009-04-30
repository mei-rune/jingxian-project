

using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public interface IComponentDescriptor
    {
        string Id { get; }

        IEnumerable<Type> Services { get; }

        IProperties ExtendedProperties { get; }

        Type ImplementationType { get; }
    }
}
