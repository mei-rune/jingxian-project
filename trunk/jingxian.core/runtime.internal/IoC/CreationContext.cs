using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{

    public class CreationContext : AbstractServiceContainer, IRuntimeContext
    {
        string _id;
        IServiceRef _serviceRef;
        IKernel _kernel;
        Stack<IServiceRef> _stack = new Stack<IServiceRef>();

        public CreationContext(IKernel kernel
            , IServiceRef serviceRef )
            : base( kernel )
        {
            _kernel = kernel;
            _serviceRef = serviceRef;
        }

        public IKernel Kernel
        {
            get { return _kernel; }
        }

        public string Id
        {
            get { return _serviceRef.Id; }
        }

        public Type ServiceType
        {
            get { return _serviceRef.ServiceType; }
        }

        public Type ImplementationType
        {
            get { return _serviceRef.ImplementationType; }
        }

        public IProperties Parameters
        {
            get { return _serviceRef.Parameters; }
        }

        public IProperties Misc
        {
            get { return _serviceRef.Misc; }
        }

        public Stack<IServiceRef> CallStack
        {
            get { return _stack; }
        }
    }
}
