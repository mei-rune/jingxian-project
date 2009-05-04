
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;

namespace jingxian.mail.popper.RFC1939
{
    public class ListCommand:POPCommand
    {
        protected long m_Octets;

        protected int m_Count;

        protected long[] m_List;

        
        public ListCommand(Stream receiver)
            : this(receiver,  string.Empty)
        { }

        
        public ListCommand(Stream receiver, int message)
            : this(receiver, message.ToString())
        { }

        
        public ListCommand(Stream receiver, string arguments)
            :base(receiver, "LIST", arguments)
        { }

        public override bool Execute()
        {
            try
            {
                m_Request = string.Concat(m_Command,  " ", m_Arguments ,NewLine );
                WriteRequest();
                ReadResponse();
                if (m_Arguments != null && m_Arguments == string.Empty)
                {
                    while (!m_Response.EndsWith( NewLine + "." + NewLine))
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
                pattern = "\r\n[0-9]+ [0-9]+";
            }
            else
            {
                pattern = "[0-9]+ [0-9]+";
            }
            MatchCollection matches = Regex.Matches(m_Response, pattern);
            m_List = new long[matches.Count];
            int i = 0;
            foreach (Match m in matches)
            {
                string[] sar = m.Value.Split(new char[] { ' ' });
                if (sar.Length > 1)
                {
                    m_List[i] = Convert.ToInt64(sar[1]);
                    i++;
                }
            }

            i = 0;

            while (i < m_List.Length)
            {
                m_Octets += m_List[i];
                i++;
            }

            m_Count = m_List.Length;
        }

        public long Octets
        {
            get { return m_Octets; }
        }

        public int Count
        {
            get { return m_Count; }
        }

        public long[] List
        {
            get { return m_List; }
        }
    }
}
