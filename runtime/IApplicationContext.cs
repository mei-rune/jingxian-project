using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.Filters;

    public interface IApplicationContext : IRuntimeContext
    {
        /// <summary>
        /// 程序名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Bundle 的搜索路径
        /// </summary>
        string BundlePath { get; }

        /// <summary>
        /// Bundle 的搜索路径中可用的程序集
        /// </summary>
        AssemblyFileSet AvailableAssemblies { get; }

        /// <summary>
        /// 设置在第二个应用程序域中载入Bundle
        /// </summary>
        bool ScanForBundlesInSecondAppDomain { get; set; }

        /// <summary>
        /// 启动的id
        /// </summary>
        string ApplicationLaunchableId { get; }

        /// <summary>
        /// 命令行参数
        /// </summary>
        ICommandLineArguments Arguments { get; }
    }
}
