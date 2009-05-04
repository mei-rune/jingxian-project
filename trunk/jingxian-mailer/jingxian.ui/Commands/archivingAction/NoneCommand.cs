using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.ui.Commands.archivingAction
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public class NoneCommand : ICommandHandler
    {
        #region ICommandHandler 成员

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
