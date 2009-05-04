using System;
using System.Collections.Generic;

namespace jingxian.mail.mime.RFC2045
{
    public interface IMultipartEntity: IEntity
    {
        IList<IEntity> BodyParts { get; set; }
    }
}
