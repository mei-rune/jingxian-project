

using System;
using System.Globalization;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public class Registration : Disposable, IComponentRegistration
    {
        IComponentDescriptor _descriptor;
        protected IComponentActivator _activator;
        protected ComponentLifestyle _lifestyle;
        protected IEnumerable<IParameter> _parameters;
        protected IScope _scope;
        protected object _synchRoot = new object();

        public event EventHandler<PreparingEventArgs> Preparing;
        public event EventHandler<ActivatingEventArgs> Activating;
        public event EventHandler<ActivatedEventArgs> Activated;

        public Registration(
            IComponentDescriptor descriptor,
            IComponentActivator activator,
            IEnumerable<IParameter> parameters,
            ComponentLifestyle lifestyle )
        {
            _descriptor = Enforce.ArgumentNotNull(descriptor, "descriptor");
            _activator = Enforce.ArgumentNotNull(activator, "activator");
            _lifestyle = lifestyle;
            _scope = scope.InstanceScopeFactory.ToScope( lifestyle );
        }

        public IComponentDescriptor Descriptor
        {
            get { return _descriptor; }
        }

        public virtual object Get(ICreationContext context, out bool newInstance)
        {
            Enforce.ArgumentNotNull(context, "context");

            CheckNotDisposed();

            lock (_synchRoot)
            {
                object instance;
                if (_scope.InstanceAvailable)
                {
                    instance = _scope.GetInstance();
                    newInstance = false;
                }
                else
                {
                    PreparingEventArgs preparingArgs = new PreparingEventArgs(context, this, _parameters);
                    Preparing(this, preparingArgs);

                    instance = preparingArgs.Instance ??
                        _activator.Create(context, preparingArgs.Parameters);

                    ActivatingEventArgs activatingArgs = new ActivatingEventArgs(context, this, instance);
                    Activating(this, activatingArgs);

                    instance = activatingArgs.Instance;

                    _scope.SetInstance(instance);
                    newInstance = true;
                }

                return instance;
            }
        }

        public virtual void InstanceActivated(ICreationContext context, object instance)
        {
            ActivatedEventArgs activatedArgs = new ActivatedEventArgs(context, this, instance);
            Activated(this, activatedArgs);
        }

        public override string ToString()
        {
            return string.Concat("( ", Descriptor
                , ", Activator = ", _activator
                , ", Scope = ", _scope, ")" );
                //, ",Ownership = ", OwnershipModel);
        }
    }
}
