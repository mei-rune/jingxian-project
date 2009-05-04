
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime
{
    /// <summary>
    /// 分析消息头各值域的分析器接口
    /// 简简说就是将消息头的每一行按":"分隔成键值对
    /// </summary>
    public interface IFieldParser
    {
        void Parse(ref IList<RFC822.Field> fields, ref string fieldString);

        void CompilePattern();
    }
}
