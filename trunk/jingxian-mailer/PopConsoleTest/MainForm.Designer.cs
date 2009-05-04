namespace PopConsoleTest
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.CommandTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.BytesLabel = new System.Windows.Forms.Label();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.ListButton = new System.Windows.Forms.Button();
            this.MessagedataGridView = new System.Windows.Forms.DataGridView();
            this.Sender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.View = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Text = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MessagedataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(96, 99);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(589, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // CommandTextBox
            // 
            this.CommandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandTextBox.Location = new System.Drawing.Point(511, 159);
            this.CommandTextBox.Name = "CommandTextBox";
            this.CommandTextBox.Size = new System.Drawing.Size(168, 552);
            this.CommandTextBox.TabIndex = 1;
            this.CommandTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(508, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // ClearButton
            // 
            this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearButton.Location = new System.Drawing.Point(511, 717);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(168, 23);
            this.ClearButton.TabIndex = 5;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // BytesLabel
            // 
            this.BytesLabel.AutoSize = true;
            this.BytesLabel.Location = new System.Drawing.Point(93, 125);
            this.BytesLabel.Name = "BytesLabel";
            this.BytesLabel.Size = new System.Drawing.Size(41, 15);
            this.BytesLabel.TabIndex = 6;
            this.BytesLabel.Text = "label2";
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Location = new System.Drawing.Point(395, 125);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(41, 15);
            this.SizeLabel.TabIndex = 7;
            this.SizeLabel.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Progress";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(426, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Current state";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(18, 9);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 12;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // ListButton
            // 
            this.ListButton.Location = new System.Drawing.Point(18, 39);
            this.ListButton.Name = "ListButton";
            this.ListButton.Size = new System.Drawing.Size(75, 23);
            this.ListButton.TabIndex = 13;
            this.ListButton.Text = "List";
            this.ListButton.UseVisualStyleBackColor = true;
            this.ListButton.Click += new System.EventHandler(this.ListButton_Click);
            // 
            // MessagedataGridView
            // 
            this.MessagedataGridView.AllowUserToAddRows = false;
            this.MessagedataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.MessagedataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessagedataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sender,
            this.Subject,
            this.View,
            this.Delete,
            this.Source,
            this.Text});
            this.MessagedataGridView.Location = new System.Drawing.Point(18, 159);
            this.MessagedataGridView.Name = "MessagedataGridView";
            this.MessagedataGridView.ReadOnly = true;
            this.MessagedataGridView.Size = new System.Drawing.Size(487, 552);
            this.MessagedataGridView.TabIndex = 14;
            this.MessagedataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MessagedataGridView_CellContentClick);
            // 
            // Sender
            // 
            this.Sender.DataPropertyName = "From";
            this.Sender.Frozen = true;
            this.Sender.HeaderText = "Sender";
            this.Sender.Name = "Sender";
            this.Sender.ReadOnly = true;
            // 
            // Subject
            // 
            this.Subject.DataPropertyName = "Subject";
            this.Subject.Frozen = true;
            this.Subject.HeaderText = "Subject";
            this.Subject.Name = "Subject";
            this.Subject.ReadOnly = true;
            // 
            // View
            // 
            this.View.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.View.Frozen = true;
            this.View.HeaderText = "View";
            this.View.Name = "View";
            this.View.ReadOnly = true;
            this.View.Text = "View";
            this.View.UseColumnTextForButtonValue = true;
            // 
            // Delete
            // 
            this.Delete.Frozen = true;
            this.Delete.HeaderText = "Delete";
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "Delete";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // Source
            // 
            this.Source.DataPropertyName = "Source";
            this.Source.Frozen = true;
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            this.Source.Visible = false;
            // 
            // Text
            // 
            this.Text.DataPropertyName = "TextMessage";
            this.Text.Frozen = true;
            this.Text.HeaderText = "Text";
            this.Text.Name = "Text";
            this.Text.ReadOnly = true;
            this.Text.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(511, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "Command history";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 752);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.MessagedataGridView);
            this.Controls.Add(this.ListButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.BytesLabel);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CommandTextBox);
            this.Controls.Add(this.progressBar1);
            this.Name = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.MessagedataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox CommandTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Label BytesLabel;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button ListButton;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView MessagedataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sender;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subject;
        private System.Windows.Forms.DataGridViewButtonColumn View;
        private System.Windows.Forms.DataGridViewButtonColumn Delete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn Text;
    }
}