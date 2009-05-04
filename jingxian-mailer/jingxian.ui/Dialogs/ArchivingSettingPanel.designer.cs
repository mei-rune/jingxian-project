
namespace jingxian.ui.dialogs
{
    partial class ArchivingSettingPanel
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
            this.grpArchiveReadMessageSettings = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_MessageTime = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_MessageAction = new System.Windows.Forms.ComboBox();
            this.grpArchiveReadMessageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpArchiveReadMessageSettings
            // 
            this.grpArchiveReadMessageSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpArchiveReadMessageSettings.Controls.Add(this.label3);
            this.grpArchiveReadMessageSettings.Controls.Add(this.m_MessageTime);
            this.grpArchiveReadMessageSettings.Controls.Add(this.label4);
            this.grpArchiveReadMessageSettings.Controls.Add(this.m_MessageAction);
            this.grpArchiveReadMessageSettings.Location = new System.Drawing.Point(3, 4);
            this.grpArchiveReadMessageSettings.Name = "grpArchiveReadMessageSettings";
            this.grpArchiveReadMessageSettings.Size = new System.Drawing.Size(414, 133);
            this.grpArchiveReadMessageSettings.TabIndex = 9;
            this.grpArchiveReadMessageSettings.TabStop = false;
            this.grpArchiveReadMessageSettings.Text = "消息设置:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(328, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "消息保留的天数:";
            // 
            // m_MessageTime
            // 
            this.m_MessageTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_MessageTime.Items.AddRange(new object[] {
            "永远",
            "30",
            "60",
            "120",
            "365"});
            this.m_MessageTime.Location = new System.Drawing.Point(56, 37);
            this.m_MessageTime.Name = "m_MessageTime";
            this.m_MessageTime.Size = new System.Drawing.Size(350, 20);
            this.m_MessageTime.TabIndex = 3;
            this.m_MessageTime.Text = "30";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(8, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(398, 37);
            this.label4.TabIndex = 4;
            this.label4.Text = "执行的动作: (当消息数超过指定数量或消息目录中允许的时间，将执行这个动作)";
            // 
            // m_MessageAction
            // 
            this.m_MessageAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_MessageAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_MessageAction.Location = new System.Drawing.Point(56, 103);
            this.m_MessageAction.Name = "m_MessageAction";
            this.m_MessageAction.Size = new System.Drawing.Size(351, 20);
            this.m_MessageAction.TabIndex = 5;
            // 
            // ArchivingSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpArchiveReadMessageSettings);
            this.Name = "ArchivingSettingPanel";
            this.Size = new System.Drawing.Size(423, 145);
            this.grpArchiveReadMessageSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpArchiveReadMessageSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_MessageTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_MessageAction;
    }
}
