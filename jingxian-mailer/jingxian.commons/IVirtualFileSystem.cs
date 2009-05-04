using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian
{
    /// <summary>
    /// 运行的目录结构为
    /// ./[安装路径]
    ///     |
    ///     +---[组件1路径]
    ///     |
    ///     +---[组件2路径]
    /// 
    /// ./[项目路径]/
    ///     | 
    ///     +---Tmp
    ///     |     |
    ///     |     +---[组件1路径]
    ///     |     |
    ///     |     +---[组件2路径]
    ///     +---Data
    ///           |
    ///           |
    ///           +---[组件1路径]
    ///           |
    ///           +---[组件2路径]
    /// </summary>
    public interface IVirtualFileSystem
    {
        /// <summary>
        /// 取得 [安装路径] + "/" + url;
        /// </summary>
        string GetBinPath(string url);

        /// <summary>
        /// 取得 [项目路径] + "/" + url;
        /// </summary>
        string GetRunPath(string url);

        /// <summary>
        /// 取得 [项目路径/Tmp] + "/" + url;
        /// </summary>
        string GetTmpPath(string url);

        /// <summary>
        /// 取得 [项目路径/Data] + "/" + url;
        /// </summary>
        string GetDataPath(string url);
    }
}
