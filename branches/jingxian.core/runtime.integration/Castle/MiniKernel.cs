

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using jingxian.core.runtime.castleIntegration.Facilities;
using System.Globalization;
using System.ComponentModel;

namespace jingxian.core.runtime.castleIntegration
{
    internal sealed class WindsorAdapter : WindsorContainer, IKernel
    {
        public const string IoCConfigFilename = "jingxian.IoC.config.xml";
        private const string ComponentIdAwareFacilityId = "jingxian.core.runtime.componentIdAwareFacility";

        private static logging.ILog _logger = logging.LogUtils.GetLogger(typeof(ContainerAdapter));

        public WindsorAdapter()
            : base("Platform IoC Container",
            new MicroKernelWrapper(),
            new DefaultComponentInstaller())
        {
        }

        public void InitializeContainer(IApplicationContext context, PredefinedService[] predefinedServices)
        {
            ((DefaultKernel)Kernel).ComponentModelBuilder = new ComponentModelBuilder(Kernel);

            Kernel.ReleasePolicy = new ExplicitReleasePolicy();


            Connect(RuntimeConstants.MiniKernelId, this, typeof(IKernel));

            Connect("kernel", Kernel, typeof(IKernel));
            Connect("windsorContainer", this, typeof(IWindsorContainer));

            //AddCustomComponents(context);
            Kernel.AddComponentInstance(RuntimeConstants.RuntimeConfigurationId, typeof(IApplicationContext), context);


            if (KernelLogger.IsDebugEnabled)
                new KernelLogger(Kernel);

            Kernel.AddComponentInstance(RuntimeConstants.ProductId, typeof(IApplicationContext), context);


            //AddCustomFacilities();
            AddFacility(ComponentIdAwareFacilityId,
                                    new TypeAwareFacility(ComponentIdAwareConcern.ComponentIdAwareModelPropertyName, typeof(IComponentIdAware),
                                                                                ComponentIdAwareConcern.Instance));
            AddFacility(ServiceRunnerFacility.ComponentTypeId, new ServiceRunnerFacility());
            AddFacility(ComponentFacility.ComponentTypeId, new ComponentFacility());


            if (null != context.Arguments)
                Connect(RuntimeConstants.ParsedCommandLineArgumentsId, context.Arguments, typeof(ICommandLineArguments));



            _logger.Debug(" + 开始添加预定义的服务...");

            foreach (PredefinedService svc in predefinedServices)
            {
                Kernel.AddComponent(svc.Id, svc.Service, svc.Implementation);
            }

            _logger.Debug(" > 添加预定义的服务完成.");


            //_initializing = false;
            //_initialized = true;
        }

        public bool Contains(string id)
        {
            return Kernel.HasComponent(id);
        }

        public bool Contains(Type service)
        {
            return Kernel.HasComponent(service);
        }

        public bool Contains<T>()
        {
            return Contains(typeof(T));
        }

        public bool Disconnect(string id)
        {
            return Kernel.RemoveComponent(id);
        }

        #region Connect

        public void Connect<T>(string id, object instance)
        {
            Connect(id, instance, typeof(T));
        }

        public void Connect(string id, object instance, Type serviceType)
        {
            Kernel.AddComponentInstance(id, serviceType, instance);
        }

        public void Connect(string id, Type classType, Type serviceType, ComponentLifestyle lifestyle)
        {
            Hashtable extendingProperties = new Hashtable();
            extendingProperties[typeof(ComponentLifestyle)] = lifestyle;

            switch (lifestyle)
            {
                case ComponentLifestyle.Singleton:
                    Kernel.AddComponentWithExtendedProperties(id, serviceType, classType, extendingProperties);
                    break;
                case ComponentLifestyle.Transient:
                    Kernel.AddComponentWithExtendedProperties(id, serviceType, classType, extendingProperties);
                    break;
                case ComponentLifestyle.DependencyInjectionOnly:
                    Kernel.AddComponentWithExtendedProperties(id, serviceType, classType, extendingProperties);
                    break;
                case ComponentLifestyle.Undefined:
                    Kernel.AddComponent(id, serviceType, classType);
                    break;
                default:
                    throw new ArgumentException("ComponentLifestyle 参数无效: " + lifestyle);
            }
        }

        public void Connect<I, T>() where T : class
        {
            Connect(GetTemporaryId(), typeof(T), typeof(I));
        }

        public void Connect<T>() where T : class
        {
            Connect(GetTemporaryId(), typeof(T));
        }

        private static string GetTemporaryId()
        {
            return string.Format(CultureInfo.InvariantCulture, "temporary-id:{0}", Guid.NewGuid());
        }

        public void Connect<I, T>(string id) where T : class
        {
            Connect(id, typeof(T), typeof(I));
        }

        public void Connect<T>(string id) where T : class
        {
            Connect(id, typeof(T));
        }

        public void Connect(string id, Type classType)
        {
            Kernel.AddComponent(id, classType);
        }

        public void Connect(string id, Type classType, Type serviceType)
        {
            Kernel.AddComponent(id, serviceType, classType);
        }

        #endregion


