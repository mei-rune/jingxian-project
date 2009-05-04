using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace jingxian
{
    using Empinia.Core.Runtime;
    using Empinia.Core.Runtime.CodeAnalysis;
    using Empinia.Core.Runtime.Xml.Expressions;
    using Empinia.Core.Utilities;
    using Empinia.Core.Runtime.Commands;

    [LabelDecorator("SessionFactory.labeler", IconId = "defaulticons.service.png")]
    /// <summary>
    /// Implementation of an <see cref="Empinia.UI.IActionService"/>.
    /// </summary>
    [Service(
        typeof(IVirtualFileSystem),
        typeof(VirtualFileSystem),
        VirtualFileSystem.Id,
        jingxian.Constants.BundleId,
        Name = VirtualFileSystem.OriginalName)]
    public class VirtualFileSystem : IVirtualFileSystem
    {
        public const string Id = "jingxian.VirtualFileSystem";
        public const string OriginalName = "jingxian.VirtualFileSystem";

        private static string _basePath;
        private static string _runPath;

        public static void SetBasePath( string basePath )
        {
            _basePath = basePath;
        }

        public static void SetRunPath(string runPath)
        {
            _runPath = runPath;
        }

        public VirtualFileSystem()
        {
            _basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            _runPath = _basePath;
        }

        #region IVirtualFileSystem 成员

        public string GetBinPath(string url)
        {
            return Path.Combine(_basePath, url);
        }

        public string GetRunPath(string url)
        {
            return Path.Combine(_runPath, url);
        }

        public string GetTmpPath(string url)
        {
            return Path.Combine(Path.Combine(_runPath, "Temp"), url);
        }

        public string GetDataPath(string url)
        {
            return Path.Combine(Path.Combine(_runPath, "Data"), url);
        }

        #endregion
    }


    public class ComponentFileSystem : IVirtualFileSystem
    {
        private string _binPath = null;
        public IVirtualFileSystem _parent;
        private string _component;



        public ComponentFileSystem(IVirtualFileSystem parent, string component)
            : this(null, parent, component)
        {
        }

        public ComponentFileSystem(string binPath, IVirtualFileSystem parent, string component)
        {
            if (string.IsNullOrEmpty(component))
                throw new ArgumentNullException("component");

            if (null == parent)
                throw new ArgumentNullException("parent");


            _parent = parent;
            _binPath = binPath;
            _component = component;

            if (string.IsNullOrEmpty(_binPath))
            {
                _binPath = _parent.GetBinPath("components");
                _binPath = Path.Combine(_binPath, _component);
            }
        }

        #region IVirtualFileSystem 成员

        public string GetBinPath(string url)
        {
            return Path.Combine(_binPath, url);
        }

        public string GetRunPath(string url)
        {
            return _parent.GetRunPath(Path.Combine(_component, url));
        }

        public string GetTmpPath(string url)
        {
            return _parent.GetTmpPath(Path.Combine(_component, url));
        }

        public string GetDataPath(string url)
        {
            return _parent.GetDataPath(Path.Combine(_component, url));
        }

        #endregion
    }
}
