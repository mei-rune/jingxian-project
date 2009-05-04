namespace jingxian.ui.controls
{
    partial class ServerItemControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblLogin = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.chkLeaveMailOnServer = new System.Windows.Forms.CheckBox();
            this.chkUseSSL = new System.Windows.Forms.CheckBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerLabel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbListProxies = new System.Windows.Forms.ComboBox();
            this.chkDisabled = new System.Windows.Forms.CheckBox();
            this.chkHeaderOnly = new System.Windows.Forms.CheckBox();
            this.labelId = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(2, 29);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(77, 12);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "服务器地址：";
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(75, 26);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(311, 21);
            this.txtServer.TabIndex = 2;
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(2, 78);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(41, 12);
            this.lblLogin.TabIndex = 0;
            this.lblLogin.Text = "登录：";
            // 
            // txtLogin
            // 
            this.txtLogin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogin.Location = new System.Drawing.Point(75, 76);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(311, 21);
            this.txtLogin.TabIndex = 4;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(2, 127);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(29, 12);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "代理";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(75, 101);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(311, 21);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(75, 51);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(311, 21);
            this.txtPort.TabIndex = 3;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(2, 54);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(41, 12);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "端口：";
            // 
            // chkLeaveMailOnServer
            // 
            this.chkLeaveMailOnServer.AutoSize = true;
            this.chkLeaveMailOnServer.Location = new System.Drawing.Point(163, 150);
            this.chkLeaveMailOnServer.Name = "chkLeaveMailOnServer";
            this.chkLeaveMailOnServer.Size = new System.Drawing.Size(132, 16);
            this.chkLeaveMailOnServer.TabIndex = 7;
            this.chkLeaveMailOnServer.TabStop = false;
            this.chkLeaveMailOnServer.Text = "删除服务器上的邮件";
            this.chkLeaveMailOnServer.UseVisualStyleBackColor = true;
            // 
            // chkUseSSL
            // 
            this.chkUseSSL.AutoSize = true;
            this.chkUseSSL.Location = new System.Drawing.Point(5, 171);
            this.chkUseSSL.Name = "chkUseSSL";
            this.chkUseSSL.Size = new System.Drawing.Size(138, 16);
            this.chkUseSSL.TabIndex = 8;
            this.chkUseSSL.TabStop = false;
            this.chkUseSSL.Text = "便用 SSL (安全连接)";
            this.chkUseSSL.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(301, 146);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(85, 21);
            this.btnTest.TabIndex = 50;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器描述：";
            // 
            // txtServerLabel
            // 
            this.txtServerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServerLabel.Location = new System.Drawing.Point(75, 2);
            this.txtServerLabel.Name = "txtServerLabel";
            this.txtServerLabel.Size = new System.Drawing.Size(311, 21);
            this.txtServerLabel.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "密码";
            // 
            // cmbListProxies
            // 
            this.cmbListProxies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbListProxies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbListProxies.FormattingEnabled = true;
            this.cmbListProxies.Items.AddRange(new object[] {
            "None",
            "Edit Proxies..."});
            this.cmbListProxies.Location = new System.Drawing.Point(75, 125);
            this.cmbListProxies.Name = "cmbListProxies";
            this.cmbListProxies.Size = new System.Drawing.Size(311, 20);
            this.cmbListProxies.TabIndex = 6;
            // 
            // chkDisabled
            // 
            this.chkDisabled.AutoSize = true;
            this.chkDisabled.Location = new System.Drawing.Point(5, 150);
            this.chkDisabled.Name = "chkDisabled";
            this.chkDisabled.Size = new System.Drawing.Size(108, 16);
            this.chkDisabled.TabIndex = 8;
            this.chkDisabled.TabStop = false;
            this.chkDisabled.Text = "禁用这个服务器";
            this.chkDisabled.UseVisualStyleBackColor = true;
            // 
            // chkHeaderOnly
            // 
            this.chkHeaderOnly.AutoSize = true;
            this.chkHeaderOnly.Location = new System.Drawing.Point(163, 171);
            this.chkHeaderOnly.Name = "chkHeaderOnly";
            this.chkHeaderOnly.Size = new System.Drawing.Size(120, 16);
            this.chkHeaderOnly.TabIndex = 8;
            this.chkHeaderOnly.TabStop = false;
            this.chkHeaderOnly.Text = "仅下载邮件的标题";
            this.chkHeaderOnly.UseVisualStyleBackColor = true;
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Location = new System.Drawing.Point(296, 180);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(17, 12);
            this.labelId.TabIndex = 51;
            this.labelId.Text = "-1";
            this.labelId.Visible = false;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(348, 178);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(29, 12);
            this.labelType.TabIndex = 52;
            this.labelType.Text = "smtp";
            this.labelType.Visible = false;
            // 
            // ServerItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelId);
            this.Controls.Add(this.cmbListProxies);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.chkHeaderOnly);
            this.Controls.Add(this.chkDisabled);
            this.Controls.Add(this.chkUseSSL);
            this.Controls.Add(this.chkLeaveMailOnServer);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServerLabel);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblServer);
            this.Name = "ServerItemControl";
            this.Size = new System.Drawing.Size(389, 196);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.CheckBox chkLeaveMailOnServer;
        private System.Windows.Forms.CheckBox chkUseSSL;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbListProxies;
        private System.Windows.Forms.CheckBox chkDisabled;
        private System.Windows.Forms.CheckBox chkHeaderOnly;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label labelType;
    }
}
