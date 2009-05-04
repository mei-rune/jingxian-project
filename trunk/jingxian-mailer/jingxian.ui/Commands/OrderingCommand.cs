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

    public abstract class OrderingCommand : TreeCommand
    {
        IDbSessionFactory _sessionFactory;
        public OrderingCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory )
            : base(selectionService)
		{
            _sessionFactory = sessionFactory;
		}

        void Insert(List<views.NodeWapper> folders, TreeNode treeNode, int ordering )
        {
            views.NodeWapper folder = GetData<views.NodeWapper>(treeNode);
            if (null == folder)
                return;
            folder.Ordering = ordering;
            folders.Add(folder);
        }

        protected abstract bool IsUp{ get; }
        protected abstract string GetStatement();

        protected override void DoWork(TreeNode treeNode)
        {
            if (null == (IsUp?treeNode.PrevNode : treeNode.NextNode) )
                return;

            views.NodeWapper folder = base.GetData<views.NodeWapper>(treeNode);
            if (null == folder)
                return;

            TreeView treeView = treeNode.TreeView;
            treeView.BeginUpdate();

            try
            {
                TreeNode old = null;

                using (IDbSession session = _sessionFactory.NewSession())
                using (TransactionScope transactionScope = new TransactionScope(session))
                {
                    List<views.NodeWapper> folders = new List<views.NodeWapper>();
                    TreeNode node = treeNode;
                    while (null != node.PrevNode)
                    {
                        node = node.PrevNode;
                    }

                    int seed = 0;
                    for (TreeNode n = node; null != n; n = n.NextNode)
                    {
                        if (n == treeNode)
                            continue;

                        if (IsUp)
                        {
                            if (treeNode == n.NextNode)
                            {
                                old = n;
                                Insert(folders, treeNode, seed++);
                            }

                            Insert(folders, n, seed++);
                        }
                        else
                        {
                            Insert(folders, n, seed++);
                            if (treeNode == n.PrevNode)
                            {
                                old = n;
                                Insert(folders, treeNode, seed++);
                            }
                        }


                    }

                    foreach (views.NodeWapper instance in folders)
                    {
                        session.Update( GetStatement() , instance.GetInstance());
                    }

                    transactionScope.VoteCommit();
                }

                if (null == old)
                    return;

                int index = old.Index; //(old.PrevNode == treeNode)? old.Index;
                treeNode.Remove();
                if( null == old.Parent )
                    old.TreeView.Nodes.Insert(index, treeNode);
                else
                    old.Parent.Nodes.Insert(index, treeNode);
            }
            catch (Exception exception)
            {
                Utils.ShowMsgBox(exception);
            }
            finally 
            {
                treeView.EndUpdate();
            }
        }
    }
}
