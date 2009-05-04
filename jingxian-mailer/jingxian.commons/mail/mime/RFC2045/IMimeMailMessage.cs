
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net.Mail;

namespace jingxian.mail.mime.RFC2045
{
    public interface IMimeMailMessage : IMailMessage
    {
        IDictionary<string, string> Body { get; set; }

        IList<IAttachment> Attachments { get; set; }

        IList<IMimeMailMessage> Messages { get; set; }

        IList<AlternateView> Views { get; set; }

        MailMessage ToMailMessage();
    }
}
