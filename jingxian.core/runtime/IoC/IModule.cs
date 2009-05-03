using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public interface IModule
    {
        void Configure(IKernel kernel);
    }
}
