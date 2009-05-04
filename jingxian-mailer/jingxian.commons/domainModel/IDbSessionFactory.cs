using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.domainModel
{
    public interface IDbSessionFactory
    {
        IDbSession NewSession();
    }
}