using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using jingxian.mail.mime;
using jingxian.mail.mime.RFC2045;

namespace PopConsoleTest
{
    public partial class MessageForm : Form
    {        
        private IMimeMailMessage m_Message;

        public MessageForm(IMimeMailMessage message)
        {
            m_Message = message;
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            
            base.OnLoad(e);
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {
            SenderLabel.Text = m_Message.From.ToString();
            Subjectlabel.Text = m_Message.Subject;
            MessagetextBox.Text = m_Message.TextMessage;

            foreach (IAttachment attachment in m_Message.Attachments)
            {
                AttachmentsToolStrip.Items.Add(attachment.Name, 
                    Images.attachments, new EventHandler(Attachment_clicked));
            }

            foreach (IMailMessage embedded in m_Message.Messages)
            {
                AttachmentsToolStrip.Items.Add(embedded.Subject,
                    Images.embedded_message, new EventHandler(EmbeddedMessage_clicked));
            }
        }

        private void Attachment_clicked(object sender, EventArgs e)
        {
            ToolStripItem tsi = sender as ToolStripItem;
            if (tsi != null)
            {
                foreach (IAttachment attachment in m_Message.Attachments)
                {
                    if (attachment.Name.Equals(tsi.Text))
                    {
                        SaveFileDialog s = new SaveFileDialog();
                        s.FileName = tsi.Text;                        
                        if (s.ShowDialog(this) == DialogResult.OK)                        
                            SaveFile(s.FileName, attachment.Data);
                    }
                }                
            }
        }

        private void EmbeddedMessage_clicked(object sender, EventArgs e)
        {
            ToolStripItem tsi = sender as ToolStripItem;
            if (tsi != null)
            {
                foreach (IMimeMailMessage embedded in m_Message.Messages)
                {
                    if (embedded.Subject.Equals(tsi.Text))
                    {
                        MessageForm form = new MessageForm(embedded);
                        form.Show(this);
                    }
                }
            }
        }

        private void SaveFile(string path, byte[] data)
        {            
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            System.IO.FileStream stream = System.IO.File.Create(path);
            stream.Write(data, 0, data.Length);
            stream.Close();
            stream.Dispose();
        }
    }
}