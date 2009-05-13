using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.scope
{
    static class InstanceScopeFactory
    {
        public static IScope ToScope( ComponentLifestyle lifestyle )
        {
            switch (lifestyle)
            {
                case ComponentLifestyle.Transient:
                    return new TransientScope();
                case ComponentLifestyle.Singleton:
                    return new SingletonScope();
                default:
                    throw new NotSupportedException(string.Format("生命周期 '{0}' 不可识别.", lifestyle));
            }
        }
    }
}
