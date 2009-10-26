using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.testSupport
{
    using jingxian.core.runtime;

    [Service( "BBB", typeof(IServiceProvider) )]
    [Component(typeof(IServiceProvider), typeof(ServerTester), "BBB", "jingxian.core.testSupport")]
    [Extension("BBBB"
    , "jingxian.core.testSupport"
    , Constants.Points.Services
   , typeof(ServerTester))]
    public class ServerTester
    {
        #region IService ��Ա

        public void Start()
        {
            Console.WriteLine("ServerTester start" );
        }

        public void Stop()
        {
            Console.WriteLine("ServerTester stop");
        }

        #endregion

        #region IRuntimePart ��Ա

        public string Id
        {
            get { return "BBB"; }
        }

        #endregion
    }
}
