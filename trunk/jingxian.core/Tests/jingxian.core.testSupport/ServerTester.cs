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
        #region IService 成员

        public void Start()
        {
            Console.WriteLine("ServerTester start" );
        }

        public void Stop()
        {
            Console.WriteLine("ServerTester stop");
        }

        #endregion

        #region IRuntimePart 成员

        public string Id
        {
            get { return "BBB"; }
        }

        #endregion
    }
}
