
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// 处理读状态下的命令执行机制
    /// </summary>
    class ReadingState:POPState
    {
        private static ReadingState s_State = null;

        public static ReadingState GetInstance()
        {
            if(s_State == null)
                s_State = new ReadingState();
            return s_State;
        }

        public override bool IssueCommand(RFC1939.POPCommand command, POPClient context)
        {
            return IsLegalCommand(command);
        }

        public override bool IsLegalCommand(RFC1939.POPCommand command)
        {
            return false;
        }

        public override string ToString()
        {
            return "READING";
        }
    }
}
