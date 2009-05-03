using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime.simpl
{
    using jingxian.core.runtime.registrars;

    public class KernelBuilder : IKernelBuilder
    {
        List<IComponentDescriptor> _descriptors = new List<IComponentDescriptor>();

        MiniKernel _kernel;
        ComponentLifestyle _defaultScope = ComponentLifestyle.Singleton;

        public KernelBuilder(MiniKernel kernel)
        {
            _kernel = kernel;
        }

        #region IKernelBuilder 成员

        public ComponentLifestyle DefaultScope
        {
            get { return _defaultScope; }
        }

        public IDisposable SetDefaultScope(ComponentLifestyle scope)
        {
            throw new NotImplementedException();
        }

        public IKernel Build()
        {
            foreach (IComponentDescriptor descriptor in _descriptors)
            {
            }

            return _kernel;
        }


        IComponentDescriptor GetDescriptor(string serviceId)
        {
            foreach (IComponentDescriptor descriptor in _descriptors)
            {
                if (serviceId == descriptor.Id)
                    return descriptor;
            }
            return null;
        }

        IComponentDescriptor GetDescriptor(Type contract)
        {
            foreach (IComponentDescriptor descriptor in _descriptors)
            {
                if (null == descriptor.Services)
                    continue;
                foreach (Type type in descriptor.Services)
                {
                    if (type == contract)
                        return descriptor;
                }
            }
            return null;
        }


        protected object invokeConstructors(IComponentDescriptor descriptor)
        {
            ConstructorInfo[] constructors = descriptor.ImplementationType.GetConstructors();
            if (constructors.Length > 1)
                throw new RuntimeException(string.Format("不能创建服务[{0}]实例,类型[{1}]有多个构造函数", descriptor.Id, descriptor.ImplementationType.FullName));
            else if (constructors.Length != 1)
                throw new RuntimeException(string.Format("不能创建服务[{0}]实例,类型[{1}]没有构造函数", descriptor.Id, descriptor.ImplementationType.FullName));

            ConstructorInfo constructor = constructors[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();

            if (constructorParameters.Length == 0)
                return Activator.CreateInstance(descriptor.ImplementationType);

            object[] parameters = new object[constructorParameters.Length];

            for (int i = 0; i < constructorParameters.Length; i++)
                parameters[i] = Resolve(constructorParameters[i].ParameterType);

            return constructor.Invoke(parameters);
        }

        protected void invokeSetters( object instance)
        {
            if (null == instance)
                return;

            foreach (PropertyInfo descriptor in instance.GetType().GetProperties())
            {
                if (!descriptor.CanWrite || null == descriptor.GetSetMethod())
                    continue;

                Type propertyType = descriptor.PropertyType;

                try
                {
                    object val = Resolve( propertyType );
                    if (null != val)
                        descriptor.SetValue(instance, val, null);
                }
                catch (Exception exception)
                {
                    throw new RuntimeException(string.Format("创建服务[ " + propertyType.Name + "]失败", exception));
                }
            }
        }

        object Resolve(Type contract)
        {
            IComponentDescriptor descriptor = GetDescriptor(contract);
            object instance = this.invokeConstructors(descriptor);
            this.invokeSetters(instance);
            return instance;
        }

        public void RegisterModule(IModule module)
        {
            throw new NotImplementedException();
        }

        public IReflectiveRegistrar Register<T>()
        {
            return Register(typeof(T));
        }

        public IReflectiveRegistrar Register(Type implementor)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar Register<T>(ComponentActivator<T> creator)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar Register<T>(ComponentActivatorWithParameters<T> creator)
        {
            throw new NotImplementedException();
        }

        public IConcreteRegistrar Register<T>(T instance)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
