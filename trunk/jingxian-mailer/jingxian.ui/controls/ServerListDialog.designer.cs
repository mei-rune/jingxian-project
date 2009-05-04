
namespace jingxian.ui.controls
{
  partial class ServerListDialog
  {
      /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }
    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerListDialog));
        this.btnNew = new System.Windows.Forms.Button();
        this.btnRemoveServer = new System.Windows.Forms.Button();
        this.btnOk = new System.Windows.Forms.Button();
        this.btnCancel = new System.Windows.Forms.Button();
        this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
        this.mailAccountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.pOPServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.sMTPServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.iMAPServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this.listView = new System.Windows.Forms.ListView();
        this.nameHeader = new System.Windows.Forms.ColumnHeader();
        this.typeHeader = new System.Windows.Forms.ColumnHeader();
        this.serverItemControl1 = new jingxian.ui.controls.ServerItemControl();
        this.contextMenuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.panel1.SuspendLayout();
        this.SuspendLayout();
        // 
        // btnNew
        // 
        this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnNew.Location = new System.Drawing.Point(131, 358);
        this.btnNew.Name = "btnNew";
        this.btnNew.Size = new System.Drawing.Size(102, 26);
        this.btnNew.TabIndex = 1;
        this.btnNew.Text = "添加服务器";
        this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
        // 
        // btnRemoveServer
        // 
        this.btnRemoveServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.btnRemoveServer.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnRemoveServer.Location = new System.Drawing.Point(10, 358);
        this.btnRemoveServer.Name = "btnRemoveServer";
        this.btnRemoveServer.Size = new System.Drawing.Size(115, 26);
        this.btnRemoveServer.TabIndex = 1;
        this.btnRemoveServer.Text = "删除服务器";
        this.btnRemoveServer.Click += new System.EventHandler(this.btnRemoveServer_Click);
        // 
        // btnOk
        // 
        this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnOk.Location = new System.Drawing.Point(363, 358);
        this.btnOk.Name = "btnOk";
        this.btnOk.Size = new System.Drawing.Size(102, 26);
        this.btnOk.TabIndex = 3;
        this.btnOk.Text = "保存";
        this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.btnCancel.Location = new System.Drawing.Point(471, 358);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(102, 26);
        this.btnCancel.TabIndex = 3;
        this.btnCancel.Text = "退出";
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // contextMenuStrip
        // 
        this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mailAccountToolStripMenuItem,
            this.pOPServerToolStripMenuItem,
            this.sMTPServerToolStripMenuItem,
            this.iMAPServerToolStripMenuItem});
        this.contextMenuStrip.Name = "contextMenuStrip1";
        this.contextMenuStrip.Size = new System.Drawing.Size(161, 92);
        // 
        // mailAccountToolStripMenuItem
        // 
        this.mailAccountToolStripMenuItem.Name = "mailAccountToolStripMenuItem";
        this.mailAccountToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
        this.mailAccountToolStripMenuItem.Text = "邮件帐号 (向导)";
        this.mailAccountToolStripMenuItem.Click += new System.EventHandler(this.OnNewMailAccountWizard);
        // 
        // pOPServerToolStripMenuItem
        // 
        this.pOPServerToolStripMenuItem.Name = "pOPServerToolStripMenuItem";
        this.pOPServerToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
        this.pOPServerToolStripMenuItem.Text = "POP 服务器";
        this.pOPServerToolStripMenuItem.Click += new System.EventHandler(this.OnNewPOPServer);
        // 
        // sMTPServerToolStripMenuItem
        // 
        this.sMTPServerToolStripMenuItem.Name = "sMTPServerToolStripMenuItem";
        this.sMTPServerToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
        this.sMTPServerToolStripMenuItem.Text = "SMTP 服务器";
        this.sMTPServerToolStripMenuItem.Click += new System.EventHandler(this.OnNewSMTPServer);
        // 
        // iMAPServerToolStripMenuItem
        // 
        this.iMAPServerToolStripMenuItem.Name = "iMAPServerToolStripMenuItem";
        this.iMAPServerToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
        this.iMAPServerToolStripMenuItem.Text = "IMAP 服务器";
        this.iMAPServerToolStripMenuItem.Click += new System.EventHandler(this.OnNewIMAPServer);
        // 
        // pictureBox1
        // 
        this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
        this.pictureBox1.Location = new System.Drawing.Point(121, 23);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(69, 55);
        this.pictureBox1.TabIndex = 6;
        this.pictureBox1.TabStop = false;
        // 
        // label1
        // 
        this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.Location = new System.Drawing.Point(197, 28);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(270, 26);
        this.label1.TabIndex = 6;
        this.label1.Text = "服务器管理";
        // 
        // label2
        // 
        this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label2.Location = new System.Drawing.Point(197, 55);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(696, 26);
        this.label2.TabIndex = 6;
        this.label2.Text = "添加/删除 POP 服务器.";
        // 
        // panel1
        // 
        this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.panel1.BackColor = System.Drawing.SystemColors.Window;
        this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.panel1.Controls.Add(this.label2);
        this.panel1.Controls.Add(this.label1);
        this.panel1.Controls.Add(this.pictureBox1);
        this.panel1.Location = new System.Drawing.Point(-116, -20);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(819, 82);
        this.panel1.TabIndex = 5;
        // 
        // listView
        // 
        this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.typeHeader});
        this.listView.FullRowSelect = true;
        this.listView.Location = new System.Drawing.Point(4, 68);
        this.listView.MultiSelect = false;
        this.listView.Name = "listView";
        this.listView.Size = new System.Drawing.Size(314, 275);
        this.listView.TabIndex = 6;
        this.listView.UseCompatibleStateImageBehavior = false;
        this.listView.View = System.Windows.Forms.View.Details;
        this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
        this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
        // 
        // nameHeader
        // 
        this.nameHeader.Text = "名字";
        this.nameHeader.Width = 150;
        // 
        // typeHeader
        // 
        this.typeHeader.Text = "类型";
        this.typeHeader.Width = 120;
        // 
        // serverItemControl1
        // 
        this.serverItemControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.serverItemControl1.Location = new System.Drawing.Point(324, 69);
        this.serverItemControl1.Name = "serverItemControl1";
        this.serverItemControl1.Size = new System.Drawing.Size(251, 274);
        this.serverItemControl1.TabIndex = 4;
        // 
        // ServerListDialog
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
        this.ClientSize = new System.Drawing.Size(585, 389);
        this.Controls.Add(this.listView);
        this.Controls.Add(this.panel1);
        this.Controls.Add(this.serverItemControl1);
        this.Controls.Add(this.btnRemoveServer);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.btnNew);
        this.Controls.Add(this.btnOk);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ServerListDialog";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.Text = "Manage Servers";
        this.contextMenuStrip.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.panel1.ResumeLayout(false);
        this.ResumeLayout(false);

    }
    #endregion

    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnRemoveServer;
    private System.Windows.Forms.Button btnOk;
    private jingxian.ui.controls.ServerItemControl serverItemControl1;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem mailAccountToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pOPServerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem sMTPServerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem iMAPServerToolStripMenuItem;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ListView listView;
    private System.Windows.Forms.ColumnHeader nameHeader;
    private System.Windows.Forms.ColumnHeader typeHeader;
  }
}
