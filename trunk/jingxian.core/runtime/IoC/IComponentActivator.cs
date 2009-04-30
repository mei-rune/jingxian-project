using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IComponentActivator
    {
		object Create(ICreationContext context);

        void Destroy(object instance);
    }
}
