
using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Specialized;

namespace jingxian.mail.mime.RFC2045
{
    public class ContentTypeField : RFC822.Field
    {
        private string m_Type;        
        private string m_SubType;        
        private StringDictionary m_Parameters;

        public ContentTypeField()
        {           
            m_Parameters = new StringDictionary();
        }

        public string Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public string SubType
        {
            get { return m_SubType; }
            set { m_SubType = value; }
        }      
        
        public StringDictionary Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }
    }
}
