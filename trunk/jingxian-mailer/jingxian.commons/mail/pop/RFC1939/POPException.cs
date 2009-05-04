using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper.RFC1939
{

    public class POPException:Exception
    {
        public POPException(string message):base(message)
        {}

        public POPException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
