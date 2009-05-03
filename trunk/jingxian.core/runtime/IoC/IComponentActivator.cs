using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IComponentActivator
    {
        object Create(ICreationContext context, IEnumerable<IParameter> parameters);

        void Destroy(object instance);
    }
}
