using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using jingxian.mail.mime;
using jingxian.mail.mime.RFC2045;
using jingxian.mail.popper;

namespace PopConsoleTest
{
    public partial class MainForm : Form
    {
        private POPClient client;
        

        public delegate void UpdateEnabledDelegate(bool enabled);
        public delegate void UpdateStatusDelegate(int i);
        public delegate void UpdateLabelDelegate(string text);
        public delegate void ShowMessageDelegate(IMimeMailMessage message);

        public MainForm()
        {
            InitializeComponent();
            progressBar1.Minimum = 0;            
            
            client = new POPClient();
            client.KeepAliveInterval = 60000;
            client.Port = 110;
            client.StateChanged += new EventHandler(client_StateChanged);
            client.DataRead += new DataReadEventHandler(client_DataRead);
            client.CommandIssued += new CommandIssuedEventHandler(client_CommandIssued);
            client.UpdateInterval = 100;
            client.Server = "20.0.8.82";
            client.User = "meifakun@betamail.net";
            client.Password = "111111";            

            SetLabel(client.CurrentState.ToString());

            this.Show();
        }


        #region ThreadSafe setters
        private void SetValue(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                 BeginInvoke(new UpdateStatusDelegate(SetValue), new object[] {value });
            }
            else
            {
                progressBar1.Value = value;
                progressBar1.Update();
            }
        }

        private void SetLabel(string text)
        {
            if (label1.InvokeRequired)
            {
                BeginInvoke(new UpdateLabelDelegate(SetLabel), new object[] { text });
            }
            else
            {
                label1.Text = text;
            }
        }

        private void SetBytesLabel(string text)
        {
            if (BytesLabel.InvokeRequired)
            {
                BeginInvoke(new UpdateLabelDelegate(SetBytesLabel), new object[] { text });
            }
            else
            {
                BytesLabel.Text = text;
            }
        }

        private void AppendText(string text)
        {
            if (CommandTextBox.InvokeRequired)
            {
                BeginInvoke(new UpdateLabelDelegate(AppendText), new object[] { text });
            }
            else
            {
                CommandTextBox.Text += text;
            }
        }

        private void ShowMesage(jingxian.mail.mime.RFC2045.IMimeMailMessage message)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new ShowMessageDelegate(ShowMesage), new object[]{ message});
            }
            else
            {
                MessageForm mf = new MessageForm(message);
                mf.Show(this);
            }            
        }               

        private void EnableMessageGrid(bool enabled)
        {
            if (MessagedataGridView.InvokeRequired)
            {
                BeginInvoke(new UpdateEnabledDelegate(EnableMessageGrid), new object[] { enabled });
            }
            else
            {
                MessagedataGridView.Enabled = enabled;
            }
        }

        #endregion

        void List()
        {        
            IList<IMailMessage> messages = new List<IMailMessage>();
             long[] lar = client.ListMessages();
             int i = 0;
             while (i < lar.Length)
             {                 
                 int size = (int)client.GetMessageSize(i + 1);
                 if (size > 0)
                 {
                     progressBar1.Maximum = size;
                     IMailMessage messsage = client.FetchMailHeader(i + 1);
                     messages.Add(messsage);
                     i++;
                 }
             }             
             MessagedataGridView.DataSource = messages;              
        }

        void client_DataRead(object sender, DataReadEventArgs args)
        {
            this.SetValue((int) args.AmountRead);
            SetBytesLabel(args.AmountRead.ToString());
        }

        void client_StateChanged(object sender, EventArgs e)
        {            
            StateChangedEventArgs scea = e as StateChangedEventArgs;
            if (scea != null)
            {
                SetLabel(scea.NewState.ToString());
            }

            if (scea.NewState.ToString().Equals("DISCONNECTED"))
            {
                ConnectButton.Text = "Connect";                
                EnableMessageGrid(false);                
            }

            if (!scea.NewState.ToString().Equals("DISCONNECTED"))
            {
                ConnectButton.Text = "Disconnect";                
            }

            if (scea.NewState.ToString().Equals("READING"))
            {
                EnableMessageGrid(false);
            }

            if (scea.NewState.ToString().Equals("TRANSACTION")) 
            {
                EnableMessageGrid(true);
            }
        }

        void client_CommandIssued(object sender, CommandIssuedEventArgs args)
        {
            AppendText(
                string.Format("Client issued '{0}' command {1} The response was {2}",
                args.Command.ToString(), Environment.NewLine, args.Command.Response));
        }

        void callbackmethod(IAsyncResult result)
        {
            IMailMessage message = client.EndFetchMail(result);
            jingxian.mail.mime.RFC2045.IMimeMailMessage mimeMessage = message as jingxian.mail.mime.RFC2045.IMimeMailMessage;                                   
            SetValue(0);
            ShowMesage(mimeMessage);
        }        

        private void MessagedataGridView_CellContentClick(object sender, 
            DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int size = (int)client.GetMessageSize(e.RowIndex + 1);
                if (size > 0)
                {
                    progressBar1.Maximum = size;
                    SizeLabel.Text = " bytes of " + size.ToString() + " read.."; ;
                    AsyncCallback callback = new AsyncCallback(callbackmethod);
                    client.BeginFetchMail(e.RowIndex + 1, callback, null);
                }
            }

            if (e.ColumnIndex == 1)
            {
                if (client.DeleteMessage(e.RowIndex + 1))
                    MessageBox.Show(string.Format("Message {0} deleted.", e.RowIndex + 1), 
                        "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }            
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (client.CurrentState.ToString().Equals("DISCONNECTED"))
            {
                if (client.Connect())
                {
                    if (!client.Authenticate())
                        MessageBox.Show("Authentication attempt failed", "Failure!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Connection attempt failed", "Failure!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
                client.Disconnect();
        }

        private void ListButton_Click(object sender, EventArgs e)
        {
            List();
            EnableMessageGrid(true);
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            CommandTextBox.Text = string.Empty;
        }
       
    }       
    
}