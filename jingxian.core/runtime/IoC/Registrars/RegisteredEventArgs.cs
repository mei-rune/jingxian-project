
using System;

namespace jingxian.core.runtime.registrars
{
    public class RegisteredEventArgs : EventArgs
    {
        public IKernel Container { get; set; }

        public IComponentRegistration Registration { get; set; }
    }
}
