

using Castle.MicroKernel;

namespace jingxian.core.runtime.castleIntegration
{
    internal sealed class KernelLogger
    {
        public const string ComponentId = "jingxian.core.runtime.kernelLogger";


        private static logging.ILog _logger = logging.LogUtils.GetLogger("jingxian.core.runtime.kernelLogger");

        public static bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        public KernelLogger(IKernelEvents kernelEvents)
        {
            AddEventHandler(kernelEvents);
        }

        private void AddEventHandler(IKernelEvents kernelEvents)
        {
            kernelEvents.ComponentRegistered += Kernel_ComponentRegistered;
            kernelEvents.ComponentUnregistered += Kernel_ComponentUnregistered;
            kernelEvents.ComponentCreated += Kernel_ComponentCreated;
            kernelEvents.ComponentDestroyed += Kernel_ComponentDestroyed;
            kernelEvents.DependencyResolving += Kernel_DependencyResolving;
            kernelEvents.ComponentModelCreated += Kernel_ComponentModelCreated;
            kernelEvents.HandlerRegistered += Kernel_HandlerRegistered;
        }

        private void Kernel_HandlerRegistered(IHandler handler, ref bool stateChanged)
        {
            _logger.DebugFormat("+ Registered new handler for '{0}' - State: _{1}_", handler.ComponentModel.Service.Name,
                           handler.CurrentState.ToString());

        }

        private void Kernel_ComponentModelCreated(Castle.Core.ComponentModel model)
        {
            _logger.DebugFormat(": Created ComponentModel '{0}' for service '{1}' implemented by '{2}'.", model.Name,
                           model.Service.Name, model.Implementation.FullName);

        }

        private void Kernel_DependencyResolving(Castle.Core.ComponentModel client, Castle.Core.DependencyModel model,
                                                object dependency)
        {
            _logger.DebugFormat("~ Resolving dependency '{0}' for component '{1}'.", model.TargetType.Name, client.Service.Name);

        }

        private void Kernel_ComponentDestroyed(Castle.Core.ComponentModel model, object instance)
        {
            _logger.DebugFormat("x Destroyed component '{0}'.", model.Service.Name);

        }

        private void Kernel_ComponentCreated(Castle.Core.ComponentModel model, object instance)
        {
            _logger.DebugFormat("*** Created component '{0}'.", model.Service.Name);
        }

        private void Kernel_ComponentUnregistered(string key, IHandler handler)
        {
            _logger.DebugFormat("- Unregistered component '{0}'.", key);

        }

        private void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            _logger.DebugFormat("+ Registered component '{0}' as '{1}'.", key, handler.ComponentModel.Service.Name);

        }

    }
}
