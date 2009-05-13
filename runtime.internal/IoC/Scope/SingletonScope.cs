using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.scope
{
    public class SingletonScope : IScope
    {
        object _instance;

        public bool InstanceAvailable
        {
            get { return null != _instance; }
        }

        public object GetInstance()
        {
            return Enforce.NotNull(_instance, "instance");
        }

        public void SetInstance(object instance)
        {
            _instance = Enforce.ArgumentNotNull(instance, "instance");
        }

        public bool DuplicateForNewContext(out IScope newScope)
        {
            newScope = null;
            return false;
        }

        public override string ToString()
        {
            return "TransientScope";
        }
    }
}
