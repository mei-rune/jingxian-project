
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class RetriveCommand:POPCommand
    {
        public RetriveCommand(Stream receiver, int message)
            : base(receiver, "RETR", message.ToString())
        { }
    }
}
