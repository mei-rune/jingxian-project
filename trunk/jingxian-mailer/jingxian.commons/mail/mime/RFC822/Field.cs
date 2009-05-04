
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC822
{
    public class Field
    {
        protected string m_Name;
        protected string m_Body;

        public Field()
        { }

        public Field(string nm, string body)
        {
            m_Name = nm;
            m_Body = body;
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }        
      
    }
}
