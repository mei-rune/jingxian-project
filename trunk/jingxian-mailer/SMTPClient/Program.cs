using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;

namespace SMTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            MailMessage msg = new MailMessage("meifakun@betamail.net", "meifakun@betamail.net");
            msg.Body = "aaaaaaaaa";
            msg.Subject = "ccc";

            Attachment att = new Attachment("D:/projects/BTNM/nope2.zip", new ContentType(MediaTypeNames.Application.Zip));
            att.ContentDisposition.Inline = false;
            att.ContentDisposition.FileName = "nope.zip";
            att.ContentDisposition.CreationDate = DateTime.Now;
            
            msg.Attachments.Add(att);

            SmtpClient client = new SmtpClient( "127.0.0.1" );

            client.Send(msg);

        }
    }
}
