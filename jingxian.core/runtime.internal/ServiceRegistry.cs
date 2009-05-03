

//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using jingxian.core.runtime.simpl.Resources;
//using System.Globalization;

//namespace jingxian.core.runtime.simpl
//{
//    [Service(
//        typeof(IServiceRegistry), typeof(ServiceRegistry),
//        RuntimeConstants.ServiceRegistryId,
//        Constants.Bundles.Internal,
//        Name = ServiceRegistry.OriginalName)]
//    internal sealed class ServiceRegistry: Service, IServiceRegistry
//    {
//        public const string OriginalName = "Service Registry"; 

//        private ServiceRegistrySingleton _singleton;

//        private readonly ILocator  _container;
//        private readonly IExtensionRegistry _registry;

//        private ServiceConfiguration[] _configurations;

//        public ServiceRegistry(ILocator container, IExtensionRegistry registry)
//        {
//            _container = container;
//            _registry = registry;
//        }

//        private ILocator Container
//        {
//            get { return _container; }
//        }

//        private IExtensionRegistry Registry
//        {
//            get { return _registry; }
//        }


//        //private ICacheService _CacheService;
//        //[OptionalDependency]
//        //public ICacheService CacheService
//        //{
//        //    get { return _CacheService; }
//        //    set { _CacheService = value; }
//        //}

//        private ServiceConfiguration[] Configurations
//        {
//            get { return _configurations; }
//        }

//        protected override void internalStart()
//        {
//            base.internalStart();
//            StartSingleton();
//            AddServiceExtensions();
//        }

//        private void StartSingleton()
//        {
//            _singleton = new ServiceRegistrySingleton(this);
//            (_singleton as IService).Start();
//        }

//        private void AddServiceExtensions()
//        {
//            //if (Registry.InitializedFromCache && CacheService != null)
//            //{
//            //    ServiceConfiguration[] serviceCfgs;
//            //    if (CacheService.TryXmlDeserializeWithXmlSerializer(ComponentId, out serviceCfgs))
//            //    {
//            //        TryAddServiceConfigurations(serviceCfgs);
//            //    }
//            //    else
//            //    {
//            //        Scan();
//            //    }
//            //}
//            //else
//            {
//                Scan();
//            }
//        }

//        private void Scan()
//        {
//            ServiceConfiguration[] serviceCfgs = ScanForServiceExtensions();
//            TryAddServiceConfigurations(serviceCfgs);
//            //TryCachingServiceConfigurations(serviceCfgs);
//        }

//        private void TryAddServiceConfigurations(ServiceConfiguration[] serviceCfgs)
//        {
//            for (int i = 0; i < serviceCfgs.Length; i++)
//            {
//                TryAddServiceToKernel(serviceCfgs[i]);
//            }
//        }

//        private void TryAddServiceToKernel(ServiceConfiguration cfg)
//        {
//            Type serviceType = Type.GetType(cfg.ServiceInterface, true, false);
//            Type classType = Type.GetType(cfg.Implementation, true, false);
//            Container.Add(cfg.Id, classType, serviceType, ComponentLifestyle.Singleton);

//            _logger.DebugFormat("增加服务 '{0}'成功.", cfg.Id);

//        }


//        private ServiceConfiguration[] ScanForServiceExtensions()
//        {
//            IExtension[] serviceExtensions = Registry.GetExtensions(Constants.Points.Services);
//            List<ServiceConfiguration> serviceCfgs = new List<ServiceConfiguration>();
//            List<string> predefinedIds = new List<string>();
//            for (int i = 0; i < Platform.PredefinedServices.Length; i++)
//            {
//                predefinedIds.Add(Platform.PredefinedServices[i].Id);
//            }
//            for (int i = 0; i < serviceExtensions.Length; i++)
//            {
//                if (!predefinedIds.Contains(serviceExtensions[i].Id))
//                {
//                    if (serviceExtensions[i].HasConfiguration)
//                    {
//                        ServiceConfiguration cfg = serviceExtensions[i].BuildConfigurationFromXml<ServiceConfiguration>();

//                        serviceCfgs.Add(cfg);
//                    }
//                    else
//                    {
//                        throw new NotSupportedException(
//                            string.Format(CultureInfo.InvariantCulture, 
								
//                                serviceExtensions[i].Id)); 
//                    }
//                }
//            }
//            return serviceCfgs.ToArray();
//        }


//        //private void TryCachingServiceConfigurations(ServiceConfiguration[] serviceCfgs)
//        //{
//        //    _configurations = serviceCfgs; /// @todo or throw them away?
//        //    if (CacheService != null)
//        //    {
//        //        CacheService.XmlSerialize(ComponentId, serviceCfgs);
//        //    }
//        //}


//        protected override void internalStop()
//        {
//            base.internalStop();
//            StopSingleton();
//            _singleton.Dispose();
//        }

//        private void StopSingleton()
//        {
//            ((IService) _singleton).Stop();
//        }

//        #region IServiceRegistry Members

//        public bool HasService(string id)
//        {
//            return Container.Contains(id);
//        }

//        public bool HasService(Type serviceType)
//        {
//            return Container.Contains(serviceType);
//        }

//        public bool HasService<T>()
//        {
//            return Container.Contains(typeof(T));
//        }

//        public T GetService<T>()
//        {
//            return Container.Resolve<T>();
//        }

//        public T GetService<T>(string id)
//        {
//            return (T) Container.Resolve(id, typeof(T));
//        }

//        public bool TryGetService(Type serviceType, out object service)
//        {
//            if (Container.Contains(serviceType))
//            {
//                service = Container[serviceType];
//                return true;
//            }
//            service = null;
//            return false;
//        }

//        public bool TryGetService<T>(out T service)
//        {
//            if (Container.Contains(typeof(T)))
//            {
//                service = Container.Resolve<T>();
//                return true;
//            }
//            service = default(T);
//            return false;
//        }

//        public bool TryGetService(Type serviceType, string id, out object service)
//        {
//            if (Container.Contains(id))
//            {
//                service = Container[id];
//                return serviceType.IsAssignableFrom(service.GetType());
//            }
//            service = null;
//            return false;
//        }

//        public bool TryGetService<T>(string id, out T service)
//        {
//            if (Container.Contains(id))
//            {
//                object o = Container.Resolve(id, typeof(T));
//                if (o != null && typeof(T).IsAssignableFrom(o.GetType()))
//                {
//                    service = (T) o;
//                    return true;
//                }
//            }
//            service = default(T);
//            return false;
//        }

//        public bool TryGetService(string id, out object service)
//        {
//            if (Container.Contains(id))
//            {
//                service = Container[id];
//                return service != null;
//            }
//            service = null;
//            return false;
//        }

//        public object GetService(Type serviceType)
//        {
//            return Container.GetService(serviceType);
//        }

//        #endregion


//    }
//}
