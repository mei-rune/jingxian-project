
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// ������״̬�µ�����ִ�л���
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
        /// ִ�����������������¼�����ִ��
        /// </summary>
        /// <returns>�������true,��˵������ִ�гɹ�</returns>
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
        /// �ж��ǲ������¼����ص�����
        /// </summary>
        /// <returns>�������true,��˵�����������¼����ص�����</returns>
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
