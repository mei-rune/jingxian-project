
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC2045
{
    public class ContentTransferEncodingFieldParser:ContentTypeFieldParser,IFieldParser
    {
        protected readonly string m_MechanismPattern;
        protected readonly string m_EncodingPattern;        

        private Regex m_Encoding;
        private Regex m_Mechanism;

        public ContentTransferEncodingFieldParser()
        {
            m_MechanismPattern = "(?i)(7bit|8bit|binary|quoted-printable|base64|" +
                m_XTokenPattern + ")(?i)";
            m_EncodingPattern = "(?i)Content-Transfer-Encoding(?i):[ ]*" + m_MechanismPattern;
            
            m_Mechanism = new Regex(m_MechanismPattern, RegexOptions.Compiled);
            m_Encoding = new Regex(m_EncodingPattern, RegexOptions.Compiled);
        }

        public override void CompilePattern()
        {            
            base.CompilePattern();
        }

        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {            
            MatchCollection matches = m_Encoding.Matches(fieldString);
            foreach (Match match in matches)
            {
                Match enc;
                ContentTransferEncodingField tmpTransfer = new ContentTransferEncodingField();
                enc = m_Mechanism.Match(match.Value);
                tmpTransfer.Name = "Content-Transfer-Encoding";
                tmpTransfer.Encoding = enc.Value;
                fields.Add(tmpTransfer);
            }         
        }
    }
}
