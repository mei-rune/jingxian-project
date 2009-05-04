namespace jingxian.ui.controls
{
    partial class MessageViewer
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.titleBrowser = new System.Windows.Forms.WebBrowser();
            this.bodyBrowser = new System.Windows.Forms.WebBrowser();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.titleBrowser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bodyBrowser, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.0128F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.98721F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(592, 547);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // titleBrowser
            // 
            this.titleBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleBrowser.Location = new System.Drawing.Point(3, 3);
            this.titleBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.titleBrowser.Name = "titleBrowser";
            this.titleBrowser.ScrollBarsEnabled = false;
            this.titleBrowser.Size = new System.Drawing.Size(586, 97);
            this.titleBrowser.TabIndex = 0;
            // 
            // bodyBrowser
            // 
            this.bodyBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyBrowser.Location = new System.Drawing.Point(3, 106);
            this.bodyBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.bodyBrowser.Name = "bodyBrowser";
            this.bodyBrowser.Size = new System.Drawing.Size(586, 438);
            this.bodyBrowser.TabIndex = 1;
            // 
            // MessageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MessageViewer";
            this.Size = new System.Drawing.Size(592, 547);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.WebBrowser titleBrowser;
        private System.Windows.Forms.WebBrowser bodyBrowser;

    }
}
