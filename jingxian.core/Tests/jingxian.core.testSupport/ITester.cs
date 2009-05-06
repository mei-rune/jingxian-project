using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.testSupport
{
    using jingxian.core.runtime;

    [ExtensionContract]
    public interface ITester
    {
        void Test();
    }
}
