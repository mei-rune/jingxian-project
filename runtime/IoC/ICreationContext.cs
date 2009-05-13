using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{

    public interface ICreationContext : IRuntimeContext
    {
        IKernel Kernel { get;  }


        bool TryGetRegistered(string id, out IComponentRegistration registration );

        bool TryGetRegistered(Type service, out IComponentRegistration registration);


        object Get( IComponentRegistration registration );
    }
}
