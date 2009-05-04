
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class TopCommand:POPCommand
    {
        public TopCommand(Stream receiver, int message, int lines):
            base(receiver, "TOP", message.ToString() + " " + lines.ToString())
        { }
    }
}
