

using System.Collections.Generic;
using System.Globalization;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Facilities;
using System.Diagnostics;

namespace jingxian.core.runtime.castleIntegration.Facilities
{
	internal sealed class ServiceRunnerFacility: AbstractFacility
	{
		public const string ComponentTypeId = "jingxian.core.runtime.serviceRunnerFacility";
		private const string IsServiceModelPropertyName = "jingxian.core.runtime.runnable";

		private static logging.ILog _logger = logging.LogUtils.GetLogger( typeof( ServiceRunnerFacility ));

		private readonly List<IHandler> _unresolvedComponentsWaitingList = new List<IHandler>();



		protected override void Init()
		{
			Kernel.ComponentModelCreated += OnKernelComponentModelCreated;
			Kernel.ComponentRegistered += OnKernelComponentRegistered;
		}


		private void OnKernelComponentRegistered(string key, IHandler handler)
		{
			bool isRunnable = (bool) handler.ComponentModel.ExtendedProperties[IsServiceModelPropertyName];
			if (isRunnable)
			{
				if (handler.CurrentState == HandlerState.WaitingDependency)
				{
                    _logger.DebugFormat("添加自启动组件 '{0}' 到时等待列表!.", key);


					_unresolvedComponentsWaitingList.Add(handler);
				}
				else
				{
                    _logger.DebugFormat("启动自启动组件 '{0}'.", key);
					RunComponent(key);
				}
			}
			CheckUnresolvedWaitingComponents();
		}

		private static void OnKernelComponentModelCreated(ComponentModel model)
		{
			bool isService = typeof(IService).IsAssignableFrom(model.Implementation);
			model.ExtendedProperties[IsServiceModelPropertyName] = isService;
			if (isService)
			{
				model.LifestyleType = LifestyleType.Singleton;
				model.LifecycleSteps.Add(LifecycleStepType.Commission, RunConcern.Instance);
				model.LifecycleSteps.Add(LifecycleStepType.Decommission, TerminateConcern.Instance);
			}
		}

        private void CheckUnresolvedWaitingComponents()
        {
            List<IHandler> validComponents = new List<IHandler>();
            foreach (IHandler handler in _unresolvedComponentsWaitingList)
            {
                if (handler.CurrentState == HandlerState.Valid)
                {
                    validComponents.Add(handler);
                }
            }
            foreach (IHandler handler in validComponents)
            {
                _logger.DebugFormat( "启动组件 '{0}' (等待列表).", handler.ComponentModel.Name);

                _unresolvedComponentsWaitingList.Remove(handler);
                RunComponent(handler.ComponentModel.Name);
            }
        }


		private void RunComponent(string key)
		{
			object instance = Kernel[key];
		}

	}
}