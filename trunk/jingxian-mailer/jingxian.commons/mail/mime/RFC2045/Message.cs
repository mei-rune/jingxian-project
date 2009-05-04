
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Net.Mail;
using System.Collections;
using System.IO;

namespace jingxian.mail.mime.RFC2045
{
    using jingxian.mail.mime.RFC2183;

    class Message : MultipartEntity, IMailMessage, IMimeMailMessage, INullable
    {
        private string m_Source;
        private MailAddress m_From;
        private MailAddress m_Sender;
        private MailAddressCollection m_To;
        private MailAddressCollection m_CarbonCopy;
        private MailAddressCollection m_BlindCarbonCopy;
        private IList<AlternateView> m_Views;
        
        private string m_Text;
        private IDictionary<string, string> m_Body;
        private IList<IAttachment> m_Attachments;
        private IList<IMimeMailMessage> m_Messages;        
        private string m_Subject;

        public Message()
        {
            m_Views = new List<AlternateView>();
        }

        public IList<AlternateView> Views
        {
            get 
            {
                if (m_Views.Count == 0)
                    LoadViews();
                return m_Views; 
            }
            set { m_Views = value; }
        }

        public MailAddress From
        {
            get
            {
                if (m_From == null)
                {
                    m_From = LoadFrom();
                }

                return m_From;
            }
            set
            {
                m_From = value;
            }
        }

        public MailAddress Sender
        {
            get 
            {
                if (m_Sender == null)
                {
                    m_Sender = this.From;
                }
                return m_Sender;
            }
            set
            {
                m_Sender = value;
                m_From = value;
            }

        }

        public MailAddressCollection To
        {
            get
            {
                if (m_To == null)
                {
                    m_To = LoadTo();
                }
                return m_To;
            }
            set
            {
                m_To = value;
            }
        }
                
        public MailAddressCollection CarbonCopy
        {
            get
            {
                if (m_CarbonCopy == null)
                {
                    m_CarbonCopy = LoadCarbonCopy();
                }
                return m_CarbonCopy;
            }
            set
            {
                m_CarbonCopy = value;
            }
        }

        public MailAddressCollection BlindCarbonCopy
        {
            get
            {
                if (m_BlindCarbonCopy == null)
                    m_BlindCarbonCopy = LoadBlindCarbonCopy();
                return m_BlindCarbonCopy;
            }
            set
            {
                m_BlindCarbonCopy = value;
            }
        }     

        public string Subject
        {
            get
            {
                if (m_Subject == null)
                {
                    LoadSubject();
                }
                return m_Subject;
            }
            set
            {
                m_Subject = value;
            }
        }

        public string TextMessage
        {
            get 
            {
                if (m_Text == null)
                {
                    IEnumerator<KeyValuePair<string, string>> eNum = this.Body.GetEnumerator();
                    eNum.MoveNext();
                    m_Text = eNum.Current.Value;
                }
                return m_Text;
            }
            set { m_Text = value; }
        }

        public string Source
        {
            get { return m_Source; }
            set { m_Source = value; }
        }

