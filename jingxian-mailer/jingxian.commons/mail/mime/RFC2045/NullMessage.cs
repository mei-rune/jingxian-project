
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace jingxian.mail.mime.RFC2045
{
    public class NullMessage:INullable, IMimeMailMessage
    {
        public static MailAddress NullAddress = new MailAddress("null@null.null","这是一个空邮件的地址");
        private IDictionary<string, string> m_Body;
        private IList<IAttachment> m_Attachments;


        public NullMessage()
        {
            m_Body = new Dictionary<string, string>();
            m_Attachments = new List<IAttachment>();
        }

        public bool IsNull()
        {
            return true;
        }

        public IDictionary<string, string> Body
        {
            get{ return m_Body;}
            set{}
        }

        public IList<IAttachment> Attachments
        {
            get{ return m_Attachments;}
            set{}
        }

        public System.Net.Mail.MailAddress From
        {
            get { return NullAddress; }
            set { }
        }

        public MailAddressCollection To
        {
            get
            {
                MailAddressCollection addresses = new MailAddressCollection();
                addresses.Add( NullAddress );
                return addresses;
            }
            set { }
        }

        public MailAddressCollection CarbonCopy
        {
            get
            {
                MailAddressCollection addresses = new MailAddressCollection();
                addresses.Add(NullAddress);
                return addresses;                
            }
            set { }
        }

        public MailAddressCollection BlindCarbonCopy
        {
            get
            {
                MailAddressCollection addresses = new MailAddressCollection();
                addresses.Add(NullAddress);
                return addresses;                                
            }
            set { }
        }

        public string Subject
        {
            get { return "空邮件"; }
            set { }
        }

        public string Source
        {
            get { return ""; }
            set { }
        }

        public string TextMessage
        {
            get { return ""; }
            set { }
        }

        public IList<AlternateView> Views
        {
            get 
            {
                IList<AlternateView> views = new List<AlternateView>();
                return views;
            }
            set { }
        }

        public MailMessage ToMailMessage()
        {
            return new MailMessage();
        }

        
        public IList<IMimeMailMessage> Messages
        {
            get
            {
                IList<IMimeMailMessage> list = new List<IMimeMailMessage>();
                list.Add(this);
                return list;
            }
            set { }
        }
    }
}
