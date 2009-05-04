
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class DeleteCommand:POPCommand
    {
        public DeleteCommand(Stream receiver, int message)
            : base(receiver,"DELE", message.ToString())
        { }
    }
}
