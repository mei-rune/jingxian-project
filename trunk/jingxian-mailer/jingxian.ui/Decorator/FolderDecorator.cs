using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace jingxian.ui.decorator
{
    using Empinia.Core.Runtime;

    public class FolderDecorator : IHierarchyDecorator
    {
        public static readonly string DecoratorId = "jingxian.folder.navigator.contentHierarchy";

        public FolderDecorator()
        {
        }

        #region IHierarchyDecorator 成员

        public bool CanDecorate(object decorated)
        {
            return true;
        }

        public Type DecoratedType
        {
            get { return typeof(jingxian.data.Folder); }
        }

        public ICollection GetChildren(object decorated)
        {
            return null;
        }

        public bool HasChildren(object decorated)
        {
            return true;
        }

        public string Id
        {
            get { return DecoratorId; }
        }

        #endregion
    }
}
