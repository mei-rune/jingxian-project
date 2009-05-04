
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC2045
{
    public class ContentTypeFieldParser:RFC822.FieldParser, IFieldParser
    {        
        protected readonly string m_ValuePattern;
        protected readonly string m_ParameterPattern;
        protected readonly string m_XTokenPattern;
        protected readonly string m_ExtensionTokenPattern;
        protected readonly string m_BoundaryStartDelimiterPattern;
        protected readonly string m_BoundaryEndDelimiterPattern;
        protected readonly string m_MIMEVersionPattern;

        private Regex m_Content;
        private Regex m_Type;
        private Regex m_SubType;
        private Regex m_Parameters;
        private Regex m_CompositeType;
        private Regex m_DescriteType;
        private Regex m_StartBoundary;
        private Regex m_EndBoundary;
        private Regex m_MIMEVersion;

        public Regex MIMEVersion
        {
            get { return m_MIMEVersion; }
            set { m_MIMEVersion = value; }
        }
        public Regex StartBoundary
        {
            get { return m_StartBoundary; }
            set { m_StartBoundary = value; }
        }        
        public Regex EndBoundary
        {
            get { return m_EndBoundary; }
            set { m_EndBoundary = value; }
        }
        public Regex DescriteType
        {
            get { return m_DescriteType; }
            set { m_DescriteType = value; }
        }
        public Regex CompositeType
        {
            get { return m_CompositeType; }
            set { m_CompositeType = value; }
        }
        
        private StringBuilder m_DiscreteBuilder;
        private StringBuilder m_CompositeBuilder;
        private StringBuilder m_MultipartSubtypesBuilder;
        private StringBuilder m_TextSubtypesBuilder;
        private StringBuilder m_ImageSubtypesBuilder;
        private StringBuilder m_ApplicationSubtypesBuilder;
        private StringBuilder m_MessageSubtypesBuilder;
        private StringBuilder m_AudioSubtypesBuilder;

        protected IList<string> m_DiscreteTypes;
        protected IList<string> m_CompositeTypes;
        protected IList<string> m_MultipartSubTypes;
        protected IList<string> m_TextSubtypes;
        protected IList<string> m_ImageSubTypes;
        protected IList<string> m_ApplicationSubtypes;
        protected IList<string> m_MessageSubtypes;
        protected IList<string> m_AudioSubtypes;

        private bool m_StrictMatch = false;

        public bool StrictMatch
        {
            get { return m_StrictMatch; }
            set { m_StrictMatch = value; }
        }

        public ContentTypeFieldParser() 
        {
            m_BoundaryStartDelimiterPattern = "--.{1,70}\x0D\x0A";
            m_BoundaryEndDelimiterPattern = "--.{1,68}--\x0D\x0A"; 
            m_XTokenPattern = "(X-|x-)" + m_TokenPattern;
            m_ExtensionTokenPattern = m_XTokenPattern; // or ietf-token
            m_ValuePattern = "(" + m_TokenPattern + "|" + m_QuotedStringPattern + ")";
            m_ParameterPattern = m_TokenPattern + "=" + m_ValuePattern;

            m_MIMEVersionPattern = "MIME-Version:([0-9]{1,1}|\\x2E{1,1}|" + m_CommentPattern + ")*";

            m_DiscreteTypes = new List<string>();
            m_CompositeTypes = new List<string>();
            m_MultipartSubTypes = new List<string>();
            m_TextSubtypes = new List<string>();
            m_ImageSubTypes = new List<string>();
            m_ApplicationSubtypes = new List<string>();
            m_MessageSubtypes = new List<string>();
            m_AudioSubtypes = new List<string>();
        }

        public override void CompilePattern()
        {
            m_DiscreteTypes.Add("text");
            m_DiscreteTypes.Add("image");
            m_DiscreteTypes.Add("audio");
            m_DiscreteTypes.Add("video");
            m_DiscreteTypes.Add("application");

            m_CompositeTypes.Add("message");
            m_CompositeTypes.Add("multipart");

            m_MultipartSubTypes.Add("mixed");
            m_MultipartSubTypes.Add("alternative");
            m_MultipartSubTypes.Add("parallel");
            m_MultipartSubTypes.Add("digest");
            m_MultipartSubTypes.Add("related");

            m_TextSubtypes.Add("plain");
            m_TextSubtypes.Add("enriched");
            m_TextSubtypes.Add("html");

            m_ImageSubTypes.Add("jpeg");
            m_ImageSubTypes.Add("gif");

            m_ApplicationSubtypes.Add("octet-stream");
            m_ApplicationSubtypes.Add("PostScript");//TODO: add the rest se rfc 2046
            m_ApplicationSubtypes.Add("pdf");

            m_MessageSubtypes.Add("rfc822");
            m_MessageSubtypes.Add("partial");
            m_MessageSubtypes.Add("external-body");

            m_AudioSubtypes.Add("mpeg");
            
            CompileDescreteTypes();
            CompileCompositeTypes();
            CompileMultipartSubtypes();
            CompileTextSubtypes();
            CompileImageSubtypes();
            CompileApplicationSubtypes();
            CompileMessageSubtypes();
            CompileAudioSubtypes();

            m_StartBoundary = new Regex(m_BoundaryStartDelimiterPattern, RegexOptions.Compiled);

            m_EndBoundary = new Regex(m_BoundaryEndDelimiterPattern, RegexOptions.Compiled);

            m_Parameters = new Regex(m_ParameterPattern, RegexOptions.Compiled);

            m_CompositeType = new Regex("(" + m_CompositeBuilder.ToString() + ")", RegexOptions.Compiled);

            m_DescriteType = new Regex("(" + m_DiscreteBuilder.ToString() + ")", RegexOptions.Compiled);

            m_Type = new Regex("(" + m_DescriteType.ToString() + "|" + m_CompositeType.ToString() + ")", 
                RegexOptions.Compiled);

            m_MIMEVersion = new Regex(m_MIMEVersionPattern, RegexOptions.Compiled);

            m_SubType = new Regex("((?<=multipart/)" + m_MultipartSubtypesBuilder.ToString() + "|" +
            "(?<=text/)" + m_TextSubtypesBuilder.ToString() + "|" + "(?<=image/)" + m_ImageSubtypesBuilder.ToString() + "|" +
            "(?<=application/)" + m_ApplicationSubtypesBuilder.ToString() + "|" + "(?<=message/)" + 
            m_MessageSubtypesBuilder.ToString() + "|" + "(?<=audio/)" + m_AudioSubtypesBuilder.ToString() + ")", 
            RegexOptions.Compiled); //TODO: continue with the rest of subtypes

            string content = "(?i)Content-Type(?i):[\x5C\x73]{1,1}" + m_Type.ToString() + "/";

            if (m_StrictMatch)
            {
                content += m_SubType.ToString();
            }
            else
            {
                content += "(" + m_ExtensionTokenPattern + "|" + m_TokenPattern + ")"; // |IanaToken??
            }
            content += "(;[\x5Cs]+" + m_Parameters.ToString() + ")*";

            m_Content = new Regex(content, RegexOptions.Compiled);                  

            // This should be called if we want to add functionality in this method
            // but let base build/compile it
            base.CompilePattern();
        }

        public override void Parse(ref IList<RFC822.Field> fields, ref string fieldString)
        {
            if (!IsPatternCompiled())
                CompilePattern();

            if (fields.Count == 0)
                base.Parse(ref fields, ref fieldString);

            MatchCollection matches = m_Content.Matches(fieldString);

            foreach (Match match in matches)
            {
                ContentTypeField tmpContent = new ContentTypeField();
                tmpContent.Name = "Content-Type";
                MatchCollection parameters;
                string key, val;

                Match tmpMatch = Regex.Match(match.Value, ":.+");

                tmpContent.Body = tmpMatch.Value.TrimStart(new char[] { ':' });
                tmpMatch = m_Type.Match(match.Value);
                tmpContent.Type = tmpMatch.Value;
                tmpMatch = m_SubType.Match(match.Value);
                tmpContent.SubType = tmpMatch.Value;
                parameters = m_Parameters.Matches(match.Value);
                foreach (Match m in parameters)
                {
                    tmpMatch = Regex.Match(m.Value, m_TokenPattern + "=");
                    key = tmpMatch.Value.TrimEnd(new char[] { '=' });
                    tmpMatch = Regex.Match(m.Value, "(?<==)" + m_ValuePattern);                    
                    val = tmpMatch.Value.Trim(new char[] { '\\', '"' });
                    tmpContent.Parameters.Add(key, val);
                }

                fields.Add(tmpContent);
            }
        }

        private bool IsPatternCompiled()
        {
            return (m_Content != null && m_Type != null && m_SubType != null &&
                m_Parameters != null);
        }
        private void CompileDescreteTypes()
        {
            m_DiscreteBuilder = new StringBuilder("(");
            for (int i = 0; i < m_DiscreteTypes.Count; i++)
            {
                m_DiscreteBuilder.Append(m_DiscreteTypes[i]);
                if (i < (m_DiscreteTypes.Count - 1))
                    m_DiscreteBuilder.Append("|");
            }
            m_DiscreteBuilder.Append(")");
        }
        private void CompileCompositeTypes()
        {
            m_CompositeBuilder = new StringBuilder("(");
            for (int i = 0; i < m_CompositeTypes.Count; i++)
            {
                m_CompositeBuilder.Append(m_CompositeTypes[i]);
                if (i < (m_CompositeTypes.Count - 1))
                    m_CompositeBuilder.Append("|");
            }
            m_CompositeBuilder.Append(")");
        }
        private void CompileMultipartSubtypes()
        {
            m_MultipartSubtypesBuilder = new StringBuilder("(");
            for (int i = 0; i < m_MultipartSubTypes.Count; i++)
            {
                m_MultipartSubtypesBuilder.Append(m_MultipartSubTypes[i]);
                if (i < (m_MultipartSubTypes.Count - 1))
                    m_MultipartSubtypesBuilder.Append("|");
            }
            m_MultipartSubtypesBuilder.Append(")");
        }
        private void CompileTextSubtypes()
        {
            m_TextSubtypesBuilder = new StringBuilder("(");
            for (int i = 0; i < m_TextSubtypes.Count; i++)
            {
                m_TextSubtypesBuilder.Append(m_TextSubtypes[i]);
                if (i < (m_TextSubtypes.Count - 1))
                    m_TextSubtypesBuilder.Append("|");
            }
            m_TextSubtypesBuilder.Append(")");
        }
        private void CompileImageSubtypes()
        {            
            m_ImageSubtypesBuilder = new StringBuilder("(?i)(");
            for (int i = 0; i < m_ImageSubTypes.Count; i++)
            {
                m_ImageSubtypesBuilder.Append(m_ImageSubTypes[i]);
                if (i < (m_ImageSubTypes.Count - 1))
                    m_ImageSubtypesBuilder.Append("|");
            }
            m_ImageSubtypesBuilder.Append(")(?i)");
            
        }
        private void CompileApplicationSubtypes()
        {
            m_ApplicationSubtypesBuilder = new StringBuilder("(");
            for (int i = 0; i < m_ApplicationSubtypes.Count; i++)
            {
                m_ApplicationSubtypesBuilder.Append(m_ApplicationSubtypes[i]);
                if (i < (m_ApplicationSubtypes.Count - 1))
                    m_ApplicationSubtypesBuilder.Append("|");
            }
            m_ApplicationSubtypesBuilder.Append(")");

        }
        private void CompileMessageSubtypes()
        {
            m_MessageSubtypesBuilder = new StringBuilder("(");
            for (int i = 0; i < m_MessageSubtypes.Count; i++)
            {
                m_MessageSubtypesBuilder.Append(m_MessageSubtypes[i]);
                if (i < (m_MessageSubtypes.Count - 1))
                    m_MessageSubtypesBuilder.Append("|");
            }
            m_MessageSubtypesBuilder.Append(")");

        }
        private void CompileAudioSubtypes()
        {
            m_AudioSubtypesBuilder = new StringBuilder("(");
            for (int i = 0; i < m_AudioSubtypes.Count; i++)
            {
                m_AudioSubtypesBuilder.Append(m_AudioSubtypes[i]);
                if (i < (m_AudioSubtypes.Count - 1))
                    m_AudioSubtypesBuilder.Append("|");
            }
            m_AudioSubtypesBuilder.Append(")");
        }
        
    }
}
