using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public class KernelAdapter : MicroKernel, IKernel 
    {
        public KernelAdapter()
        {
            _componentsByType[typeof(IServiceProvider)] = this;
            _componentsByType[typeof(ILocator)] = this;
            _componentsByType[typeof(IKernel)] = this;
            _componentsByType[typeof(IObjectBuilder)] = new ObjectBuilder( this );
        }

        public KernelAdapter(IServiceProvider serviceProvider)
        : base( serviceProvider )
        {
            _componentsByType[typeof(IServiceProvider)] = this;
            _componentsByType[typeof(ILocator)] = this;
            _componentsByType[typeof(IKernel)] = this;
            _componentsByType[typeof(IObjectBuilder)] = new ObjectBuilder(this);
        }

        #region IKernel 成员

        public bool Contains(string id)
        {
            return _componentsById.ContainsKey(id);
        }

        public bool Contains(Type service)
        {
            return _componentsByType.ContainsKey(service);
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T)); 
        }

        public void Release(object instance)
        {
           
        }

        public IKernelBuilder CreateBuilder()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region ILocator 成员


        public T Get<T>()
        {
            return (T) GetService(typeof(T));
        }

        public T Get<T>(string id)
        {
            return (T)Get(id, typeof(T));
        }

        public object Get(string id, Type service)
        {
            object value = GetService(id);
            if (null == value)
                return value;
            if (service.IsAssignableFrom(value.GetType()))
                return value;
            return null;
        }

        #endregion


        
    [Service(
        typeof(IObjectBuilder), typeof(ObjectBuilder),
        RuntimeConstants.ObjectBuilderServiceId,
        Constants.Bundles.Internal,
        Name = ObjectBuilder.OriginalName)]
    internal sealed class ObjectBuilder : Service, IObjectBuilder
    {
        public const string OriginalName = "Object Builder Service";

        KernelAdapter _kernelAdapter;

        public ObjectBuilder(KernelAdapter kernel)
        {
            _kernelAdapter = kernel;
        }

        #region IObjectBuilder 成员

        public bool TryGetType(string typeName, out Type type)
        {
            Enforce.ArgumentNotNullOrEmpty(typeName, "typeName");

            type = Type.GetType(typeName, false, false);
            return type != null;
        }

        public Type GetType(string typeName)
        {
            Enforce.ArgumentNotNullOrEmpty(typeName, "typeName");

            Type type = Type.GetType(typeName, true, false);
            return type;
        }

        public T BuildTransient<T>()
        {
            return (T)BuildTransient(typeof(T));
        }

        public object BuildTransient(Type classType)
        {
            Enforce.ArgumentNotNull( classType, "classType" );

            bool newInstance;
            return _kernelAdapter.createInstance(classType, out newInstance);
        }

        public object BuildTransient(string classType)
        {
            Enforce.ArgumentNotNullOrEmpty(classType, "classType");

            bool newInstance;
            return _kernelAdapter.createInstance(GetType(classType), out newInstance);
        }

        #endregion
    }
    }
}
