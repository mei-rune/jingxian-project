namespace jingxian.ui.views
{
    partial class MessageExplorerWidget
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
            this.m_splitter = new System.Windows.Forms.SplitContainer();
            this.table = new jingxian.ui.controls.XPTable.Models.Table();
            this.columnModel = new jingxian.ui.controls.XPTable.Models.ColumnModel();
            this.mailColumn = new jingxian.ui.controls.XPTable.Models.ImageColumn();
            this.levelColumn = new jingxian.ui.controls.XPTable.Models.ImageColumn();
            this.senderColumn = new jingxian.ui.controls.XPTable.Models.TextColumn();
            this.readStateColumn = new jingxian.ui.controls.XPTable.Models.ImageColumn();
            this.subjectColumn = new jingxian.ui.controls.XPTable.Models.TextColumn();
            this.isspamColumn = new jingxian.ui.controls.XPTable.Models.ImageColumn();
            this.dateColumn = new jingxian.ui.controls.XPTable.Models.TextColumn();
            this.sizeColumn = new jingxian.ui.controls.XPTable.Models.TextColumn();
            this.tableModel = new jingxian.ui.controls.XPTable.Models.TableModel();
            this.m_splitter.Panel1.SuspendLayout();
            this.m_splitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table)).BeginInit();
            this.SuspendLayout();
            // 
            // m_splitter
            // 
            this.m_splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_splitter.Location = new System.Drawing.Point(0, 0);
            this.m_splitter.Name = "m_splitter";
            this.m_splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // m_splitter.Panel1
            // 
            this.m_splitter.Panel1.Controls.Add(this.table);
            this.m_splitter.Size = new System.Drawing.Size(615, 350);
            this.m_splitter.SplitterDistance = 181;
            this.m_splitter.TabIndex = 0;
            // 
            // table
            // 
            this.table.BorderColor = System.Drawing.Color.Black;
            this.table.ColumnModel = this.columnModel;
            this.table.DataMember = null;
            this.table.DataSourceColumnBinder = dataSourceColumnBinder1;
            this.table.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table.FullRowSelect = true;
            this.table.Location = new System.Drawing.Point(0, 0);
            this.table.Name = "table";
            this.table.NoItemsText = "";
            this.table.Size = new System.Drawing.Size(615, 181);
            this.table.TabIndex = 0;
            this.table.TableModel = this.tableModel;
            this.table.UnfocusedBorderColor = System.Drawing.Color.Black;
            this.table.CellClick += new jingxian.ui.controls.XPTable.Events.CellMouseEventHandler(this.table_CellClick);
            this.table.SelectionChanged += new jingxian.ui.controls.XPTable.Events.SelectionEventHandler(this.table_SelectionChanged);
            // 
            // columnModel
            // 
            this.columnModel.Columns.AddRange(new jingxian.ui.controls.XPTable.Models.Column[] {
            this.mailColumn,
            this.levelColumn,
            this.senderColumn,
            this.readStateColumn,
            this.subjectColumn,
            this.isspamColumn,
            this.dateColumn,
            this.sizeColumn});
            // 
            // mailColumn
            // 
            this.mailColumn.Alignment = jingxian.ui.controls.XPTable.Models.ColumnAlignment.Center;
            this.mailColumn.Image = global::jingxian.ui.Properties.Resources.mail;
            this.mailColumn.IsTextTrimmed = false;
            this.mailColumn.Width = 30;
            // 
            // levelColumn
            // 
            this.levelColumn.Alignment = jingxian.ui.controls.XPTable.Models.ColumnAlignment.Center;
            this.levelColumn.Image = global::jingxian.ui.Properties.Resources.red_flag;
            this.levelColumn.IsTextTrimmed = false;
            this.levelColumn.Width = 25;
            // 
            // senderColumn
            // 
            this.senderColumn.IsTextTrimmed = false;
            this.senderColumn.Text = "发件人";
            this.senderColumn.Width = 100;
            // 
            // readStateColumn
            // 
            this.readStateColumn.Alignment = jingxian.ui.controls.XPTable.Models.ColumnAlignment.Center;
            this.readStateColumn.Image = global::jingxian.ui.Properties.Resources.big_point;
            this.readStateColumn.IsTextTrimmed = false;
            this.readStateColumn.Width = 20;
            // 
            // subjectColumn
            // 
            this.subjectColumn.IsTextTrimmed = false;
            this.subjectColumn.Text = "主题";
            this.subjectColumn.Width = 400;
            // 
            // isspamColumn
            // 
            this.isspamColumn.Alignment = jingxian.ui.controls.XPTable.Models.ColumnAlignment.Center;
            this.isspamColumn.Image = global::jingxian.ui.Properties.Resources.spam;
            this.isspamColumn.IsTextTrimmed = false;
            this.isspamColumn.Width = 25;
            // 
            // dateColumn
            // 
            this.dateColumn.IsTextTrimmed = false;
            this.dateColumn.Text = "日期";
            // 
            // sizeColumn
            // 
            this.sizeColumn.IsTextTrimmed = false;
            this.sizeColumn.Text = "大小";
            // 
            // MessageExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_splitter);
            this.Name = "MessageExplorer";
            this.Size = new System.Drawing.Size(615, 350);
            this.m_splitter.Panel1.ResumeLayout(false);
            this.m_splitter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.table)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer m_splitter;
        private jingxian.ui.controls.XPTable.Models.Table table;
        private jingxian.ui.controls.XPTable.Models.ColumnModel columnModel;
        private jingxian.ui.controls.XPTable.Models.TableModel tableModel;
        private jingxian.ui.controls.XPTable.Models.ImageColumn mailColumn;
        private jingxian.ui.controls.XPTable.Models.ImageColumn levelColumn;
        private jingxian.ui.controls.XPTable.Models.TextColumn senderColumn;
        private jingxian.ui.controls.XPTable.Models.ImageColumn readStateColumn;
        private jingxian.ui.controls.XPTable.Models.TextColumn subjectColumn;
        private jingxian.ui.controls.XPTable.Models.ImageColumn isspamColumn;
        private jingxian.ui.controls.XPTable.Models.TextColumn dateColumn;
        private jingxian.ui.controls.XPTable.Models.TextColumn sizeColumn;
    }
}
