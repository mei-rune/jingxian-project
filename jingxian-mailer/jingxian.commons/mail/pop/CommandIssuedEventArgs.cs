using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    public delegate void CommandIssuedEventHandler(object sender, CommandIssuedEventArgs args);

    public class CommandIssuedEventArgs:EventArgs
    {
        private RFC1939.POPCommand m_Command;

        public RFC1939.POPCommand Command
        {
            get { return m_Command; }
            internal set { m_Command = value; }
        }

        public CommandIssuedEventArgs(RFC1939.POPCommand command)
        {
            m_Command = command;
        }
    }
}
