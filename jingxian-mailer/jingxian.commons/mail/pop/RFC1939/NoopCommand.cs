
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{

    public class NoopCommand:POPCommand
    {
        public NoopCommand(Stream receiver)
            : base(receiver, "NOOP", string.Empty)
        { }
    }
}
