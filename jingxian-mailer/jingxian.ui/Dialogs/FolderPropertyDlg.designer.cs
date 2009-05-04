
namespace jingxian.ui.dialogs
{
    partial class FolderPropertyDlg
    {
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.TabPage tabArchive;
        private ArchivingSettingPanel archivingSettingPane;
        private System.Windows.Forms.CheckBox chkUseDefaultSettings;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private jingxian.ui.controls.ImageSelector iconComboBox;
        private System.Windows.Forms.TextBox txtFolderCaption;
        private System.Windows.Forms.Button btnBrowserForIcon;
        

        private System.ComponentModel.Container components = null;
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
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label1;
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabArchive = new System.Windows.Forms.TabPage();
            this.archivingSettingPane = new jingxian.ui.dialogs.ArchivingSettingPanel();
            this.chkUseDefaultSettings = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.iconComboBox = new jingxian.ui.controls.ImageSelector();
            this.txtFolderCaption = new System.Windows.Forms.TextBox();
            this.btnBrowserForIcon = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.tabArchive.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            label5.Location = new System.Drawing.Point(13, 46);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 25);
            label5.TabIndex = 1;
            label5.Text = "图标:";
            // 
            // label1
            // 
            label1.Location = new System.Drawing.Point(13, 17);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(39, 25);
            label1.TabIndex = 1;
            label1.Text = "标题:";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOk.Location = new System.Drawing.Point(248, 186);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 24);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(344, 186);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 24);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabArchive
            // 
            this.tabArchive.Controls.Add(this.archivingSettingPane);
            this.tabArchive.Controls.Add(this.chkUseDefaultSettings);
            this.tabArchive.Location = new System.Drawing.Point(4, 21);
            this.tabArchive.Name = "tabArchive";
            this.tabArchive.Size = new System.Drawing.Size(434, 144);
            this.tabArchive.TabIndex = 1;
            this.tabArchive.Text = "档案";
            this.tabArchive.UseVisualStyleBackColor = true;
            // 
            // archivingSettingPane
            // 
            this.archivingSettingPane.Action = null;
            this.archivingSettingPane.Dock = System.Windows.Forms.DockStyle.Fill;
            this.archivingSettingPane.Location = new System.Drawing.Point(0, 0);
            this.archivingSettingPane.Name = "archivingSettingPane";
            this.archivingSettingPane.Size = new System.Drawing.Size(434, 144);
            this.archivingSettingPane.TabIndex = 8;
            this.archivingSettingPane.Time = "30";
            // 
            // chkUseDefaultSettings
            // 
            this.chkUseDefaultSettings.Checked = true;
            this.chkUseDefaultSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseDefaultSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkUseDefaultSettings.Location = new System.Drawing.Point(10, 9);
            this.chkUseDefaultSettings.Name = "chkUseDefaultSettings";
            this.chkUseDefaultSettings.Size = new System.Drawing.Size(303, 25);
            this.chkUseDefaultSettings.TabIndex = 7;
            this.chkUseDefaultSettings.Text = "使用缺省设置...";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabArchive);
            this.tabControl.Location = new System.Drawing.Point(10, 9);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(442, 169);
            this.tabControl.TabIndex = 8;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.Transparent;
            this.tabGeneral.Controls.Add(this.iconComboBox);
            this.tabGeneral.Controls.Add(this.txtFolderCaption);
            this.tabGeneral.Controls.Add(label1);
            this.tabGeneral.Controls.Add(label5);
            this.tabGeneral.Controls.Add(this.btnBrowserForIcon);
            this.tabGeneral.Location = new System.Drawing.Point(4, 21);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(434, 144);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "基本";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // iconComboBox
            // 
            this.iconComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.iconComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iconComboBox.FormattingEnabled = true;
            this.iconComboBox.ItemHeight = 17;
            this.iconComboBox.Location = new System.Drawing.Point(58, 46);
            this.iconComboBox.Name = "iconComboBox";
            this.iconComboBox.Size = new System.Drawing.Size(54, 23);
            this.iconComboBox.TabIndex = 9;
            // 
            // txtFolderCaption
            // 
            this.txtFolderCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderCaption.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtFolderCaption.Location = new System.Drawing.Point(58, 17);
            this.txtFolderCaption.Name = "txtFolderCaption";
            this.txtFolderCaption.Size = new System.Drawing.Size(345, 21);
            this.txtFolderCaption.TabIndex = 0;
            // 
            // btnBrowserForIcon
            // 
            this.btnBrowserForIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowserForIcon.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBrowserForIcon.Location = new System.Drawing.Point(118, 44);
            this.btnBrowserForIcon.Name = "btnBrowserForIcon";
            this.btnBrowserForIcon.Size = new System.Drawing.Size(29, 25);
            this.btnBrowserForIcon.TabIndex = 7;
            this.btnBrowserForIcon.Text = "...";
            this.btnBrowserForIcon.Click += new System.EventHandler(this.btnBrowserForIcon_Click);
            // 
            // FolderPropertyDlg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(462, 217);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "FolderPropertyDlg";
            this.ShowInTaskbar = false;
            this.Text = "文件夹属性...";
            this.tabArchive.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
    }
}

