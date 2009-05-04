
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace jingxian.mail.popper.RFC1939
{
    public class ApopCommand:POPCommand
    {
        private string m_User;
        private string m_Secret;

        private GenericCommand m_GreatingCommand;
        private Regex m_TimeStampRegex;        

        public ApopCommand(Stream receiver, string user, string secret)
            :base(receiver, "APOP", user)
        {
            m_User = user;
            m_Secret = secret;
            m_GreatingCommand = new GenericCommand(receiver, string.Empty, string.Empty);
            m_TimeStampRegex = new Regex("<.*>", RegexOptions.Compiled);            
        }

        public override bool Execute()
        {
            try
            {
                if (m_GreatingCommand.Execute())
                {                    
                    string hash = ComputeHash();
                    m_Request = "APOP " + m_User + " " + hash + Environment.NewLine;
                    WriteRequest();
                    ReadResponse();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ThrowException( ex);
            }

            return m_Response.StartsWith(OK);
        }        

        private string ComputeHash()
        {
            Match match = m_TimeStampRegex.Match(m_GreatingCommand.Response);
            byte[] tmpar = System.Text.Encoding.ASCII.GetBytes(match.Value + m_Secret);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash(tmpar);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
