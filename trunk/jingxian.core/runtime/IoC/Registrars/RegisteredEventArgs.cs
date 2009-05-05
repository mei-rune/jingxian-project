
using System;

namespace jingxian.core.runtime.registrars
{
    public class RegisteredEventArgs : EventArgs
    {
        IKernel _kernel;
        IComponentRegistration _registration;

        public IKernel Container
        { 
            get { return _kernel; }
            set { _kernel = value;  }
        }

        public IComponentRegistration Registration
        {
            get { return _registration; }
            set { _registration = value; }
        }
    }
}
