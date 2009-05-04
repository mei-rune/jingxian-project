
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// 处理登录状态下的命令执行机制
    /// </summary>
    public class AuthorizationState:POPState
    {
        private static AuthorizationState s_State = null;

        public static AuthorizationState GetInstance()
        {
            if (s_State == null)
                s_State = new AuthorizationState();
            return s_State;
        }

        public override bool IssueCommand(RFC1939.POPCommand command, POPClient context)
        {
            if (!IsLegalCommand(command))
                return false;

            if (command is RFC1939.ConnectCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    context.CurrentState = TransactionState.GetInstance();
                    return true;
                }
                context.LoggCommand(command);
            }
            else if (command is RFC1939.UserCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    return true;
                }
                context.LoggCommand(command);
            }
            else if (command is RFC1939.PassCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    context.CurrentState = TransactionState.GetInstance();
                    return true;
                }
                context.LoggCommand(command);
            }
            else if (command is RFC1939.QuitCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    context.CurrentState = DisconnectedState.GetInstane();
                    return true;
                }
                context.LoggCommand(command);
            }

            return false;
        }

        public override bool IsLegalCommand(jingxian.mail.popper.RFC1939.POPCommand command)
        {
            if (command is RFC1939.DeleteCommand)
                return false;
            if (command is RFC1939.ListCommand)
                return false;
            if (command is RFC1939.NoopCommand)
                return false;            
            if (command is RFC1939.ResetCommand)
                return false;
            if (command is RFC1939.RetriveCommand)
                return false;
            if (command is RFC1939.StatCommand)
                return false;
            if (command is RFC1939.TopCommand)
                return false;
            if (command is RFC1939.UIDLCommand)
                return false;

            return true;
        }

        public override string ToString()
        {
            return "AUTHORIZATION";
        }
    }
}
