
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC2045
{
    internal class MIMERException:Exception
    {
        internal MIMERException(string message, Exception innerException)
            :base(message, innerException)
        { }

        internal MIMERException(string message)
            : base(message)
        { }
    }
}
