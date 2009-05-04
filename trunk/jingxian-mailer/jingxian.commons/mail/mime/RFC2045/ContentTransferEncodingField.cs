
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC2045
{
    public class ContentTransferEncodingField : RFC822.Field
    {
        public string Encoding
        {
            get { return this.Body; }
            set { this.Body = value; }
        }
    }
}
