
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace jingxian.ui.controls
{
    using jingxian.data;
    using jingxian.domainModel;


    public partial class ServerListDialog : System.Windows.Forms.Form
    {
        int _generateId = 0;
        IDbSessionFactory _sessionFactory;

        public ServerListDialog(IDbSessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            InitializeComponent();

            InitServerList();
        }

        public void InitServerList()
        {
            this.listView.SelectedItems.Clear();
            List<BaseServer> servers = new List<BaseServer>();
            foreach (ListViewItem item in this.listView.Items)
            {
                BaseServer svr = item.Tag as BaseServer;
                if (null == svr)
                    continue;

                if (svr.Id <= 0)
                    servers.Add(svr);
            }

            this.listView.Items.Clear();

            using (IDbSession session = _sessionFactory.NewSession())
            {
                foreach (BaseServer server in session.QueryForList<BaseServer>("GetAllServers", null))
                {
                    Insert(server);
                }
            }

            foreach (BaseServer server in servers)
            {
                Insert(server);
            }

            if (this.listView.Items.Count > 0)
                this.listView.SelectedIndices.Add(0);
        }

        private BaseServer GetCurrent()
        {
            if (1 != listView.SelectedItems.Count)
                return null;
            return listView.SelectedItems[0].Tag as BaseServer;
        }

        private void btnNew_Click(object sender, System.EventArgs e)
        {
            contextMenuStrip.Show(btnNew, new Point(0, btnNew.Height));
        }

        private void btnRemoveServer_Click(object sender, System.EventArgs e)
        {
            BaseServer item = GetCurrent();
            if (null == item)
            {
                MessageBox.Show("请选择一个服务器!", "警告", MessageBoxButtons.OK);
                return;
            }

            if (DialogResult.Yes == MessageBox.Show("你想要删除所选的服务器?", "确认", MessageBoxButtons.YesNo))
            {
                using (IDbSession session = _sessionFactory.NewSession())
                using (TransactionScope transactionScope = new TransactionScope(session))
                {
                    session.Delete("DeleteServer", item.Id);
                    transactionScope.VoteCommit();
                }
                InitServerList();
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            BaseServer server = serverItemControl1.Get();

            using (IDbSession session = _sessionFactory.NewSession())
            using (TransactionScope transactionScope = new TransactionScope(session))
            {
                if (0 < server.Id)
                    session.Update("UpdateServer", server);
                else
                    session.Insert("InsertServer", server);

                transactionScope.VoteCommit();
            }

            if( 1 == this.listView.SelectedIndices.Count )
                this.listView.Items.RemoveAt(this.listView.SelectedIndices[0]);

            InitServerList();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnNewMailAccountWizard(object sender, EventArgs e)
        {
            AccountWizard wizard = new AccountWizard(_sessionFactory);

            AddOwnedForm(wizard);

            wizard.ShowDialog();

            InitServerList();
        }

        private void OnNewPOPServer(object sender, EventArgs e)
        {
            PopServer svr = new PopServer();

            svr.Caption = "添加 POP 服务器";
            svr.Port = 110;
            svr.Id = --_generateId;

            listView.SelectedIndices.Add( Insert(svr) );
        }

        private void OnNewSMTPServer(object sender, EventArgs e)
        {
            SmtpServer svr = new SmtpServer();
            svr.Caption = "添加 SMTP 服务器";
            svr.Port = 25;
            svr.Id = --_generateId;

            listView.SelectedIndices.Add(Insert(svr));
        }

        private void OnNewIMAPServer(object sender, EventArgs e)
        {
            IMAPServer svr = new IMAPServer();
            svr.Caption = "添加 IMAP 服务器";

            svr.Port = 143;
            svr.Id = --_generateId;

            listView.SelectedIndices.Add(Insert(svr));
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            serverItemControl1.Set( GetCurrent() );
        }

        //private void OnNewHttpProxy(object sender, EventArgs e)
        //{
        //    Server svr = new Server();
        //    svr.Caption = "添加 HTTP 代理";
        //    svr.Type = "http";

        //    svr.Port = 8080;

        //    lstServer.Items.Add(svr);
        //    lstServer.SelectedItems.Clear();
        //    lstServer.SelectedItems.Add(svr);
        //}

        //private void OnNewSSHTunnel(object sender, EventArgs e)
        //{
        //    Server svr = new Server();
        //    svr.Caption = "New SSH Tunnel";
        //    svr.Type = "ssh";
        //    svr.Port = 22;

        //    lstServer.Items.Add(svr);
        //    lstServer.SelectedItems.Clear();
        //    lstServer.SelectedItems.Add(svr);
        //}

        //private void OnFTPServer(object sender, EventArgs e)
        //{
        //    Server svr = new Server();
        //    svr.Caption = "添加 FTP 服务器";
        //    svr.Type = "ftp";
        //    svr.Port = 21;

        //    lstServer.Items.Add(svr);
        //    lstServer.SelectedItems.Clear();
        //    lstServer.SelectedItems.Add(svr);
        //}

        public int Insert(BaseServer server)
        {
            string header = ToHeader(server.Type);

            ListViewGroup currentGroup = null;
            foreach (ListViewGroup group in this.listView.Groups)
            {
                if (header == group.Header)
                {
                    currentGroup = group;
                    break;
                }
            }

            if (null == currentGroup)
            {
                currentGroup = new ListViewGroup(header, HorizontalAlignment.Left );
                this.listView.Groups.Add(currentGroup);
            }

            ListViewItem item = new ListViewItem(new string[] { server.ToString(), header }, currentGroup );
            item.Tag = server;

            this.listView.Items.Add(item);
            //currentGroup.Items.Add(item);
            return item.Index;
        }

        private string ToHeader(string type)
        {
            switch (type.ToLower())
            {
                case "smtp":
                    return "SMTP 服务器";
                case "pop":
                    return "POP 服务器";
                case "imap":
                    return "IMAP 服务器";
                case "proxy":
                    return "代理 服务器";
                default:
                    return "其它";
            }
        }
    }
}
