
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.mime.RFC2045
{
    public class Attachment:IAttachment
    {
        string m_Type;
        string m_SubType;
        string m_Disposition;
        string m_Name;
        byte[] m_Data;

        public string Disposition
        {
            get { return m_Disposition; }
            set { m_Disposition = value; }
        }

        public string SubType
        {
            get { return m_SubType; }
            set { m_SubType = value; }
        }

        public string Type
        {
            get{return m_Type;}
            set{ m_Type = value;}
        }

        public string Name
        {
            get{ return m_Name;}
            set{ m_Name = value;}
        }

        public byte[] Data
        {
            get{ return m_Data;}
            set{ m_Data = value;}
        }

        public Stream GetContentStream()
        {
            return new MemoryStream(m_Data);
        }
    }
}
