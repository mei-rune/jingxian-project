using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl
{
    public class InstanceRef : ServiceRef
    {
        object _instance;

        public InstanceRef(MicroKernel kernel, string id, Type serviceType
            , object instance, IProperties paramenters, IProperties misc)
            : base(kernel, id, serviceType, instance.GetType(), paramenters, misc)
        {
            _instance = instance;
        }

        public override object Get(CreationContext context)
        {
            return _instance;
        }
    }
}
