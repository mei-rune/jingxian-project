
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC822
{
    /// <summary>
    /// Ò»¸ö»»ÐÐ·û£¨\r\n£©
    /// </summary>
    class EndOfLineStrategy:IEndCriteriaStrategy
    {
        public bool IsEndReached(char[] data, int size)
        {
            int previous, current;

            if (size > 0)
            {
                previous = data[size - 1];
                current = data[size];

                if (previous == 13 && current == 10)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
