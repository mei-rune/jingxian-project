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

    public abstract class NewTreeNodeCommand : TreeCommand
    {
        protected readonly IDbSessionFactory _sessionFactory;
        protected readonly string _statement;

        public NewTreeNodeCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory
            , string statement )
            : base(selectionService)
        {

            _sessionFactory = sessionFactory;
            _statement = statement;
        }

        protected abstract Form CreateForm();

        protected abstract object GetResult( Form form);

        protected override void DoWork(TreeNode treeNode)
        {
            views.NodeWapper folder = GetData<views.NodeWapper>(treeNode);
            if (null == folder)
                return;

            Form dlg = CreateForm();

        reTry:
            if (DialogResult.OK != dlg.ShowDialog())
                return;

            views.NodeWapper newFolder = new views.NodeWapper( GetResult(dlg) );
            newFolder.Parent = folder.Id;
            try
            {
                using (IDbSession session = _sessionFactory.NewSession())
                using (TransactionScope transactionScope = new TransactionScope(session))
                {
                    session.Insert(_statement , newFolder.GetInstance());
                    transactionScope.VoteCommit();
                }

                views.TreePage mailPage = treeNode.TreeView as views.TreePage;
                if (null != mailPage)
                    mailPage.InsertNode(treeNode.Nodes, mailPage.CreateNode( newFolder ) );
                return;
            }
            catch (Exception exception)
            {
                Utils.ShowMsgBox(exception);
            }
            goto reTry;
        }
    }
}
