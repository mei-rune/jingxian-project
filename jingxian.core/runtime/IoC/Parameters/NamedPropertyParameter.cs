using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class NamedPropertyParameter : ConstantParameter
    {
        private string _name;
        public string Name 
        {
            get { return _name; }
        }

        public NamedPropertyParameter(string name, object value)
            : base(value, delegate(ParameterInfo parameterInfo) 
                        { return parameterInfo.Member.Name.Replace("set_", "") == _name; })
        {
            _name = Enforce.ArgumentNotNullOrEmpty(name, "name");
        }
    }
}