        public new IDictionary<string, string> Body
        {
            get
            {
                if (m_Body == null)
                {
                    m_Body = new Dictionary<string, string>();
                    LoadBody(this);
                }
                return m_Body;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<IAttachment> Attachments
        {
            get
            {
                if (m_Attachments == null)
                {
                    m_Attachments = new List<IAttachment>();
                    LoadAttachments(this);
                }
                    
                return m_Attachments;
            }
            set
            {
                m_Attachments = value;
            }
        }

        public IList<IMimeMailMessage> Messages
        {
            get
            {
                if (m_Messages == null)
                {
                    m_Messages = new List<IMimeMailMessage>();
                    LoadMessages(this);
                }
                return m_Messages;
            }
            set { m_Messages = value; }
        }

        public virtual bool IsNull()
        {
            return false;
        }

        private static string ToString(byte[] data)
        {
            if (data == null)
                return string.Empty;

            char[] stringdata = new char[data.Length];
            for (int i = 0; i < data.Length; i++)
                stringdata[i] = Convert.ToChar(data[i]);
            return new string(stringdata);
        }

        private void LoadSubject()
        {
            foreach ( RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("subject"))
                {
                    m_Subject = field.Body; 
                }                   
            }            
        }

        private void LoadBody(Entity parent)
        {
            if (parent is MultipartEntity)
            {
                MultipartEntity mpe = parent as MultipartEntity;
                foreach (Entity child in mpe.BodyParts)
                {
                    if(child != parent && !(child is Message))
                        LoadBody(child);
                }
            }

            ContentTypeField contentTypeField = null;
            ContentDispositionField contentDispositionField = null;

            foreach (RFC822.Field field in parent.Fields)
            {                
                if (field is ContentTypeField)
                {
                    ContentTypeField contentField = field as ContentTypeField;
                    if (contentField.Type.ToLower().Equals("text"))
                    {
                        contentTypeField = contentField;
                    }
                }

                if (field is ContentDispositionField)
                {
                    contentDispositionField = field as ContentDispositionField;                    
                }
            }

            if (contentTypeField != null && 
                (contentDispositionField == null || 
                contentDispositionField.Disposition.ToLower().Equals("inline")))
            {
                string text = ToString(parent.Body);
                m_Body.Add(contentTypeField.Type + "/" + contentTypeField.SubType,
                    text);
            }
        }

        private void LoadMessages(IEntity parent)
        {
            if (parent is MultipartEntity)
            {
                MultipartEntity mpe = parent as MultipartEntity;
                foreach (Entity child in mpe.BodyParts)
                {
                    if (child is MultipartEntity && !(child is Message))
                    {
                        LoadMessages(child);
                    }
                    else if (child is Message)
                    {
                        Message message = child as Message;
                        m_Messages.Add(message);                        
                    }   
                }
            }
        }

        private void LoadAttachments(IEntity parent)
        {
            if (parent is MultipartEntity)
            {
                MultipartEntity mpe = parent as MultipartEntity;
                foreach (Entity entity in mpe.BodyParts)
                {
                    if (entity is MultipartEntity && !(entity is Message))
                    {
                        LoadAttachments(entity);
                    }
                    else if (!(entity is MultipartEntity) && !(entity is Message))
                    { 
                        ContentDispositionField dispositionField = null;
                        ContentTypeField contentTypeField = null;

                        foreach (RFC822.Field field in entity.Fields)
                        {
                            if (field is RFC2183.ContentDispositionField)
                            {
                                dispositionField = field as ContentDispositionField;
                            }

                            if(field is ContentTypeField)
                            {
                                contentTypeField = field as ContentTypeField;
                            }
                        }

                        if(dispositionField != null && contentTypeField != null)
                        {
                            IAttachment attachment = new Attachment();
                            attachment.Disposition = dispositionField.Disposition;
                            attachment.Name = dispositionField.Parameters["filename"];
                            attachment.Data = entity.Body;
                            attachment.Type = contentTypeField.Type;
                            attachment.SubType = contentTypeField.SubType;
                            m_Attachments.Add(attachment);
                        }                                      
                    }
                }                    
            }            
        }

        private MailAddress LoadFrom()
        {
            foreach (RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("from"))
                {
                    MailAddress address ;
                    try
                    {
                        address = new MailAddress(field.Body);
                    }
                    catch(FormatException)
                    {
                        string sAddress = FieldParserProxy.ParseAddress(field.Body);
                        address = new MailAddress(sAddress);
                    }                    
                    return address;
                }
            }
            return null;
        }

        private MailAddressCollection LoadTo()
        {
            foreach (RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("to"))
                {
                    return LoadAdressCollection(field.Body);                    
                }
                 
            }
            return null;
        }

        private MailAddressCollection LoadCarbonCopy()
        {
            foreach (RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("cc"))
                {
                    return LoadAdressCollection(field.Body);                    
                }                    
            }
            return null;
        }

        private MailAddressCollection LoadBlindCarbonCopy()
        {
            foreach (RFC822.Field field in m_Fields)
            {
                if (field.Name.ToLower().Equals("bcc"))
                {
                    return LoadAdressCollection(field.Body);
                }
            }
            return null;       
        }

        private MailAddressCollection LoadAdressCollection(string fieldBody)
        {
            IFieldParserProxy parser =
                       jingxian.mail.mime.FieldParserProxy.Getinstance();
            if (parser.AddrSpec.IsMatch(fieldBody))
            {
                MailAddressCollection addresses = new MailAddressCollection();
                try
                {
                    addresses.Add(fieldBody);
                }
                catch (FormatException)
                {
                    MatchCollection matches =
                    parser.AddrSpec.Matches(fieldBody);
                    foreach (Match match in matches)
                    {
                        addresses.Add(match.Value);
                    }
                }
                return addresses;
            }

            return null;
        }

        private void LoadViews()
        {
            foreach (KeyValuePair<string, string> enu in Body)
            {
                MemoryStream stream =
                    new MemoryStream(System.Text.Encoding.ASCII.GetBytes(enu.Value));
                AlternateView view = new AlternateView(stream, enu.Key);
                m_Views.Add(view);
            }
        }

        public MailMessage ToMailMessage()
        {
            MailMessage message = new MailMessage();
            if(To != null)
                message.To.Add(this.To.ToString());
            if (CarbonCopy != null)
                message.CC.Add(this.CarbonCopy.ToString());
            if(From != null)
                message.Sender = new MailAddress(this.From.ToString());
            if(Subject != null)
                message.Subject = this.Subject.ToString();
            if(TextMessage != null)
                message.Body = this.TextMessage.ToString();

            foreach (IAttachment attachment in this.Attachments)
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream(attachment.Data);
                message.Attachments.Add(new System.Net.Mail.Attachment(stream, attachment.Name, 
                    attachment.Type + "/" + attachment.SubType));
            }

            foreach (AlternateView view in Views)
            {
                AlternateView aView = new AlternateView(view.ContentStream, view.ContentType.MediaType);
                message.AlternateViews.Add(aView);
            }

            return message;
        }
    }
}
