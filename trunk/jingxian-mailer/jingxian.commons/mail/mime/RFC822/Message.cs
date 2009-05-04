
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC822
{
    class Message : IMailMessage
    {
        protected MailAddressCollection m_ToAddresses;
        protected MailAddressCollection m_CarbonCopyAddresses;
        protected MailAddressCollection m_BlindCarbonCopyAddresses;

        protected string m_Source;
        protected IList<Field> m_Fields;
        protected string m_Body;

        public IList<Field> Fields
        {
            get
            {
                return m_Fields;
            }
            set
            {
                m_Fields = value;
            }
        }        

        public System.Net.Mail.MailAddress From
        {
            get
            {
                foreach (Field field in m_Fields)
                {
                    if (field.Name != null)
                    {
                        if (field.Name.ToLower().Equals("from"))
                        {
                            MailAddress address;
                            try
                            {
                                address = new MailAddress(field.Body);
                            }
                            catch (FormatException)
                            {
                                string sAddress = FieldParserProxy.ParseAddress(field.Body);
                                address = new MailAddress(sAddress);
                            }
                            return address;                            
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Field f in m_Fields)
                {
                    if (f.Name != null)
                    {
                        if (f.Name.ToLower().Equals("from"))
                        {
                            f.Body = value.Address;
                            return;
                        }
                    }
                }
                Field nField = new Field();
                nField.Name = "From";
                nField.Body = value.Address;
                m_Fields.Add(nField);
            }
        }

        public MailAddressCollection To
        {
            get
            {
                if(m_ToAddresses == null)
                    m_ToAddresses = LoadTo();
                return m_ToAddresses;
            }
            set
            {
                m_ToAddresses = value;
            }
        }

        public MailAddressCollection CarbonCopy
        {
            get
            {
                if (m_CarbonCopyAddresses == null)
                    m_CarbonCopyAddresses = LoadCarbonCopy();
                return m_CarbonCopyAddresses;
            }
            set
            {
                m_CarbonCopyAddresses = value;
            }
        }

        public MailAddressCollection BlindCarbonCopy
        {
            get
            {
                if (m_BlindCarbonCopyAddresses == null)
                    m_BlindCarbonCopyAddresses = LoadBlindCarbonCopy();
                return m_BlindCarbonCopyAddresses;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string TextMessage
        {
            get
            {
                return this.m_Body;
            }
            set
            {
                this.m_Body = value;
            }
        }

        public string Subject
        {
             get
            {
                foreach (Field f in m_Fields)
                {
                    if (f.Name != null)
                    {
                        if (f.Name.ToLower().Equals("subject"))
                        {
                            return f.Body;
                        }
                    }
                }
                return null;
            }
            set
            {
                foreach (Field f in m_Fields)
                {
                    if (f.Name != null)
                    {
                        if (f.Name.ToLower().Equals("subject"))
                        {
                            f.Body = value;
                            return;
                        }
                    }
                }
                Field nField = new Field();
                nField.Name = "Subject";
                nField.Body = value;
                m_Fields.Add(nField);
            }
        }       

        public string Source
        {
            get
            {
                return m_Source;
            }
            set
            {
                m_Source = value;
            }
        }

        public bool IsNull()
        {
            return false;
        }

        private MailAddressCollection LoadTo()
        {
            foreach (Field field in m_Fields)
            {
                if (field.Name != null)
                {
                    if (field.Name.ToLower().Equals("to"))
                    {
                        IFieldParserProxy parser =
                           FieldParserProxy.Getinstance();
                        if (parser.AddrSpec.IsMatch(field.Body))
                        {
                            MailAddressCollection addresses = new MailAddressCollection();
                            MatchCollection matches =
                                parser.AddrSpec.Matches(field.Body);
                            foreach (Match match in matches)
                            {
                                addresses.Add(match.Value);
                            }
                            return addresses;
                        }
                    }
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
                    IFieldParserProxy parser =
                       FieldParserProxy.Getinstance();
                    if (parser.AddrSpec.IsMatch(field.Body))
                    {
                        MailAddressCollection addresses = new MailAddressCollection();
                        MatchCollection matches =
                            parser.AddrSpec.Matches(field.Body);
                        foreach (Match match in matches)
                        {
                            addresses.Add(match.Value);
                        }
                        return addresses;
                    }
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
                    IFieldParserProxy parser =
                       FieldParserProxy.Getinstance();
                    if (parser.AddrSpec.IsMatch(field.Body))
                    {
                        MailAddressCollection addresses = new MailAddressCollection();
                        MatchCollection matches =
                            parser.AddrSpec.Matches(field.Body);
                        foreach (Match match in matches)
                        {
                            addresses.Add(match.Value);
                        }
                        return addresses;
                    }
                }
            }
            return null;
        }
    }
}
