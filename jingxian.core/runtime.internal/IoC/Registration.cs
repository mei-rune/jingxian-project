

using System;
using System.Globalization;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public class Registration : Disposable, IComponentRegistration
    {
        IComponentDescriptor _descriptor;
        IComponentActivator _activator;
        ComponentLifestyle _lifestyle;
        IScope _scope;
        object _synchRoot = new object();


        public event EventHandler<PreparingEventArgs> Preparing;
        public event EventHandler<ActivatingEventArgs> Activating;
        public event EventHandler<ActivatedEventArgs> Activated;


        public Registration(
            IComponentDescriptor descriptor,
            IComponentActivator activator,
            ComponentLifestyle lifestyle )
        {
            _descriptor = Enforce.ArgumentNotNull(descriptor, "descriptor");
            _activator = Enforce.ArgumentNotNull(activator, "activator");
            _lifestyle = lifestyle;
            _scope = scope.InstanceScopeFactory.ToScope( lifestyle );
        }

        #region IComponentRegistration Members

        public IComponentDescriptor Descriptor
        {
            get { return _descriptor; }
        }

        public virtual object Get(ICreationContext context, IEnumerable<IParameter> parameters, IDisposer disposer, out bool newInstance)
        {
            Enforce.ArgumentNotNull(context, "context");
            Enforce.ArgumentNotNull(parameters, "parameters");
            Enforce.ArgumentNotNull(disposer, "disposer");

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
                    var preparingArgs = new PreparingEventArgs(context, this, parameters);
                    Preparing(this, preparingArgs);

                    instance = preparingArgs.Instance ??
                        Activator.Create(context, preparingArgs.Parameters);

                    var activatingArgs = new ActivatingEventArgs(context, this, instance);
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
            var activatedArgs = new ActivatedEventArgs(context, this, instance);
            Activated(this, activatedArgs);
        }

        #endregion

        public virtual ComponentLifestyle Lifestyle
        {
            get { return _lifestyle; }
        }
        public virtual IScope  Scope
        {
            get { return _scope; }
        }

        public virtual IComponentActivator Activator
        {
            get { return _activator; }
        }

        public override string ToString()
        {
            return string.Concat(Descriptor
                , ", Activator = ", Activator
                , ", Scope = ", Scope );
                //, ",Ownership = ", OwnershipModel);
        }
    }
}
