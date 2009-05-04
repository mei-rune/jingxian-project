
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class GreetingCommand:POPCommand
    {
        public GreetingCommand(Stream receiver)
            : base(receiver, "Greeting", string.Empty) { }

        public override bool Execute()
        {
            ReadResponse();
            if(m_Response.StartsWith(OK))
                return true;
            return false;
        }
    }
}
