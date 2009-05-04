using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui
{
    using jingxian.data;
    using jingxian.domainModel;

    public partial class AccountWizard : Form
    {
        IDbSessionFactory _sessionFactory;

        public AccountWizard(IDbSessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            InitializeComponent();

            this.smtpControl.ReloadProxies(sessionFactory);
            this.popControl.ReloadProxies(sessionFactory);
        }

        private void proxyButton_Click(object sender, EventArgs e)
        {
        }

        private void wizardControl_NextButtonClick(object sender, jingxian.ui.controls.wizards.GenericCancelEventArgs<jingxian.ui.controls.wizards.WizardControl> evtArgs)
        {
            try
            {
                switch (this.wizardControl.CurrentStepIndex)
                {
                    case 0:
                        {
                            System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(this.MailBox_textBox.Text);
                            this.smtpControl.Set(new SmtpServer(this.MailBox_textBox.Text
                                , "smtp." + mailAddress.Host
                                , 25
                                , this.MailBox_textBox.Text
                                , this.Password_textBox.Text ));
                        }
                        break;
                    case 1:
                        {
                            System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(this.MailBox_textBox.Text);
                            this.popControl.Set(new PopServer(this.MailBox_textBox.Text
                                , "pop." + mailAddress.Host
                                , 25
                                , this.MailBox_textBox.Text
                                , this.Password_textBox.Text ));
                        }
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
                return;
            }
            catch
            { }
            evtArgs.Cancel = true;
        }

        private void wizardControl_FinishButtonClick(object sender, EventArgs e)
        {
            BaseServer smtp = this.smtpControl.Get();
            BaseServer pop = this.popControl.Get();

            using (IDbSession session = _sessionFactory.NewSession())
            using( TransactionScope transaction = new TransactionScope( session ) )
            {
                session.Insert("InsertServer", smtp);
                session.Insert("InsertServer", pop);
                transaction.VoteCommit();
            }


            Close();
        }

        private void wizardControl_CancelButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
