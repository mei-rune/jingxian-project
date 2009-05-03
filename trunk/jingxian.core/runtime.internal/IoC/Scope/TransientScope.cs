using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.scope
{
    public class TransientScope : IScope
    {
        public bool InstanceAvailable
        {
            get { return false; }
        }

        public object GetInstance()
        {
            //TODO: 以后来做国际化
            throw new InvalidOperationException("实例不可用");
        }

        public void SetInstance(object instance)
        {
            Enforce.ArgumentNotNull(instance, "instance");
        }

        public bool DuplicateForNewContext(out IScope newScope)
        {
            newScope = new TransientScope();
            return true;
        }

        public override string ToString()
        {
            return "TransientScope";
        }
    }
}
