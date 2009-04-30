
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.ComponentModel;


namespace jingxian.core.runtime
{
    [ExtensionContract(Constants.Points.Services)]
    public abstract class Service : IService, IComponentIdAware, IDisposable
    {
        protected logging.ILog _logger;

        private static readonly List<Type> _instantiatedServiceTypeSingletons = new List<Type>();
        private string _componentId;

        protected internal Service()
        {
            _logger = logging.LogUtils.GetLogger(GetType());

            if (!_instantiatedServiceTypeSingletons.Contains(GetType()))
            {
                _instantiatedServiceTypeSingletons.Add(GetType());
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture
                    , Resources.ExceptionMessages.InstantiateServiceTwice, GetType(), GetDetailedMessage()));
            }
        }

        private static string GetDetailedMessage()
        {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine(Resources.ExceptionMessages.OtherCurrentlyInstantiatedServices);
            foreach (Type type in _instantiatedServiceTypeSingletons)
            {
                builder.AppendLine(" + " + type.Name);
            }
            return builder.ToString();
        }


        ~Service()
        {
            Dispose(false);
        }


        protected virtual void internalStart()
        {
        }


        protected virtual void internalStop()
        {
        }

        public string ComponentId
        {
            get { return _componentId; }
            set
            {
                if (null != _componentId)
                    throw new WriteOnceViolatedException("ComponentId");

                _componentId = value;
            }
        }

        public string Id
        {
            get { return ComponentId; }
        }

        public void Start()
        {
            _logger.DebugFormat("启动服务 {0}.", ComponentId);

            internalStart();
        }

        public void Stop()
        {
            _logger.DebugFormat("停止服务 {0}.", ComponentId);

            internalStop();
        }

        public void Dispose()
        {
            _instantiatedServiceTypeSingletons.Remove(GetType());
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
