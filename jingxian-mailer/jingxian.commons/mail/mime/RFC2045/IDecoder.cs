
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace jingxian.mail.mime.RFC2045
{
    public interface IDecoder
    {
        bool CanDecode(string encodign);

        byte[] Decode(ref Stream dataStream);

        byte[] Decode(ref string data);
    }
}
