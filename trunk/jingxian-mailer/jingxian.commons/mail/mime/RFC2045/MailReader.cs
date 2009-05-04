
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace jingxian.mail.mime.RFC2045
{
    public class MailReader : RFC822.MailReader, IMailReader
    {        
        private IList<RFC2045.IDecoder> m_Decoders;
        protected new IFieldParserProxy m_FieldParser;

        public IList<RFC2045.IDecoder> Decoders
        {
            get { return m_Decoders; }
            set { m_Decoders = value; }
        }

        public MailReader()
        {   
            m_Decoders = new List<RFC2045.IDecoder>();
            m_Decoders.Add(new QuotedPrintableDecoder());
            m_Decoders.Add(new Base64Decoder());            
            m_FieldParser = FieldParserProxy.Getinstance();
            m_FieldParser.CompilePattern();
        }

        public MailReader(IList<IDecoder> decoders)
            : this()
        {
            foreach (IDecoder decoder in decoders)
                m_Decoders.Add(decoder);
        }

        public MailReader(IList<IDecoder> decoders, IFieldParserProxy proxy):this(decoders)
        {
            m_FieldParser = proxy;            
            m_FieldParser.CompilePattern();
        }

        public new IMailMessage Read(ref Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            return ReadMimeMessage(ref dataStream, endOfMessageCriteria) as IMailMessage;
        }      

        public IMimeMailMessage ReadMimeMessage(ref Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            m_EndOfMessageStrategy = endOfMessageCriteria;
            
            if (dataStream == null)
                throw new ArgumentNullException("dataStream");

            m_BytesRead = 0;
            m_Source = new StringBuilder();
            IList<RFC822.Field> fields;
            // 读消息头
            int cause = ParseFields(ref dataStream, out fields);            

            if (fields.Count == 0)
                return new NullMessage();

            Message m = new Message();
            m.Fields = fields;

            if (cause < 1)
                return m;
            
            // 判断是文本邮件还是多媒体邮件
            if (isMIME(ref fields))
            {                
                string delimiter = ParseMessage(ref dataStream, ref m, fields);                
            }
            else
            {
                IMailMessage im = m as IMailMessage;
                base.ReadBody(ref dataStream, ref im);             
            }

            m.Source = m_Source.ToString();
            m_Source = null;
            return m as IMimeMailMessage;
        }

        private string ParseMessage(ref Stream dataStream, ref Message message, IList<RFC822.Field> fields)
        {            
            foreach (RFC822.Field contentField in fields)
            {
                if (contentField is ContentTypeField)
                {
                    ContentTypeField contentTypeField = contentField as ContentTypeField;

                    if (m_FieldParser.CompositeType.IsMatch(contentTypeField.Type))
                    {//是复合邮件
                        IMultipartEntity e = message as IMultipartEntity;
                        e.Delimiter = ReadDelimiter(ref contentTypeField);                        
                        return ReadCompositeEntity(ref dataStream, ref e);                        
                    }
                    else if (m_FieldParser.DescriteType.IsMatch(contentTypeField.Type))
                    {
                        IEntity e = message as IEntity;
                        message.BodyParts.Add(e);// This is a message witch body lies within its own entity                        
                        return ReadDiscreteEntity(ref dataStream, ref e);                        
                    }
                }
            }
            return string.Empty;
        }

        private string ReadCompositeEntity(ref Stream dataStream, ref IMultipartEntity parent)
        {            
            IEntity child;
            string delimiter = null;
            
            IEntity e = parent as IEntity;
            if (parent.BodyParts.Count < 1)
            {
                delimiter = FindStartDelimiter(ref dataStream, ref e);
                if (!delimiter.Equals("--" + parent.Delimiter))
                    return string.Empty;
            }

            delimiter = CreateEntity(ref dataStream, ref parent, out child);          
           
            if (child == null || delimiter == string.Empty)
            {
                return string.Empty;
            }

            if (child is MultipartEntity)
            {
                IMultipartEntity mChild = child as IMultipartEntity;
                delimiter = ReadCompositeEntity(ref dataStream, ref mChild);
            }
            else
                delimiter = ReadDiscreteEntity(ref dataStream, ref child);                

            // until we reach end of mail
            while (delimiter != string.Empty && parent != null)
            {
                if (parent.Delimiter == null)
                {
                    parent = parent.Parent;
                }
                else if (delimiter.Contains(parent.Delimiter))
                {
                    if (delimiter.Equals("--" + parent.Delimiter))
                    {
                        delimiter = ReadCompositeEntity(ref dataStream, ref parent);
                    }
                    else if (delimiter.Equals("--" + parent.Delimiter + "--"))
                    {
                        parent = parent.Parent;
                        IEntity ent = parent as IEntity;
                        delimiter = FindStartDelimiter(ref dataStream, ref ent);                        
                    }
                }                         
            }
            return delimiter;
        }

        private string ReadDiscreteEntity(ref Stream dataStream, ref IEntity entity)
        {
            string body;
            string delimiter;            

            body = string.Empty;
            delimiter = ReadDiscreteBody(ref dataStream,ref entity, ref body);

            string encoding = string.Empty;
            
            foreach (RFC822.Field field in entity.Fields)
            {                
                if (field is ContentTransferEncodingField)
                {
                    ContentTransferEncodingField transferField = field as ContentTransferEncodingField;
                    encoding = transferField.Encoding;                    
                    break;
                }
            }

            foreach (RFC2045.IDecoder decoder in m_Decoders)
            {
                if (decoder.CanDecode(encoding))
                {
                    entity.Body = decoder.Decode(ref body);
                    break;
                }
            }

            return delimiter;
        }



        private string CreateEntity(ref Stream dataStream, ref IMultipartEntity parent, out IEntity entity)
        {
            entity = null;
            IList<RFC822.Field> fields;
            int cause = ParseFields(ref dataStream, out fields);
            if (cause > 0)
            {
                foreach (RFC822.Field contentField in fields)
                {
                    if (contentField is ContentTypeField)
                    {
                        ContentTypeField contentTypeField = contentField as ContentTypeField;

                        if (m_FieldParser.CompositeType.IsMatch(contentTypeField.Type))
                        {
                            MultipartEntity mEntity = new MultipartEntity();                            
                            mEntity.Fields = fields;
                            entity = mEntity;
                            entity.Parent = parent;
                            parent.BodyParts.Add(entity);

                            if (Regex.IsMatch(contentTypeField.Type, "(?i)message") &&
                                Regex.IsMatch(contentTypeField.SubType, "(?i)rfc822"))
                            {
                                Message message = new Message();
                                IList<RFC822.Field> messageFields;
                                cause = ParseFields(ref dataStream, out messageFields);
                                message.Fields = messageFields;                                
                                mEntity.BodyParts.Add(message);
                                message.Parent = mEntity;
                                if(cause > 0)
                                    return ParseMessage(ref dataStream, ref message, messageFields);
                                break;
                            }
                            else
                            {
                                mEntity.Delimiter = ReadDelimiter(ref contentTypeField);
                                return parent.Delimiter;                                
                            }                           
                        }
                        else if (m_FieldParser.DescriteType.IsMatch(contentTypeField.Type))
                        {
                            entity = new Entity();
                            entity.Fields = fields;
                            entity.Parent = parent;
                            parent.BodyParts.Add(entity);
                            return parent.Delimiter;                                   
                        }
                    }
                }
            }
            return string.Empty;
        }       

        private string ReadDiscreteBody(ref Stream dataStream, ref IEntity entity, ref string body)
        {
            StringBuilder sBuilder;
            char[] cLine;
            string currentLine, boundary;
            bool bContinue = true;            
            
            int fulFilledCriteria;            
            m_Criterias.Clear();
            m_Criterias.Add(m_EndOfMessageStrategy);
            m_Criterias.Add(m_EndOfLineStrategy);

            sBuilder = new StringBuilder();
            do
            {                   
                cLine = ReadData(ref dataStream, m_Criterias, out fulFilledCriteria);                
                currentLine = new string(cLine);
                boundary = FindDelimiter(ref entity, ref currentLine);
                if (!string.IsNullOrEmpty(boundary))
                {
                    break;
                }

                // Have we found the end of this message?                
                if (fulFilledCriteria == 0)
                {
                    boundary = string.Empty;
                    break;
                }
                sBuilder.Append(cLine);
            }
            while (bContinue);
                        
            body = sBuilder.ToString();
            sBuilder.Append(cLine);
            m_Source.Append(sBuilder.ToString());
            return boundary;
        }

        private new int ParseFields(ref Stream dataStream, out IList<RFC822.Field> fields)
        {
            string headers;
            int cause = ReadHeaders(ref dataStream, out headers);
            m_Source.Append(headers);
            headers = m_FieldParser.Unfold(headers);
            fields = new List<RFC822.Field>();
            m_FieldParser.Parse(ref fields, ref headers);            
            return cause;
        }

        private string ReadDelimiter(ref ContentTypeField contentTypeField)
        {
            if (contentTypeField.Parameters["boundary"] != null)
            {
                return contentTypeField.Parameters["boundary"];
            }
            else
            {
                return string.Empty;
            }            
        }

        private string FindStartDelimiter(ref Stream dataStream, ref IEntity entity)
        {            
            int fulFilledCriteria;
            string line, delimiter;                        
            m_Criterias.Clear();
            m_Criterias.Add(m_EndOfMessageStrategy);
            m_Criterias.Add(m_EndOfLineStrategy);

            delimiter = string.Empty;

            do
            {                
                line = new string(ReadData(ref dataStream, m_Criterias, out fulFilledCriteria));
                m_Source.Append(line);
                if (fulFilledCriteria == 0)
                {
                    delimiter = string.Empty;
                    break;
                }

                delimiter = FindDelimiter(ref entity, ref line);
                if (!string.IsNullOrEmpty(delimiter))
                {                    
                    break;
                }                
            } while (fulFilledCriteria > 0);
            return delimiter;
        }

        private string FindDelimiter(ref IEntity entity, ref string line)
        {           
            string boundary = string.Empty;            
            if (m_FieldParser.EndBoundary.IsMatch(line) ||
                m_FieldParser.StartBoundary.IsMatch(line))
            {
                Match match;
                char[] cTrims = new char[] { '\\', '"' };
                if (IsDelimiter(ref entity, ref line))
                {
                    if (m_FieldParser.EndBoundary.IsMatch(line))
                    {
                        match = m_FieldParser.EndBoundary.Match(line);
                        boundary = match.Value.Trim();
                        boundary = boundary.Trim(cTrims);                        
                    }
                    else if (m_FieldParser.StartBoundary.IsMatch(line))
                    {
                        match = m_FieldParser.StartBoundary.Match(line);
                        boundary = match.Value.Trim();
                        boundary = boundary.Trim(cTrims);                        
                    }
                }
            }
            return boundary;
        }

        private bool IsDelimiter(ref IEntity entity, ref string line)
        {
            IEntity test = entity;
            while (test != null)
            {
               if(test.Delimiter != null && line.Contains(test.Delimiter))
                   return true;
                test = test.Parent;
            }
            return false;
        }

        private bool isMIME(ref IList<RFC822.Field> fields)
        {
            foreach (RFC822.Field field in fields)
            {                
                if (m_FieldParser.MIMEVersion.IsMatch(field.Name + ":" + field.Body))
                    return true;
            }
            return false;
        }
    }
}
