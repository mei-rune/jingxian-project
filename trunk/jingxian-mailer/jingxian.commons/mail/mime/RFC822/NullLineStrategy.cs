
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC822
{
    /// <summary>
    /// һ�����н�β��Ҳ�����������з���\r\n\r\n��
    /// </summary>
    class NullLineStrategy:IEndCriteriaStrategy
    {
        public bool IsEndReached(char[] data, int size)
        {
            if (size >= 3)
            {
                int fourth = data[size - 3];
                int third = data[size - 2];
                int second = data[size - 1];
                int first = data[size];

                if (fourth == 13 && third == 10 &&
                    second == 13 && first == 10)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
