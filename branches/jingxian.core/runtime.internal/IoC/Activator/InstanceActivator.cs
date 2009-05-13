using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.activator
{
    public class InstanceActivator : IComponentActivator
    {
        object _instance;

        public InstanceActivator( object instance )
        {
            _instance = instance;
        }

        public object Create(ICreationContext context, IEnumerable<IParameter> parameters)
        {
            return _instance;
        }

        public void Destroy(object instance)
        {
        }
    }
}
