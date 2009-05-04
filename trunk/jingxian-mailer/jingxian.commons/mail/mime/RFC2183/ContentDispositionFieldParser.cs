
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC2183
{
    public class ContentDispositionFieldParser: RFC2045.ContentTypeFieldParser, IFieldParser
    {
        protected readonly string m_DispositionPattern;        
        protected readonly string m_DispositionParmPattern;
        protected readonly string m_FilenameParmPattern;
        protected readonly string m_QuotedDateTimePattern;
        protected readonly string m_CreationDateParmPattern;
        protected readonly string m_ModificationDateParmPattern;
        protected readonly string m_ReadDateParmPattern;
        protected readonly string m_SizeParmPattern;

        protected List<string> m_DispositionTypes;

        private StringBuilder m_DispositionTypesBuilder;

        private Regex m_DispositionParm;
        private Regex m_DispositionType;
        private Regex m_Disposition;

        public ContentDispositionFieldParser()
        {
            m_DispositionTypes = new List<string>();            
            
            m_FilenameParmPattern = "filename=.*?(?=;)";
            m_QuotedDateTimePattern = "\"" + m_DateTimePattern + "\"";
            m_CreationDateParmPattern = "creation-date=" + m_QuotedDateTimePattern;
            m_ModificationDateParmPattern = "modification-date=" + m_QuotedDateTimePattern;
            m_ReadDateParmPattern = "read-date=" + m_QuotedDateTimePattern;
            m_SizeParmPattern = "size=[0-9]{1,1}";

            m_DispositionParmPattern = "(" + m_FilenameParmPattern + "|" + m_CreationDateParmPattern + 
                "|" + m_ModificationDateParmPattern + "|" + m_ReadDateParmPattern + "|" + 
                m_SizeParmPattern + "|" + m_ParameterPattern + ")";

            m_DispositionParm = new Regex(m_DispositionParmPattern, RegexOptions.Compiled);
        }

        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {
            if (fields.Count == 0)
            {
                base.Parse(ref fields, ref fieldString);
            }

            IList<RFC822.Field> tmpFields = new List<RFC822.Field>();

            foreach(RFC822.Field field in fields)
            {
                if(field.Name.Equals("Content-Disposition"))
                {
                    Match typeMatch, tmpMatch;
                    string key, val;
                    
                    typeMatch = m_DispositionType.Match(field.Body);
                    MatchCollection parameterMatches = m_DispositionParm.Matches(field.Body);

                    ContentDispositionField dispositionField = new ContentDispositionField();
                    dispositionField.Name = field.Name;
                    dispositionField.Body = field.Body;
                    dispositionField.Disposition = typeMatch.Value;

                    foreach(Match parameterMatch in parameterMatches)
                    {
                        tmpMatch = Regex.Match(parameterMatch.Value, m_TokenPattern + "=");
                        key = tmpMatch.Value.TrimEnd(new char[] { '=' });
                        tmpMatch = Regex.Match(parameterMatch.Value, "(?<==)" + m_ValuePattern);                    
                        val = tmpMatch.Value.Trim(new char[] { '\\', '"' });
                        dispositionField.Parameters.Add(key, val);
                    }
                    tmpFields.Add(dispositionField);
                }
            }

            foreach (RFC822.Field field in tmpFields)
            {
                fields.Add(field);
            }         
        }

        public override void CompilePattern()
        {
            m_DispositionTypes.Add("inline");
            m_DispositionTypes.Add("Attachment");
            m_DispositionTypes.Add(m_XTokenPattern);

            CompileDispositionTypes();

            m_DispositionType = new Regex("(?i)" + m_DispositionTypesBuilder.ToString() + 
                "(?i)", RegexOptions.Compiled);

            m_Disposition = new Regex("Content-Disposition:\x5C\x73" + m_DispositionType.ToString() +
                "(;\x5C\x73" + m_DispositionParmPattern + ")*");

            base.CompilePattern();
        }

        private void CompileDispositionTypes()
        {
            m_DispositionTypesBuilder = new StringBuilder();
            m_DispositionTypesBuilder.Append("(");
            for (int i = 0; i < m_DispositionTypes.Count; i++)
            {
                m_DispositionTypesBuilder.Append(m_DispositionTypes[i]);
                if (i < (m_DispositionTypes.Count - 1))
                    m_DispositionTypesBuilder.Append("|");
            }
            m_DispositionTypesBuilder.Append(")");
        }
    }
}
