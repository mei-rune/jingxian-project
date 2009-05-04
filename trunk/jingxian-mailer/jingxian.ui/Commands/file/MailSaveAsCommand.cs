using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.commands.file
{
    using Empinia.Core.Runtime;
    using Empinia.UI;
    using Empinia.UI.Workbench;

    public class MailSaveAsCommand : ICommandHandler
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            // TODO: 实现这个功能
            MessageBox.Show("没有实现这个功能!");
        }

        public string Id
        {
            get { throw new NotImplementedException(); }
        }
    }
}
