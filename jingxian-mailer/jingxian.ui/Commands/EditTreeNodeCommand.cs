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

    public abstract class EditTreeNodeCommand : TreeCommand
    {
        protected readonly IDbSessionFactory _sessionFactory;
        protected readonly string _statement;

        public EditTreeNodeCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory
            , string statement )
            : base(selectionService)
        {
            _statement = statement;

            _sessionFactory = sessionFactory;
		}

        protected abstract Form CreateForm( int id );

        protected abstract object GetResult(Form form);

        protected override void DoWork(TreeNode treeNode)
        {
            views.NodeWapper folder = GetData<views.NodeWapper>(treeNode);
            if (null == folder)
                return;

            Form dlg = CreateForm(folder.Id);
        reTry:
            if (DialogResult.OK != dlg.ShowDialog())
                return;

            object newFolder = GetResult( dlg );

            try
            {
                using (IDbSession session = _sessionFactory.NewSession())
                using (TransactionScope transactionScope = new TransactionScope(session))
                {
                    session.Update( _statement , newFolder);
                    transactionScope.VoteCommit();
                }
            }
            catch (Exception exception)
            {
                Utils.ShowMsgBox(exception);
                goto reTry;
            }

            views.TreePage mailPage = treeNode.TreeView as views.TreePage;
            if (null != mailPage)
            {
                mailPage.BeginUpdate();
                try
                {
                    views.TreePage.ContentNode newFolderNode = mailPage.CreateNode( new views.NodeWapper( newFolder) );
                    mailPage.InsertNode((null == treeNode.Parent) ? mailPage.Nodes : treeNode.Parent.Nodes
                        , newFolderNode);
                    TreeNode[] treeNodes = new TreeNode[ treeNode.Nodes.Count ];
                    treeNode.Nodes.CopyTo(treeNodes, 0);

                    treeNode.Remove();
                    foreach (TreeNode n in treeNodes )
                    {
                        newFolderNode.Nodes.Add( n );
                    }

                    mailPage.SelectedNode = newFolderNode;
                }
                catch (Exception exception)
                {
                    Utils.ShowMsgBox(exception);
                }
                finally
                {
                    mailPage.EndUpdate();
                }
            }
        }
    }
}
