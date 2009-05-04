
using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC2045
{
    public class QuotedPrintableDecoder : IDecoder
    {
        private static readonly string s_TransportPadding = "[\x5Cs]";
        private static readonly string s_HexOctet = "=[0-9A-F]{2,2}";
        private static readonly string s_SafeChar = "[\x21-\x3C\x3E-\x7E]";
        private static readonly string s_Ptext = "(" + s_HexOctet + "|" + s_SafeChar + ")";
        private static readonly string s_QPsection = "((" + s_Ptext + "|\x20|\x09|)*" + s_Ptext + ")";
        private static readonly string s_QPsegment = s_QPsection + "|\x20|\x09|=";
        private static readonly string s_QPpart = s_QPsegment + "{,76}";
        private static readonly string s_QPline = "(" + s_QPsegment + s_TransportPadding + "\x0D\x0A)*" +
            s_QPpart + s_TransportPadding;
        private static readonly string s_QuotedPrintable = s_QPline + "(\x0D\x0A" + s_QPline + ")*";

        private Regex m_REquotedPrintable = new Regex(s_QuotedPrintable, RegexOptions.Compiled);
        private Regex m_REhexOctet = new Regex(s_HexOctet, RegexOptions.Compiled);

        public bool CanDecode(string encodign)
        {
            if (encodign != null)
            {
                return encodign.ToLower().Equals("quoted-printable") || encodign.ToLower().Equals("7bit");
            }
            return false;
        }

        public byte[] Decode(ref System.IO.Stream dataStream)
        {            
            string coded = ReadData(ref dataStream);
            return Decode(ref coded);   
        }

        private string ReadData(ref System.IO.Stream dataStream)
        {
            int size = 1024;
            int pos = 0;
            char[] buffer = new char[size];

            int c;
            while ((c = dataStream.ReadByte()) != -1)
            {
                if (pos >= size)
                {
                    size = size * 2;
                    char[] tmpBuffer = new char[size];
                    buffer.CopyTo(tmpBuffer, 0);
                    buffer = null;
                    buffer = tmpBuffer;
                }
                buffer[pos] = (char)c;

                if (pos >= 3)
                {
                    char bBeforePrevious = buffer[pos - 3];
                    char beforePrevious = buffer[pos - 2];
                    char previous = buffer[pos - 1];
                    char current = buffer[pos];

                    //TODO: 检测 Delimiterline 可能更好
                    // 一空行，也就是两个"\r\n"
                    if (bBeforePrevious == 13 && beforePrevious == 10 &&
                        previous == 13 && current == 10)
                    {
                        break;
                    }
                }
                pos++;
            }

            char[] data = new char[pos + 1];
            Array.Copy(buffer, data, pos);
            buffer = null;
            return new string(data);
        }

        public byte[] Decode(ref string data)
        {
            return System.Text.Encoding.UTF8.GetBytes(DecodeToString(ref data));            
        }

        public string DecodeToString(ref string data)
        {
            string decoded = String.Empty;
            MatchCollection matches = m_REquotedPrintable.Matches(data);
            MatchCollection hexMatches = m_REhexOctet.Matches(data);

            foreach (Match match in matches)
            {
                decoded += match.Value;
            }

            while (m_REhexOctet.IsMatch(decoded))
            {
                Match match = m_REhexOctet.Match(decoded);
                string hex = match.Value.TrimStart(new char[] { '=' });
                int number = Convert.ToInt32(hex, 16);
                char c = (char)number;
                decoded = Regex.Replace(decoded, match.Value, c.ToString());
            }
            return decoded;
        }
    }
}
