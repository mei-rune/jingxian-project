
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// 处理传输状态下的命令执行机制
    /// </summary>
    public class TransactionState : POPState
    {
        private static TransactionState s_State = null;

        public static TransactionState GetInstance()
        {
            if (s_State == null)
                s_State = new TransactionState();
            return s_State;
        }


        /// <summary>
        /// 执行命令，如果命令是与登录相关则不执行
        /// </summary>
        /// <returns>如果返回true,则说明命令执行成功</returns>
        public override bool IssueCommand(RFC1939.POPCommand command, POPClient context)
        {
            if (!IsLegalCommand(command))
                return false;

            if (command is RFC1939.QuitCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    context.CurrentState = DisconnectedState.GetInstane();
                    return true;
                }
                context.CurrentState = DisconnectedState.GetInstane();
                context.LoggCommand(command);
            }
            else if (command is RFC1939.RetriveCommand || command is RFC1939.TopCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    context.CurrentState = ReadingState.GetInstance();
                    return true;
                }
                context.LoggCommand(command);
            }
            else if (command is RFC1939.NoopCommand)
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    return true;
                }
                else
                {
                    context.LoggCommand(command);
                    context.Disconnect();
                    context.CurrentState = DisconnectedState.GetInstane();
                }
            }
            else
            {
                if (command.Execute())
                {
                    context.LoggCommand(command);
                    return true;
                }
                context.LoggCommand(command);
            }
            return false;
        }

        /// <summary>
        /// 判断是不是与登录不相关的命令
        /// </summary>
        /// <returns>如果返回true,则说明命令是与登录不相关的命令</returns>
        public override bool IsLegalCommand(RFC1939.POPCommand command)
        {
            if (command is RFC1939.UserCommand)
                return false;
            if (command is RFC1939.PassCommand)
                return false;
            if (command is RFC1939.ApopCommand)
                return false;

            return true;
        }

        public override string ToString()
        {
            return "TRANSACTION";
        }
    }
}
