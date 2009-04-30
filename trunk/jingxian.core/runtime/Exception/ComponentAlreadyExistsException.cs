using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    public class ComponentAlreadyExistsException : RuntimeException
    {
        string _componentName;

        public ComponentAlreadyExistsException(string componentName)
            : base(string.Concat("���[", componentName, "]�Ѿ�����"))
        {
            _componentName = componentName;
        }

        public string ComponentName
        {
            get { return _componentName; }
        }
    }
}
