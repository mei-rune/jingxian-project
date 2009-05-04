
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime
{
    /// <summary>
    /// ������Ϣͷ��ֵ��ķ������ӿ�
    /// ���˵���ǽ���Ϣͷ��ÿһ�а�":"�ָ��ɼ�ֵ��
    /// </summary>
    public interface IFieldParser
    {
        void Parse(ref IList<RFC822.Field> fields, ref string fieldString);

        void CompilePattern();
    }
}
