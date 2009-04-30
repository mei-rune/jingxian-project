using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl
{
    public class DefaultRef : ServiceRef
    {

        IComponentActivator _activator;
        public DefaultRef(MicroKernel kernel, string id, Type serviceType
            , Type implementType, IProperties paramenters,ComponentLifestyle lifestyleType, IProperties misc)
            : base(kernel, id, serviceType, implementType, paramenters, misc)
        {
            _activator = kernel.CreateActivator(lifestyleType, this);
            if (null == _activator)
                throw new RuntimeException("不可识别的生命周期 - " + lifestyleType.ToString());
        }

        public override object Get(CreationContext context)
        {
            //if (null == context)
            //    context = Kernel.NewCreateContext(this);
            context.CallStack.Push(this);
            return _activator.Create(context);
        }
    }
}
