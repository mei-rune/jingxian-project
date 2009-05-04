namespace jingxian.ui.views
{
    partial class ListExplorer
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
            jingxian.ui.controls.XPTable.Models.DataSourceColumnBinder dataSourceColumnBinder1 = new jingxian.ui.controls.XPTable.Models.DataSourceColumnBinder();
            this._splitter = new System.Windows.Forms.SplitContainer();
            this.table = new jingxian.ui.controls.XPTable.Models.Table();
            this._splitter.Panel1.SuspendLayout();
            this._splitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
            this.SuspendLayout();
            // 
            // _splitter
            // 
            this._splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitter.Location = new System.Drawing.Point(0, 0);
            this._splitter.Name = "_splitter";
            this._splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitter.Panel1
            // 
            this._splitter.Panel1.Controls.Add(this.table);
            this._splitter.Size = new System.Drawing.Size(516, 445);
            this._splitter.SplitterDistance = 172;
            this._splitter.TabIndex = 0;
            // 
            // table
            // 
            this.table.BorderColor = System.Drawing.Color.Black;
            this.table.DataMember = null;
            this.table.DataSourceColumnBinder = dataSourceColumnBinder1;
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.Size = new System.Drawing.Size(516, 172);
            this.table.TabIndex = 0;
            this.table.Text = "table1";
            this.table.UnfocusedBorderColor = System.Drawing.Color.Black;
            // 
            // ListExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitter);
            this.Name = "ListExplorer";
            this.Size = new System.Drawing.Size(516, 445);
            this._splitter.Panel1.ResumeLayout(false);
            this._splitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitter;
        private jingxian.ui.controls.XPTable.Models.Table table;
    }
}
