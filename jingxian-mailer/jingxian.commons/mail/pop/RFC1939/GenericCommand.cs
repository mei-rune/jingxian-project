
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class GenericCommand:POPCommand
    {
        public GenericCommand(Stream receiver, string command, string arguments):
            base(receiver, command, arguments)
        {}
    }
}
