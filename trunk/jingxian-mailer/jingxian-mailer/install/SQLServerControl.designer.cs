namespace jingxian.install
{
    partial class SQLServerControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_DBServer = new System.Windows.Forms.Label();
            this.DBHost_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_DBName = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DBUsername_textBox = new System.Windows.Forms.TextBox();
            this.DBPassword_textBox = new System.Windows.Forms.TextBox();
            this.DBInitialCatalog_textBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_DBServer
            // 
            this.label_DBServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DBServer.AutoSize = true;
            this.label_DBServer.Location = new System.Drawing.Point(3, 0);
            this.label_DBServer.Name = "label_DBServer";
            this.label_DBServer.Size = new System.Drawing.Size(86, 25);
            this.label_DBServer.TabIndex = 1;
            this.label_DBServer.Text = "服务器:";
            this.label_DBServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DBHost_textBox
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.DBHost_textBox, 3);
            this.DBHost_textBox.Location = new System.Drawing.Point(95, 3);
            this.DBHost_textBox.Name = "DBHost_textBox";
            this.DBHost_textBox.Size = new System.Drawing.Size(228, 21);
            this.DBHost_textBox.TabIndex = 11;
            this.DBHost_textBox.Text = "localhost";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户名:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "密码:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_DBName
            // 
            this.label_DBName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label_DBName.AutoSize = true;
            this.label_DBName.Location = new System.Drawing.Point(3, 75);
            this.label_DBName.Name = "label_DBName";
            this.label_DBName.Size = new System.Drawing.Size(86, 25);
            this.label_DBName.TabIndex = 4;
            this.label_DBName.Text = "数据库名:";
            this.label_DBName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 238F));
            this.tableLayoutPanel3.Controls.Add(this.label_DBServer, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label_DBName, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.DBHost_textBox, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.DBUsername_textBox, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.DBPassword_textBox, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.DBInitialCatalog_textBox, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 8;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(351, 115);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // DBUsername_textBox
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.DBUsername_textBox, 3);
            this.DBUsername_textBox.Location = new System.Drawing.Point(95, 28);
            this.DBUsername_textBox.Name = "DBUsername_textBox";
            this.DBUsername_textBox.Size = new System.Drawing.Size(228, 21);
            this.DBUsername_textBox.TabIndex = 13;
            this.DBUsername_textBox.Text = "btim";
            // 
            // DBPassword_textBox
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.DBPassword_textBox, 3);
            this.DBPassword_textBox.Location = new System.Drawing.Point(95, 53);
            this.DBPassword_textBox.Name = "DBPassword_textBox";
            this.DBPassword_textBox.PasswordChar = '*';
            this.DBPassword_textBox.Size = new System.Drawing.Size(228, 21);
            this.DBPassword_textBox.TabIndex = 14;
            this.DBPassword_textBox.Text = "btim";
            // 
            // DBInitialCatalog_textBox
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.DBInitialCatalog_textBox, 3);
            this.DBInitialCatalog_textBox.Location = new System.Drawing.Point(95, 78);
            this.DBInitialCatalog_textBox.Name = "DBInitialCatalog_textBox";
            this.DBInitialCatalog_textBox.Size = new System.Drawing.Size(228, 21);
            this.DBInitialCatalog_textBox.TabIndex = 15;
            this.DBInitialCatalog_textBox.Text = "btim";
            // 
            // SQLServerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "SQLServerControl";
            this.Size = new System.Drawing.Size(351, 115);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_DBServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_DBName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        internal System.Windows.Forms.TextBox DBHost_textBox;
        internal System.Windows.Forms.TextBox DBUsername_textBox;
        internal System.Windows.Forms.TextBox DBPassword_textBox;
        internal System.Windows.Forms.TextBox DBInitialCatalog_textBox;
    }
}
