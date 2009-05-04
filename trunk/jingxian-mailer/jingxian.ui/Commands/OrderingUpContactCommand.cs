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

    public class OrderingUpContactCommand : OrderingCommand
    {
        public OrderingUpContactCommand(ISelectionService selectionService
            , IDbSessionFactory sessionFactory )
            : base(selectionService, sessionFactory)
		{
		}

        protected override bool IsUp
        {
            get { return true; }
        }

        protected override string GetStatement()
        {
            return "UpdateContactForOrdering";
        }
    }
}
