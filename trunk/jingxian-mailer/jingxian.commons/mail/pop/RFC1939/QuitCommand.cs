
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class QuitCommand:POPCommand
    {
        public QuitCommand(Stream receiver)
            : base(receiver, "QUIT", string.Empty)
        { }
    }
}
