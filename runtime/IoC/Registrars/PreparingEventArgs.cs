
using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public class PreparingEventArgs : EventArgs
    {
        ICreationContext _context;
        IComponentRegistration _component;
        IEnumerable<IParameter> _parameters;
        object _instance;

        public PreparingEventArgs(ICreationContext context, IComponentRegistration component, IEnumerable<IParameter> parameters)
        {
            Context = context;
            Component = component;
            Parameters = parameters;
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
            set
            {
                Enforce.ArgumentNotNull(value, "value");
                _instance = value;
            }
        }


        public IEnumerable<IParameter> Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                Enforce.ArgumentNotNull(value, "value");
                _parameters = value;
            }
        }
    }
}
