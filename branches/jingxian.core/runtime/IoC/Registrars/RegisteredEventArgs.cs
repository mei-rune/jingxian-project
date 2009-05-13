
using System;

namespace jingxian.core.runtime.registrars
{
    public class RegisteredEventArgs : EventArgs
    {
        IKernel _kernel;
        IComponentRegistration _registration;

        public RegisteredEventArgs()
        { }

        public RegisteredEventArgs( IKernel kernel, IComponentRegistration registration)
        {
            _kernel = kernel;
            _registration = registration;
        }

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
