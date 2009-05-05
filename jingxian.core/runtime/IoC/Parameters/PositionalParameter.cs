

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class PositionalParameter : ConstantParameter
    {
        int _position;

        public int Position
        {
            get { return _position; } 
        }

        public PositionalParameter(int position, object value)
            : base(value, delegate(ParameterInfo pi)
        {
            return pi.Position == position &&
                pi.Member.MemberType == MemberTypes.Constructor;
        })
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("position");

            _position = position;
        }
    }
}
