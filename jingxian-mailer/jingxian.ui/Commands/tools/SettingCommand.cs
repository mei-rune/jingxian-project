using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.commands.tools
{
    using jingxian.domainModel;

    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public class SettingCommand : ICommandHandler
    {
        IDbSessionFactory _sessionFactory;

        public SettingCommand(IDbSessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            jingxian.ui.controls.ServerListDialog dialog = new jingxian.ui.controls.ServerListDialog(_sessionFactory);
            dialog.ShowDialog();
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
    }
}
