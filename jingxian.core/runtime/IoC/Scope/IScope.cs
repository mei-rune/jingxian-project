using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IScope
    {
        bool InstanceAvailable { get; }

        object GetInstance();

        void SetInstance(object instance);

        bool DuplicateForNewContext(out IScope newScope);
    }
}
