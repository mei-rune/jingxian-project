
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime
{
    /// <summary>
    /// 用于在分析消息时判断数据是否结束的接口
    /// </summary>
    public interface IEndCriteriaStrategy
    {
        bool IsEndReached(char[] data, int size);
    }
}
