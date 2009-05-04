
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace jingxian.mail.popper.RFC1939
{
    public class ConnectCommand:POPCommand
    {
        private IList<POPCommand> m_Commands;

        public ConnectCommand(Stream receiver, string user, string password)
            : base(receiver, "Connect", string.Empty)
        {
            m_Commands = new List<POPCommand>();
            m_Commands.Add(new UserCommand(receiver, user));
            m_Commands.Add(new PassCommand(receiver, password));            
        }

        public override bool Execute()
        {
            m_Response = string.Empty;
            try
            {  
                foreach (POPCommand command in m_Commands)
                {
                    if (!command.Execute())
                    {
                        m_Response += command.Response;
                        return false;
                    }
                    m_Response += command.Response;
                }
                return true;
            }
            catch (Exception ex)
            {
                ThrowException("不能执行复合的连接命令!", ex);
            }
            return false;
        }
    }
}
