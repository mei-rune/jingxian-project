
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC2045
{
    class MultipartEntity : RFC2045.Entity, IEntity, RFC2045.IMultipartEntity
    {
        protected IList<RFC2045.IEntity> m_BodyParts;

        public MultipartEntity()
        {
            m_BodyParts = new List<RFC2045.IEntity>();
        }

        public IList<RFC2045.IEntity> BodyParts        
        {
            get { return m_BodyParts; }
            set { m_BodyParts = value; }
        }
    }
}
