using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class NamedPropertyParameter : ConstantParameter
    {
        private string _name;
        public string Name { get; private set; }

        public NamedPropertyParameter(string name, object value)
            : base(value, delegate(ParameterInfo parameterInfo) 
                        { return parameterInfo.Member.Name.Replace("set_", "") == _name; })
        {
            Name = Enforce.ArgumentNotNullOrEmpty(name, "name");
        }
    }
}
