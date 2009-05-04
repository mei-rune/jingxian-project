
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace jingxian.mail.mime
{
    /// <summary>
    /// ��Ϣ�ķ�װ�ӿ�
    /// </summary>
    public interface IMailMessage
    {
        MailAddress From { get; set; }

        MailAddressCollection To { get; set; }

        // ����CC�ֶ�
        MailAddressCollection CarbonCopy { get; set; }

        // ����BCC�ֶ�
        MailAddressCollection BlindCarbonCopy { get; set; }

        String Subject { get; set; }

        string Source { get; set; }

        string TextMessage { get; set; }

        bool IsNull();
    }
}
