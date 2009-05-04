using System;
using System.Collections.Generic;
using System.Text;

namespace POP
{
    using jingxian.mail.mime;
    using jingxian.mail.popper;

    class Program
    {
        static void client_CommandIssued(object sender, CommandIssuedEventArgs args)
        {
            System.Console.WriteLine(args.Command.Response);
        }

        static void Main(string[] args)
        {
            POPClient client = new POPClient();
            client.Server = "127.0.0.1";
            client.Port = 110;
            client.Password = "111111";
            client.User = "meifakun@betamail.net";

            client.CommandIssued +=new CommandIssuedEventHandler(client_CommandIssued);

            int count = client.CountMessage();

            for (int i = 0; i <= count; i++)
            {
                IMailMessage m = client.FetchMail(i);
                string name = System.Guid.NewGuid().ToString() + ".msg";

                System.IO.File.WriteAllText(name, m.Source);

                System.Console.WriteLine(m.Subject + "\r->\r" + name);
            }

            System.Console.ReadLine();
        }
    }
}
