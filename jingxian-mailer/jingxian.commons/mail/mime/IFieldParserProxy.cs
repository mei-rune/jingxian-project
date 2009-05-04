
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace jingxian.mail.mime
{
    /// <summary>
    /// 分析消息各部分的分析器代理接口
    /// </summary>
    public interface IFieldParserProxy
    {
        void CompilePattern();
        Regex CompositeType { get; }
        Regex DescriteType { get; }
        Regex EndBoundary { get; }
        Regex MIMEVersion { get; }
        Regex AddrSpec { get; }
        void Parse(ref IList<RFC822.Field> fields, ref string fieldString);        
        Regex StartBoundary { get; }
        string Unfold(string headers);
    }
}
