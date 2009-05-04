
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class UserCommand:POPCommand
    {
        public UserCommand(Stream receiver, string user)
            : base(receiver,"user", user)
        { }
    }
}
