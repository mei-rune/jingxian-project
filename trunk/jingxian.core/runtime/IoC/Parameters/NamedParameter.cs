

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class NamedParameter : ConstantParameter
    {
        private string _name;

        public NamedParameter(string name, object value)
            : base(value, delegate(ParameterInfo  pi ){ return pi.Name == _name; } )
        {
            _name = Enforce.ArgumentNotNullOrEmpty(name, "name");
        }
        public string Name
        {
            get { return _name; }
        }
    }
}
