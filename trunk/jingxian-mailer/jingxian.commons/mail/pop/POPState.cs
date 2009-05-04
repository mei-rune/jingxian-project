
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// 状态机模式
    /// </summary>
    public abstract class POPState
    {
        /// <summary>
        /// 判断是不是合法命令，如果是则执行命令
        /// </summary>
        public abstract bool IssueCommand(RFC1939.POPCommand command, POPClient context);

        /// <summary>
        /// 判断是不是合法命令
        /// </summary>
        public abstract bool IsLegalCommand(RFC1939.POPCommand command);

        public new abstract string ToString();
        
    }
}
