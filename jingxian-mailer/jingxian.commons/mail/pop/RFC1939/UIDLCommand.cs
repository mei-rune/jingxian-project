
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace jingxian.mail.popper.RFC1939
{
    public class UIDLCommand:POPCommand
    {
        protected StringCollection m_UIDS;

        public StringCollection UIDS
        {
            get { return m_UIDS; }
            set { m_UIDS = value; }
        }

        public UIDLCommand(Stream receiver) 
            : base(receiver, "UIDL", string.Empty) { }

        public UIDLCommand(Stream receiver, int message)
            : base(receiver, "UIDL", message.ToString())
        { }

        public override bool Execute()
        {
            try
            {
                m_Request = string.Concat( m_Command, " " , m_Arguments , NewLine );
                WriteRequest();
                ReadResponse();
                if ( m_Arguments != null && m_Arguments == string.Empty)
                {
                    while (!m_Response.EndsWith(NewLine + "." + NewLine))
                    {
                        ReadResponse();
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException( ex);
            }
            ProcessResponse();
            return m_Response.StartsWith(OK);            
        }

        protected virtual void ProcessResponse()
        {
            if (String.IsNullOrEmpty(m_Response))
                return;

            string pattern;
            if (m_Arguments == string.Empty)
            {
                pattern = "\r\n[0-9]+ [\x5Cw]+";
            }
            else
            {
                pattern = "[0-9]+ [\x5Cw]+";
            }
            MatchCollection matches = Regex.Matches(m_Response, pattern);
            m_UIDS = new StringCollection();
            int i = 0;
            foreach (Match m in matches)
            {
                string[] sar = m.Value.Split(new char[] { ' ' });
                if (sar.Length > 1)
                {
                    m_UIDS.Add(sar[1]);
                    i++;
                }
            }
        }
    }
}
