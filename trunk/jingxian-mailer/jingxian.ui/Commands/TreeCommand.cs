using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.commands
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

	public abstract class TreeCommand: ICommandHandler
	{
        protected readonly ISelectionService m_selectionService;

        protected TreeCommand(ISelectionService selectionService)
        {
            m_selectionService = selectionService;
		}
        protected T GetData<T>(TreeNode treeNode) where T : class 
        {
            IContentProvider contentProvider = treeNode as IContentProvider;
            if (null == contentProvider)
                return default( T );

            return contentProvider.Content as T;
        }

        protected abstract void DoWork(TreeNode treeNode);

		public bool CanExecute(object parameter)
		{
			throw new NotImplementedException();
		}

		public void Execute(object parameter)
		{
            ObjectSelection selection = m_selectionService.CurrentSelection as ObjectSelection;
            if (null == selection)
                return;
            TreeNode treeNode = selection.SelectedElement as TreeNode;
            DoWork(treeNode);
		}

		public string Id
		{
			get{throw new NotImplementedException();}
		}
	}
}
