using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{
    public abstract class ServiceRef : IServiceRef
    {
        string _id;
        Type _interfaceType;
        Type _implementationType;
        IProperties _paramenters;
        IProperties _misc;

        MicroKernel _kernel;

        public ServiceRef(MicroKernel kernel, string id, Type serviceType, Type implementType, IProperties paramenters, IProperties misc)
        {
            _kernel = kernel;
            _id = id;
            _interfaceType = serviceType;
            _implementationType = implementType;
            _paramenters = paramenters;
            _misc = misc;

        }

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Type ServiceType
        {
            get { return _interfaceType; }
            set { _interfaceType = value; }
        }
        
        public IProperties Parameters
        {
            get 
            {
                if (null == _paramenters)
                    _paramenters = new MapProperties();
                return _paramenters; 
            }
        }

        public IProperties Misc
        {
            get 
            {
                if (null == _misc)
                    _misc = new MapProperties();
                return _misc; 
            }
        }

        public MicroKernel Kernel
        {
            get { return _kernel; }
        }

        public Type ImplementationType
        {
            get { return _implementationType; }
            set { _implementationType = value; }
        }

        public abstract object Get(CreationContext context);


    }
}
