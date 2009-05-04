
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace jingxian.mail.mime
{
    public class FieldParserProxy:RFC2045.ContentTransferEncodingFieldParser, IFieldParser, IFieldParserProxy
    {
        private RFC822.FieldParser m_822Parser;
        private RFC2045.ContentTypeFieldParser m_ContentTypeFieldParser;
        private RFC2045.ContentTransferEncodingFieldParser m_ContentTransferEncodingFieldParser;
        private RFC2183.ContentDispositionFieldParser m_ContentDispositionFieldParser;
        private RFC2047.ExtendedFieldParser m_ExtendedFieldParser;
        
        protected static IFieldParserProxy s_Proxy = null;

        protected FieldParserProxy() 
        {
            m_822Parser = new RFC822.FieldParser();
            m_ContentTypeFieldParser = new RFC2045.ContentTypeFieldParser();
            m_ContentTransferEncodingFieldParser = new RFC2045.ContentTransferEncodingFieldParser();
            m_ContentDispositionFieldParser = new RFC2183.ContentDispositionFieldParser();
            m_ExtendedFieldParser = new RFC2047.ExtendedFieldParser();
        }

        #region Static methods
        public static IFieldParserProxy Getinstance()
        {
            if (s_Proxy == null)
                s_Proxy = new FieldParserProxy();
            return s_Proxy;
        }

        public static string ParseAddress(string data)
        {
            IFieldParserProxy proxy = Getinstance();
            if (proxy.AddrSpec.IsMatch(data))
                return proxy.AddrSpec.Match(data).Value;
            else
                return string.Empty;
        }        

        public static void ParseFields(ref IList<RFC822.Field> fields, ref string fieldString)
        {
            IFieldParserProxy proxy = Getinstance();
            proxy.Parse(ref fields, ref fieldString);
        }

        #endregion

        #region IFieldParser Members

        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {   
            m_822Parser.Parse(ref fields, ref fieldString);
            m_ExtendedFieldParser.Parse(ref fields, ref fieldString);
            m_ContentTypeFieldParser.Parse(ref fields, ref fieldString);
            m_ContentTransferEncodingFieldParser.Parse(ref fields, ref fieldString);
            m_ContentDispositionFieldParser.Parse(ref fields, ref fieldString);            
        }

        public override void CompilePattern()
        {
            m_822Parser.CompilePattern();
            m_ContentTypeFieldParser.CompilePattern();
            m_ContentTransferEncodingFieldParser.CompilePattern();
            m_ContentDispositionFieldParser.CompilePattern();
            m_ExtendedFieldParser.CompilePattern();
        }       

        #endregion

        public new Regex CompositeType
        {
            get { return m_ContentTypeFieldParser.CompositeType; }
        }

        public new Regex DescriteType
        {
            get { return m_ContentTypeFieldParser.DescriteType; }
        }

        public new Regex StartBoundary
        {
            get { return m_ContentTypeFieldParser.StartBoundary; }
        }

        public new Regex EndBoundary
        {
            get { return m_ContentTypeFieldParser.EndBoundary; }
        }

        public new Regex MIMEVersion
        {
            get { return m_ContentTypeFieldParser.MIMEVersion; }
        }

        public new Regex AddrSpec
        {
            get
            {
                return m_822Parser.AddrSpec;
            }
        }
    }
}
