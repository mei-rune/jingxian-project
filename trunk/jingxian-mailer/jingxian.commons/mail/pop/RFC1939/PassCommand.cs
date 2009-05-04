

using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class PassCommand:POPCommand
    {
        public PassCommand(Stream receiver, string password)
            : base(receiver,"PASS", password)
        { }
    }
}
