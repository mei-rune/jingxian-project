namespace PopConsoleTest
{
    partial class MessageForm
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
            this.SenderDescriptionlabel = new System.Windows.Forms.Label();
            this.SenderLabel = new System.Windows.Forms.Label();
            this.SubjectDescriptionlabel = new System.Windows.Forms.Label();
            this.MessagetextBox = new System.Windows.Forms.TextBox();
            this.Subjectlabel = new System.Windows.Forms.Label();
            this.AttachmentsToolStrip = new System.Windows.Forms.ToolStrip();
            this.AttachmentsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SenderDescriptionlabel
            // 
            this.SenderDescriptionlabel.AutoSize = true;
            this.SenderDescriptionlabel.Location = new System.Drawing.Point(13, 13);
            this.SenderDescriptionlabel.Name = "SenderDescriptionlabel";
            this.SenderDescriptionlabel.Size = new System.Drawing.Size(47, 15);
            this.SenderDescriptionlabel.TabIndex = 0;
            this.SenderDescriptionlabel.Text = "Sender";
            // 
            // SenderLabel
            // 
            this.SenderLabel.AutoSize = true;
            this.SenderLabel.Location = new System.Drawing.Point(67, 13);
            this.SenderLabel.Name = "SenderLabel";
            this.SenderLabel.Size = new System.Drawing.Size(41, 15);
            this.SenderLabel.TabIndex = 1;
            this.SenderLabel.Text = "label2";
            // 
            // SubjectDescriptionlabel
            // 
            this.SubjectDescriptionlabel.AutoSize = true;
            this.SubjectDescriptionlabel.Location = new System.Drawing.Point(13, 32);
            this.SubjectDescriptionlabel.Name = "SubjectDescriptionlabel";
            this.SubjectDescriptionlabel.Size = new System.Drawing.Size(48, 15);
            this.SubjectDescriptionlabel.TabIndex = 2;
            this.SubjectDescriptionlabel.Text = "Subject";
            // 
            // MessagetextBox
            // 
            this.MessagetextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.MessagetextBox.Location = new System.Drawing.Point(0, 79);
            this.MessagetextBox.Multiline = true;
            this.MessagetextBox.Name = "MessagetextBox";
            this.MessagetextBox.Size = new System.Drawing.Size(498, 288);
            this.MessagetextBox.TabIndex = 3;
            // 
            // Subjectlabel
            // 
            this.Subjectlabel.AutoSize = true;
            this.Subjectlabel.Location = new System.Drawing.Point(67, 33);
            this.Subjectlabel.Name = "Subjectlabel";
            this.Subjectlabel.Size = new System.Drawing.Size(41, 15);
            this.Subjectlabel.TabIndex = 4;
            this.Subjectlabel.Text = "label1";
            // 
            // AttachmentsToolStrip
            // 
            this.AttachmentsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.AttachmentsToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.AttachmentsToolStrip.Location = new System.Drawing.Point(100, 51);
            this.AttachmentsToolStrip.Name = "AttachmentsToolStrip";
            this.AttachmentsToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.AttachmentsToolStrip.Size = new System.Drawing.Size(102, 25);
            this.AttachmentsToolStrip.TabIndex = 5;
            this.AttachmentsToolStrip.Text = "AttachmentsToolStrip";
            // 
            // AttachmentsLabel
            // 
            this.AttachmentsLabel.AutoSize = true;
            this.AttachmentsLabel.Location = new System.Drawing.Point(13, 58);
            this.AttachmentsLabel.Name = "AttachmentsLabel";
            this.AttachmentsLabel.Size = new System.Drawing.Size(74, 15);
            this.AttachmentsLabel.TabIndex = 6;
            this.AttachmentsLabel.Text = "Attachments";
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 367);
            this.Controls.Add(this.AttachmentsLabel);
            this.Controls.Add(this.AttachmentsToolStrip);
            this.Controls.Add(this.Subjectlabel);
            this.Controls.Add(this.MessagetextBox);
            this.Controls.Add(this.SubjectDescriptionlabel);
            this.Controls.Add(this.SenderLabel);
            this.Controls.Add(this.SenderDescriptionlabel);
            this.Name = "MessageForm";
            this.Text = "MessageForm";
            this.Load += new System.EventHandler(this.MessageForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SenderDescriptionlabel;
        private System.Windows.Forms.Label SenderLabel;
        private System.Windows.Forms.Label SubjectDescriptionlabel;
        private System.Windows.Forms.TextBox MessagetextBox;
        private System.Windows.Forms.Label Subjectlabel;
        private System.Windows.Forms.ToolStrip AttachmentsToolStrip;
        private System.Windows.Forms.Label AttachmentsLabel;
    }
}