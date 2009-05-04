using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Text.RegularExpressions;
using jingxian.mail.popper;
using jingxian.mail.mime;

namespace PopConsoleTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            MainForm form = new MainForm();
            form.BringToFront();
            Application.Run(form);
        }
    }
}
