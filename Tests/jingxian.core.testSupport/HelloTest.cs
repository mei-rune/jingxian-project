using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.testSupport
{
    using jingxian.core.runtime;

    [Extension( "aaaaaa"
        , "jingxian.core.testSupport"
        ,"jingxian.core.testSupport.autoTests"
       , typeof(HelloTest))]
    public class HelloTest : ITester
    {
        public HelloTest()
        {
        }

        #region ITester ≥…‘±

        public void Test()
        {
            Console.WriteLine("Hello world!");
        }

        #endregion
    }
}
