using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class StatCommand:POPCommand
    {
        public StatCommand(Stream receiver)
            : base(receiver,"STAT", string.Empty)
        { }

        public int Count
        {
            get 
            {
                if (m_Response != null)
                    return ParseCount();
                else
                    return 0;
            }
        }

        public int Octets
        {
            get 
            {
                if (m_Response != null)
                    return ParseOctets();
                else
                    return 0;
            }
        }

        private int ParseCount()
        {
            string[] splits = m_Response.Split(new char[] { ' ' });
            return Convert.ToInt32(splits[1]);
        }

        private int ParseOctets()
        {
            string[] splits = m_Response.Split(new char[] { ' ' });
            return Convert.ToInt32(splits[2]);
        }

    }
}
