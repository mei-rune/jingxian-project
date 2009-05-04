
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC822
{

    public class FieldParser:IFieldParser
    {
        protected readonly string m_TSpecialsPattern;
        protected readonly string m_TokenPattern;
        protected readonly string m_QuotedPairPattern;
        protected readonly string m_DtextPattern;
        protected readonly string m_AtomPattern;
        protected readonly string m_UnfoldPattern;
        protected readonly string m_FieldPattern;
        protected readonly string m_FieldNamePattern;
        protected readonly string m_FieldBodyPattern;
        protected readonly string m_QuotedStringPattern;
        protected readonly string m_CtextPattern;

        protected readonly string m_CommentPattern;
        protected readonly string m_WordPattern;
        protected readonly string m_LocalPartPattern;
        protected readonly string m_DomainRefPattern;
        protected readonly string m_SubDomainPattern;
        protected readonly string m_DomainPattern;
        protected readonly string m_AddrSpecPattern;
        protected readonly string m_DomainLiteralPattern;

        protected readonly string m_HourPattern;
        protected readonly string m_ZonePattern;
        protected readonly string m_MonthPattern;
        protected readonly string m_DayPattern;
        protected readonly string m_TimePattern;
        protected readonly string m_DatePattern;
        protected readonly string m_DateTimePattern;
        
        protected Regex m_Unfold;      
        protected Regex m_Field;
        protected Regex m_HeaderName;
        protected Regex m_HeaderBody;
        private Regex m_AddrSpec;

        
        public Regex AddrSpec
        {
            get { return m_AddrSpec; }
            set { m_AddrSpec = value; }
        }

        public Regex Field
        {
            get { return m_Field; }
            set { m_Field = value; }
        }        

        public Regex HeaderName
        {
            get { return m_HeaderName; }
            set { m_HeaderName = value; }
        }

        public FieldParser()
        {
            m_TSpecialsPattern = "][()<>@,;\x5C\x5C:\x22/?=";
            m_TokenPattern = "[^" + m_TSpecialsPattern + "\x00-\x20]+";
            m_QuotedPairPattern = "\x5C\x5C[\x00-\x7F]";
            m_DtextPattern = "[^]\x0D\x5B\x5C\x5C\x80-\xFF]";
            m_AtomPattern = "[^][()<>@,;:.\x5C\x5C\x22\x00-\x20\x7F]+";
            m_UnfoldPattern = "\x0D\x0A\x5Cs";
            m_FieldPattern = "[^\x00-\x20\x7F:]{1,}:{1,1}.+";
            m_FieldNamePattern = "[^\x00-\x20\x7F:]{1,}(?=:)";
            m_FieldBodyPattern = "(?<=[^\x00-\x20\x7F:]{1,}: )[^\xA\xD]+";
            m_QuotedStringPattern = "\x22(?:(?:(?:\x5C\x5C{2})+|\x5C\x5C[^\x5C\x5C]|[^\x5C\x5C\x22])*)\x22";        
            m_CtextPattern = "[^()\x5C\x5C]+";

            m_HourPattern = "[0-9]{2,2}:[0-9]{2,2}(:[0-9]{2,2})*";
            m_ZonePattern = "(UT|GMT|EST|EDT|CST|CDT|MST|MDT|PST|PDT|1ALPHA|[+-]{1,1}[0-9]{4,4})";
            m_MonthPattern = "(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)";
            m_DayPattern = "(Mon|Tue|Wed|Thu|Fri|Sat|Sun)";
            m_TimePattern = m_HourPattern + m_ZonePattern;
            m_DatePattern = "([0-9]{2,2}\x5C\x73" + m_MonthPattern + "\x5C\x73[0-9]{2,4}){1,1}";
            m_DateTimePattern = "(" + m_DayPattern + ", )" + m_DatePattern + " " + m_TimePattern;

            m_CommentPattern = "(?:[(]{1,1} *(" + m_CtextPattern + "|" + m_QuotedPairPattern + ")[)]{1,1})";
            m_WordPattern = "(" + m_AtomPattern + "|" + m_QuotedStringPattern + ")";
            m_LocalPartPattern = "(" + m_WordPattern + "(?:\x5C\x2E" + m_WordPattern + ")*)";
            m_DomainRefPattern = m_AtomPattern;
            m_DomainLiteralPattern = "(\x5C\x5B(?:" + m_DtextPattern + "|" + m_QuotedPairPattern + ")*\x5C\x5D)";
            m_SubDomainPattern = "(?:(" + m_DomainRefPattern + "|" + m_DomainLiteralPattern + "))";
            m_DomainPattern = "(" + m_SubDomainPattern + "(?:\x5C\x2E" + m_SubDomainPattern + ")*)";
            m_AddrSpecPattern = "(" + m_LocalPartPattern + "@" + m_DomainPattern + ")";            

            m_Unfold = new Regex(m_UnfoldPattern, RegexOptions.Compiled);
            m_Field = new Regex(m_FieldPattern, RegexOptions.Compiled);
            m_HeaderName = new Regex(m_FieldNamePattern, RegexOptions.Compiled);
            m_HeaderBody = new Regex(m_FieldBodyPattern, RegexOptions.Compiled);
            m_AddrSpec = new Regex(m_AddrSpecPattern, RegexOptions.Compiled);
        }

        public virtual void CompilePattern()
        {
            
        }

        public string Unfold(string data)
        {
            return m_Unfold.Replace(data, " ");
        }

        public virtual void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {   
            MatchCollection matches = m_Field.Matches(fieldString);
            foreach (Match m in matches)
            {
                RFC822.Field f = new RFC822.Field();
                Match tmp = m_HeaderName.Match(m.Value);
                f.Name = tmp.Value;
                tmp = m_HeaderBody.Match(m.Value);
                f.Body = tmp.Value;          
                fields.Add(f);
            }            
        }
    }
}
