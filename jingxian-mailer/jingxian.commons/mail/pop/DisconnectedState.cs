
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// 处理断开状态下的命令执行机制
    /// </summary>
    class DisconnectedState:POPState
    {
        private static DisconnectedState s_State = null;

        public static DisconnectedState GetInstane()
        {
            if (s_State == null)
                s_State = new DisconnectedState();
            return s_State;
        }

        public override bool IssueCommand(RFC1939.POPCommand command, POPClient context)
        {
            if (IsLegalCommand(command))
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    context.CurrentState = AuthorizationState.GetInstance();
                    return true;
                }
            }
            return false;
        }

        public override bool IsLegalCommand(RFC1939.POPCommand command)
        {
            if (command is RFC1939.GreetingCommand)
                return true;
            return false;
        }

        public override string ToString()
        {
            return "DISCONNECTED";
        }
    }
}
