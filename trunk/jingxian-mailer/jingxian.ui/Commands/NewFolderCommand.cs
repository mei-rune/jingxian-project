using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.commands
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    using jingxian.data;
    using jingxian.domainModel;

    public class NewFolderCommand : NewTreeNodeCommand
    {
        readonly IExtensionRegistry _extensionRegistry;
        readonly IHierarchyDecoratorService _contentService;
        readonly IIconResourceService _iconResourceService;
        readonly IVirtualFileSystem _virtualFs;

        public NewFolderCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory
            , IExtensionRegistry extensionRegistry
            , IHierarchyDecoratorService contentService
            , IIconResourceService iconResourceService
            , IVirtualFileSystem virtualFs )
            : base(selectionService, sessionFactory, "InsertFolder")
        {
            _extensionRegistry = extensionRegistry;
            _contentService = contentService;
            _iconResourceService = iconResourceService;
            _virtualFs = virtualFs;
        }

        protected override Form CreateForm()
        {
            return new dialogs.FolderPropertyDlg(_extensionRegistry
                , _contentService, _iconResourceService, _virtualFs, null );
        }

        protected override object GetResult(Form form)
        {
            return ((dialogs.FolderPropertyDlg)form).GetResult();
        }


    }
}
