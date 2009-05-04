
using System;
using System.Collections.Generic;

namespace jingxian.mail.mime.RFC2045
{
    public interface IEntity
    {
        byte[] Body { get; set; }
        string Delimiter { get; set; }
        IList<RFC822.Field> Fields { get; set; }
        IMultipartEntity Parent { get; set; }
    }
}
