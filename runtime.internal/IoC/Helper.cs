using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.simpl
{
    public class Helper
    {

        public static bool IsReference(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            str = str.Trim();

            return str.StartsWith("#{") || str.StartsWith("${");
        }


        public static string ExtractReference(string str)
        {
            return str.Trim().Substring(2).TrimEnd('}');
        }
    }
}
