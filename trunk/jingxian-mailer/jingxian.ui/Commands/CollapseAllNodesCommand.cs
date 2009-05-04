using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.commands
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public class CollapseAllNodesCommand : TreeCommand
    {
        public CollapseAllNodesCommand(ISelectionService selectionService)
            : base(selectionService)
		{
		}

        protected override void DoWork(TreeNode treeNode)
        {
            treeNode.Collapse();
        }
    }
}