        object ILocator.this[string key]
        {
            get
            {
                try
                {
                    return this[key];
                }
                catch (HandlerException exc)
                {
                    throw new DependencyResolutionException(exc.Message, exc);
                }
            }
        }

        object ILocator.this[Type service]
        {
            get
            {
                try
                {
                    return this[service];
                }
                catch (HandlerException exc)
                {
                    throw new DependencyResolutionException(exc.Message, exc);
                }
            }
        }

        public T Get<T>()
        {
            try
            {
                return Resolve<T>();
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public T Get<T>(IDictionary arguments)
        {
            try
            {
                return Resolve<T>(arguments);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public object Get(string id)
        {
            try
            {
                return Resolve(id);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public object Get(string id, IDictionary arguments)
        {
            try
            {
                return Resolve(id, arguments);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public T Get<T>(string id)
        {
            try
            {
                return Resolve<T>(id);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public T Get<T>(string id, IDictionary arguments)
        {
            try
            {
                return Resolve<T>(id, arguments);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public object Get(string id, Type service)
        {
            try
            {
                return Resolve(id, service);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public object Get(string id, Type service, IDictionary arguments)
        {
            try
            {
                return Resolve(id, service, arguments);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }

        public object Get(Type service)
        {
            try
            {
                return Resolve(service);
            }
            catch (HandlerException exc)
            {
                throw new DependencyResolutionException(exc.Message, exc);
            }
        }


        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        #region IKernel 成员


        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 创建本类而不直接输出WindsorAdapter类的目的是希望用户的项目中没需要添加castle的引用
    /// </summary>
    public sealed class ContainerAdapter : IKernel
    {

        private static logging.ILog _logger = logging.LogUtils.GetLogger(typeof(ContainerAdapter));


        private WindsorAdapter _adapter = new WindsorAdapter();

        public void InitializeContainer(IApplicationContext context, PredefinedService[] predefinedServices)
        {
            _adapter.InitializeContainer(context, predefinedServices);
        }

        public void Dispose()
        {
            _adapter.Dispose();
            GC.SuppressFinalize(this);
        }


        #region IContainerAdapter 成员


        public bool Contains(string id)
        {
            return _adapter.Contains(id);
        }

        public bool Contains(Type service)
        {
            return _adapter.Contains(service);
        }

        public bool Contains<T>()
        {
            return _adapter.Contains<T>();
        }

        public void Connect(string id, Type classType, Type serviceType, ComponentLifestyle lifestyle)
        {
            _adapter.Connect(id, classType, serviceType, lifestyle );
        }

        public void Connect<TInterface, TImplementation>() where TImplementation : class
        {
            _adapter.Connect<TInterface, TImplementation>();
        }

        public void Connect<TImplementation>() where TImplementation : class
        {
            _adapter.Connect< TImplementation>();
        }

        public void Connect<TInterface, TImplementation>(string id) where TImplementation : class
        {
            _adapter.Connect<TInterface, TImplementation>(id);
        }

        public void Connect<TImplementation>(string id) where TImplementation : class
        {
            _adapter.Connect< TImplementation>(id);
        }

        public void Connect(string id, Type classType)
        {
            _adapter.Connect(id, classType );
        }

        public void Connect(string id, Type classType, Type serviceType)
        {
            _adapter.Connect(id, classType, serviceType );
        }

        public void Connect<TInterface>(string id, object instance)
        {
            _adapter.Connect<TInterface>(id, instance);
        }

        public void Connect(string id, object instance, Type serviceType)
        {
            _adapter.Connect(id, instance, serviceType);
        }

        public T Get<T>()
        {
            return _adapter.Get<T>();
        }

        public T Get<T>(IDictionary arguments)
        {
            return _adapter.Get<T>(arguments);
        }

        public object Get(string id)
        {
            return _adapter.Get( id );
        }

        public object Get(string id, IDictionary arguments)
        {
            return _adapter.Get(id, arguments);
        }

        public T Get<T>(string id)
        {
            return _adapter.Get<T>(id);
        }

        public T Get<T>(string id, IDictionary arguments)
        {
            return _adapter.Get<T>(id, arguments);
        }

        public object Get(string id, Type service)
        {
            return _adapter.Get(id, service);
        }

        public object Get(string id, Type service, IDictionary arguments)
        {
            return _adapter.Get(id, service, arguments);
        }

        public object Get(Type service)
        {
            return _adapter.Get( service );
        }

        public void Release(object instance)
        {
            _adapter.Release(instance);
        }

        public bool Disconnect(string id)
        {
            return _adapter.Disconnect(id);
        }

        #endregion

        #region IKernel 成员

        public object this[Type serviceType]
        {
            get { return _adapter[serviceType]; }
        }

        public object this[string key]
        {
            get { return _adapter[key]; }
        }

        public void Start()
        {
             _adapter.Start();
        }

        public void Stop()
        {
             _adapter.Start();
        }

        #endregion

        #region IServiceProvider 成员

        public object GetService(Type serviceType)
        {
            return _adapter.GetService(serviceType);
        }

        #endregion
    }
}
