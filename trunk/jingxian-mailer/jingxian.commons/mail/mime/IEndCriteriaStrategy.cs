
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime
{
    /// <summary>
    /// �����ڷ�����Ϣʱ�ж������Ƿ�����Ľӿ�
    /// </summary>
    public interface IEndCriteriaStrategy
    {
        bool IsEndReached(char[] data, int size);
    }
}
