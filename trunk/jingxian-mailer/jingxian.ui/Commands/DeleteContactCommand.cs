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

    public class DeleteContactCommand : TreeCommand
    {
        IDbSessionFactory _sessionFactory;
        public DeleteContactCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory )
            : base(selectionService)
		{
            _sessionFactory = sessionFactory;
		}

        protected override void DoWork(TreeNode treeNode)
        {
            IContentProvider contentProvider = treeNode as IContentProvider;
            if (null == contentProvider)
                return;

            views.NodeWapper folder = contentProvider.Content as views.NodeWapper;
            if (null == folder)
                return;

            try
            {
                using (IDbSession session = _sessionFactory.NewSession())
                using (TransactionScope transactionScope = new TransactionScope(session))
                {
                    session.Delete("DeleteContact", folder.Id);
                    transactionScope.VoteCommit();
                }

                treeNode.Remove();
            }
            catch( Exception exception)
            {
                Utils.ShowMsgBox(exception);
            }
        }
    }
}
