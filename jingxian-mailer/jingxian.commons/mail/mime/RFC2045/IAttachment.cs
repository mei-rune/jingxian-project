
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime.RFC2045
{
    public interface IAttachment
    {
        string Type { get; set; }

        string SubType { get; set; }

        string Disposition { get; set; }

        string Name { get; set; }

        byte[] Data { get; set; }
    }
}
