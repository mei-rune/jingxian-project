

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace jingxian.core.runtime
{
    public class PositionalParameter : ConstantParameter
    {
        public int Position { get; private set; }

        public PositionalParameter(int position, object value)
            : base(value, pi => pi.Position == position &&
                                pi.Member.MemberType == MemberTypes.Constructor)
        {
            if (position < 0) 
                throw new ArgumentOutOfRangeException("position");

            Position = position;
        }
    }
}
