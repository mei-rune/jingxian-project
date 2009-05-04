
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace jingxian.mail.mime
{
    /// <summary>
    /// 消息的封装接口
    /// </summary>
    public interface IMailMessage
    {
        MailAddress From { get; set; }

        MailAddressCollection To { get; set; }

        // 就是CC字段
        MailAddressCollection CarbonCopy { get; set; }

        // 就是BCC字段
        MailAddressCollection BlindCarbonCopy { get; set; }

        String Subject { get; set; }

        string Source { get; set; }

        string TextMessage { get; set; }

        bool IsNull();
    }
}
