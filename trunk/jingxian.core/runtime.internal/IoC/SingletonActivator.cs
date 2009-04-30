using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl
{

    [Serializable]
    public abstract class AbstractActivator : IComponentActivator
    {
        private MicroKernel kernel;
        private IServiceRef model;
        private ComponentInstanceDelegate onCreation;
        private ComponentInstanceDelegate onDestruction;

        public AbstractActivator(MicroKernel kernel,
            IServiceRef model,
            ComponentInstanceDelegate onCreation,
            ComponentInstanceDelegate onDestruction)
        {
            this.model = model;
            this.kernel = kernel;
            this.onCreation = onCreation;
            this.onDestruction = onDestruction;
        }

        public MicroKernel Kernel
        {
            get { return kernel; }
        }

        public IServiceRef Model
        {
            get { return model; }
        }

        public ComponentInstanceDelegate OnCreation
        {
            get { return onCreation; }
        }

        public ComponentInstanceDelegate OnDestruction
        {
            get { return onDestruction; }
        }

        public virtual object Create(CreationContext context)
        {
            object instance = InternalCreate(context);

            onCreation(model, instance);

            return instance;
        }

        public virtual void Destroy(object instance)
        {
            InternalDestroy(instance);

            onDestruction(model, instance);
        }

        protected abstract object InternalCreate(CreationContext context);

        protected abstract void InternalDestroy(object instance);
    }

    public class SingletonActivator : AbstractActivator 
    {
        public object _instance;

        public SingletonActivator(MicroKernel kernel,
            IServiceRef model,
            ComponentInstanceDelegate onCreation,
            ComponentInstanceDelegate onDestruction)
            : base( kernel, model, onCreation, onDestruction )
        {
        }

        protected override object InternalCreate(CreationContext context)
        {
            if (null != _instance)
                return _instance;
            _instance = Kernel.createService(context);
            return _instance;
        }

        protected override void InternalDestroy(object instance)
        {

        }
    }
}
