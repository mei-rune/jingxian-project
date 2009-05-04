
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace jingxian.mail.mime.RFC2183
{
    public class ContentDispositionField:RFC822.Field
    {
        private StringDictionary m_Parameters;
        private string m_Disposition;

        internal ContentDispositionField()
        {
            m_Parameters = new StringDictionary();
        }

        public StringDictionary Parameters
        {
            get { return m_Parameters; }
            set { m_Parameters = value; }
        }

        public string Disposition
        {
            get { return this.m_Disposition; }
            set { this.m_Disposition = value; }
        }
    }
}
