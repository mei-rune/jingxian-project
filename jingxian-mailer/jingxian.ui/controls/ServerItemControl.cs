
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace jingxian.ui.controls
{
    using jingxian.data;
    using jingxian.domainModel;
    

  public partial class ServerItemControl : UserControl
  {
    public ServerItemControl()
    {
      InitializeComponent();

      //ReloadProxies();
    }

    public void ReloadProxies(IDbSessionFactory sessionFactory)
    {
        cmbListProxies.Items.Clear();
        cmbListProxies.Items.Add("");

        using (IDbSession session = sessionFactory.NewSession())
        using (TransactionScope transactionScope = new TransactionScope(session))
        {
            foreach (BaseServer server in session.QueryForList("GetProxys", null))
            {
                cmbListProxies.Items.Add(server);
            }
        }
    }

    public BaseServer Get()
    {
        BaseServer server = null;
        switch (labelType.Text.ToLower())
        {
            case "smtp":
                server = new SmtpServer();
                break;
            case "pop":
                server = new PopServer();
                break;
            case "proxy":
                server = new ProxyServer();
                break;
        }
        server.Id = int.Parse(labelId.Text);
        server.Caption = txtServerLabel.Text;
        server.Address = txtServer.Text;
        server.Port = int.Parse(txtPort.Text);
        server.UserName = txtLogin.Text;
        server.Password = txtPassword.Text;

        ProxyServer item = cmbListProxies.SelectedItem as ProxyServer;
        if (null != item)
            server.Misc["proxy"] = item.Id.ToString();

        if (chkUseSSL.Checked)
            server.Misc["ssl"] = "True";

        if (chkDisabled.Checked)
            server.Misc["disabled"] = "True";
        
        if (chkHeaderOnly.Checked)
            server.Misc["headeronly"] = "True";

        if (chkLeaveMailOnServer.Checked)
            server.Misc["behaviour"] = Behaviour.LeaveMessageOnServer.ToString();

        return server;
    }

    public void Set(BaseServer server)
    {
        if (null == server)
        {
            labelId.Text = "-1";
            txtServerLabel.Text = "";
            txtServer.Text = "";
            txtPassword.Text = "";
            txtPort.Text = "0";
            txtLogin.Text = "";
            labelType.Text = "smtp";

            chkDisabled.Visible = false;
            chkLeaveMailOnServer.Visible = false;
            chkHeaderOnly.Visible = false;

            return;
        }

        labelId.Text = server.Id.ToString();
        txtServerLabel.Text = server.Caption;
        txtServer.Text = server.Address;
        txtPassword.Text = server.Password;
        txtPort.Text = server.Port.ToString();
        txtLogin.Text = server.UserName;
        labelType.Text = server.Type;

        if (server.Type == "pop")
        {
            chkDisabled.Visible = true;
            chkLeaveMailOnServer.Visible = true;
            chkHeaderOnly.Visible = true;


            chkUseSSL.Visible = true;
            cmbListProxies.Visible = true;
        }
        else
        {
            chkDisabled.Visible = false;
            chkLeaveMailOnServer.Visible = false;
            chkHeaderOnly.Visible = false;

            if (server.Type == "proxy")
            {
                chkUseSSL.Visible = false;
                cmbListProxies.Visible = false;
            }
            else
            {
                chkUseSSL.Visible = true;
                cmbListProxies.Visible = true;
            }
        }



        chkUseSSL.Checked = (0 == string.Compare( server.Misc["ssl"], "true", true ));
        chkLeaveMailOnServer.Checked = (0 == string.Compare(server.Misc["behaviour"], "LeaveMessageOnServer", true));
        chkHeaderOnly.Checked =(0 == string.Compare( server.Misc["headeronly"], "true", true ));
        chkDisabled.Checked = (0 == string.Compare(server.Misc["disabled"]??"true", "true", true));

        string proxy = server.Misc["proxy"];
        cmbListProxies.SelectedItem = cmbListProxies.Items[0];
        if ( !string.IsNullOrEmpty( proxy ))
        {
            int proxyId = int.Parse( proxy );
            foreach( object obj in cmbListProxies.Items )
            {
                BaseServer px = obj as BaseServer;
                if ( null != px && px.Id == proxyId)
                {
                    cmbListProxies.SelectedItem = obj;
                    break;
                }
            }
        }
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
      string msg = "";

      try
      {
          //if (labelType.Text == "pop")
          //{
          //    if (ServerHelper.TryPOP(null,　Get() ))
          //    {
          //        MessageBox.Show("POP 服务器测试成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
          //        return;
          //    }
          //}
          //else if (labelType.Text == "smtp")
          //{
          //    if (ServerHelper.TrySMTP(null, Get() ))
          //    {
          //        MessageBox.Show("SMTP 服务器测试成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
          //        return;
          //    }
          //}
          //else if (labelType.Text == "imap")
          //{
          //    if (ServerHelper.TryIMAP(null, Get()))
          //    {
          //        MessageBox.Show("IMAP 服务器测试成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
          //        return;
          //    }
          //}
          //else
          //{
          //    if (ServerHelper.TryPing(null, Get()))
          //    {
          //        MessageBox.Show("服务器测试成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
          //        return;
          //    }
          //}
      }
      catch (Exception ex)
      {
          msg = ex.Message;
      }

      MessageBox.Show("测试失败!\r\n" + msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
