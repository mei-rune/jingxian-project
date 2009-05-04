
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC2045
{
    public class Base64Decoder:IDecoder
    {
        private Regex m_NonBase64 = new Regex(s_NonBase64, RegexOptions.Compiled);

        public static readonly string s_NonBase64 = "[^A-Za-z0-9+/=]";
        
        public bool CanDecode(string encodign)
        {
            if (encodign != null)
            {
                return encodign.ToLower().Equals("base64");
            }
            return false;
        }

        public byte[] Decode(ref System.IO.Stream dataStream)
        {
            // TODO: implement
            throw new NotImplementedException();
        }

        public byte[] Decode(ref string data)
        {            
            data = m_NonBase64.Replace(data, string.Empty);            
            return Convert.FromBase64String(data);
        }
    }
}
