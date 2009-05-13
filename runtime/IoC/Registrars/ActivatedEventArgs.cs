

using System;

namespace jingxian.core.runtime
{
	public class ActivatedEventArgs : EventArgs
	{
        ICreationContext _context;
		IComponentRegistration _component;
		object _instance;

        public ActivatedEventArgs(ICreationContext context, IComponentRegistration component, object instance)
		{
			Context = context;
			Component = component;
			Instance = instance;
		}

        public ICreationContext Context
		{
			get
			{
				return _context;
			}
			private set
			{
                Enforce.ArgumentNotNull(value, "value");
				_context = value;
			}
		}

		public IComponentRegistration Component
		{
			get
			{
				return _component;
			}
			private set
			{
                Enforce.ArgumentNotNull(value, "value");
				_component = value;
			}
		}

		public object Instance
		{
			get
			{
				return _instance;
			}
			private set
			{
                Enforce.ArgumentNotNull(value, "value");
				_instance = value;
			}
		}
	}
}
