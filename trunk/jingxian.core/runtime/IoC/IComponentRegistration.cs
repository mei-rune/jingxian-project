

using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public interface IComponentRegistration : IDisposable
	{
        event EventHandler<PreparingEventArgs> Preparing;

		event EventHandler<ActivatingEventArgs> Activating;

		event EventHandler<ActivatedEventArgs> Activated;

        IComponentDescriptor Descriptor { get; }

        void InstanceActivated(ICreationContext context, object instance);

        object Get(ICreationContext context, IEnumerable<IParameter> parameters, IDisposer disposer, out bool newInstance);
    }
}
