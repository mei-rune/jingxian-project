
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    /// <summary>
    /// ״̬��ģʽ
    /// </summary>
    public abstract class POPState
    {
        /// <summary>
        /// �ж��ǲ��ǺϷ�����������ִ������
        /// </summary>
        public abstract bool IssueCommand(RFC1939.POPCommand command, POPClient context);

        /// <summary>
        /// �ж��ǲ��ǺϷ�����
        /// </summary>
        public abstract bool IsLegalCommand(RFC1939.POPCommand command);

        public new abstract string ToString();
        
    }
}
