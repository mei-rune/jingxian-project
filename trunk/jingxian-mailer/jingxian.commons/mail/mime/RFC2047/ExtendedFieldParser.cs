
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC2047
{
    public class ExtendedFieldParser:RFC822.FieldParser, IFieldParser
    {
        protected readonly string m_EncodingPattern;
        protected readonly string m_CharsetPattern;
        protected readonly string m_EncodedTextPattern;
        protected readonly string m_EncodedWordPattern;

        private Regex m_EncodedWord;
        private Regex m_Charset;
        private Regex m_EncodedText;
        private Regex m_Encoding;

        private Encoding m_TargetEncoding;
        private RFC2045.QuotedPrintableDecoder m_QPDecoder;
        private RFC2045.Base64Decoder m_B64decoder;

        public Encoding TargetEncoding
        {
            get { return m_TargetEncoding; }
            set { m_TargetEncoding = value; }
        }

        public ExtendedFieldParser()
        {
            m_EncodingPattern = "(?i)(?<=\x5C?)(Q|B)(?=\x5C?)";
            m_CharsetPattern = "(?<==\x5C?)" + m_TokenPattern + "(?=\x5C?)";
            m_EncodedTextPattern = "(?<=\x5C?)[^\x3F\x20]+(?=\x5C?=)";
            m_EncodedWordPattern = "=\x5C?" + m_TokenPattern + "\x5C?" + m_TokenPattern + "\x5C?[^\x3F\x20]+\x5C?=";
            m_TargetEncoding = Encoding.UTF8;
            m_QPDecoder = new RFC2045.QuotedPrintableDecoder();
            m_B64decoder = new RFC2045.Base64Decoder();
        }

        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {
            if (!IsCompiled())
                CompilePattern();

            if(fields.Count == 0)
                base.Parse(ref fields, ref fieldString);

            foreach (RFC822.Field field in fields)
            {
                if(m_EncodedWord.IsMatch(field.Body))
                {
                    string charset = m_Charset.Match(field.Body).Value;
                    string text = m_EncodedText.Match(field.Body).Value;
                    string encoding = m_Encoding.Match(field.Body).Value;

                    Encoding enc = Encoding.GetEncoding(charset);
                    byte[] bar = enc.GetBytes(text);
                    bar = Encoding.Convert(enc, m_TargetEncoding, bar);                    
                    text = m_TargetEncoding.GetString(bar);

                    if (encoding.ToLower().Equals("q"))
                    {
                        text = m_QPDecoder.DecodeToString(ref text);
                    }
                    else
                    {
                        bar = m_B64decoder.Decode(ref text);
                        text = m_TargetEncoding.GetString(bar);
                    }

                    field.Body = Regex.Replace(field.Body, 
                        m_EncodedWordPattern, text);
                    field.Body = field.Body.Replace('_', ' ');

                }
            }
      
        }

        public override void CompilePattern()
        {
            m_Charset = new Regex(m_CharsetPattern, RegexOptions.Compiled);
            m_Encoding = new Regex(m_EncodingPattern, RegexOptions.Compiled);
            m_EncodedText = new Regex(m_EncodedTextPattern, RegexOptions.Compiled);
            m_EncodedWord = new Regex(m_EncodedWordPattern, RegexOptions.Compiled);
            base.CompilePattern();
        }

        private bool IsCompiled()
        {
            return (m_EncodedWord != null);
        }
    }
}
