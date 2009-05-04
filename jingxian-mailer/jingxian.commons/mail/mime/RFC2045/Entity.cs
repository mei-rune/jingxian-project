
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC2045
{
    class Entity : RFC2045.IEntity
    {
        private byte[] m_Body;        
        
        protected IList<RFC822.Field> m_Fields;        
        private string m_Delimiter;
        private RFC2045.IMultipartEntity m_Parent;        

        public Entity()
        {            
            m_Fields = new List<RFC822.Field>();            
        }

        public byte[] Body
        {
            get { return m_Body; }
            set { m_Body = value; }
        }        

        public IList<RFC822.Field> Fields
        {
            get { return m_Fields; }
            set { m_Fields = value; }
        }

        public string Delimiter
        {
            get { return m_Delimiter; }
            set { m_Delimiter = value; }
        }

        public RFC2045.IMultipartEntity Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }
    }
}
