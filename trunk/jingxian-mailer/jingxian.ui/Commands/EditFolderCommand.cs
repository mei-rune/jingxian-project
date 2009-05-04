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

    public class EditFolderCommand : EditTreeNodeCommand
    {
        readonly IExtensionRegistry _extensionRegistry;
        readonly IHierarchyDecoratorService _contentService;
        readonly IIconResourceService _iconResourceService;
        readonly IVirtualFileSystem _virtualFs;

        public EditFolderCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory
            , IExtensionRegistry extensionRegistry
            , IHierarchyDecoratorService contentService
            , IIconResourceService iconResourceService
            , IVirtualFileSystem virtualFs )
            : base(selectionService, sessionFactory, "UpdateFolder")
        {
            _extensionRegistry = extensionRegistry;
            _contentService = contentService;
            _iconResourceService = iconResourceService;
            _virtualFs = virtualFs;
		}


        protected override Form CreateForm(int id)
        {
            FolderWithBLOBs oldFolder = null;
            using (IDbSession session = _sessionFactory.NewSession())
            {
                oldFolder = session.QueryForObject<FolderWithBLOBs>("GetFolderWithBLOBs", id);
            }

            return new dialogs.FolderPropertyDlg(_extensionRegistry
                , _contentService, _iconResourceService, _virtualFs, oldFolder);            
        }

        protected override object GetResult(Form form)
        {
            return ((dialogs.FolderPropertyDlg)form).GetResult();
        }
    }
}
