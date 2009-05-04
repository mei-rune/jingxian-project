
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime
{
    public interface IMailReader
    {
        IMailMessage Read(ref System.IO.Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria);

        event DataReadEventHandler DataRead;

        long DataReadUpdateInterval { get; set;}

    }
}
