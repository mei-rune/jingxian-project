
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class ResetCommand:POPCommand
    {
        public ResetCommand(Stream receiver)
            : base(receiver,"RSET", string.Empty)
        { }
    }
}
