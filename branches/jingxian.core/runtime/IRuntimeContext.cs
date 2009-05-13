
using System;
using System.Collections.Generic;

namespace jingxian.core.runtime
{
    public interface IRuntimeContext : IServiceProvider
    {
        IProperties Misc { get; }
    }
}