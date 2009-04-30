using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{    
    public delegate void ComponentInstanceDelegate( IServiceRef serviceRef, object instance );

    public interface IComponentActivator
    {
		object Create(CreationContext context);

        void Destroy(object instance);
    }
}
