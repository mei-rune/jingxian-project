
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Text.RegularExpressions;

namespace jingxian.mail.mime.RFC822
{
    public class MailReader:IMailReader
    {
        protected FieldParser m_FieldParser;
        protected IList<IEndCriteriaStrategy> m_Criterias;
        protected IEndCriteriaStrategy m_EndOfMessageStrategy;
        protected IEndCriteriaStrategy m_EndOfLineStrategy;
        protected IEndCriteriaStrategy m_NullLineStrategy;
        protected StringBuilder m_Source;

        /// <summary>
        /// 总共所读取的字节数
        /// </summary>
        protected long m_BytesRead;
        /// <summary>
        /// 每从数据流中读取n个字符就引发事件
        /// </summary>
        private long m_UpdateInterval = 1;

        
        public event DataReadEventHandler DataRead;

        public MailReader()
        {
            m_FieldParser = new FieldParser();
            m_FieldParser.CompilePattern();
            m_Criterias = new List<IEndCriteriaStrategy>();
            m_EndOfLineStrategy = new EndOfLineStrategy();
            m_NullLineStrategy = new NullLineStrategy();
        }

        public IMailMessage Read(ref Stream dataStream, IEndCriteriaStrategy endOfMessageCriteria)
        {
            m_BytesRead = 0;
            m_EndOfMessageStrategy = endOfMessageCriteria;
            Message m = new Message();
            IMailMessage im = m as IMailMessage;            
            m_Source = new StringBuilder();
            IList<RFC822.Field> fields;
            int cause = ParseFields(ref dataStream, out fields);
            m.Fields = fields;             
            if (cause > 0)
            {
                ReadBody(ref dataStream, ref im);
            }
            m.Source = m_Source.ToString();
            return m;            
        }

        /// <summary>
        /// 每从数据流中读取n个字符就引发事件
        /// </summary>
        public long DataReadUpdateInterval
        {
            get
            {
                return m_UpdateInterval;
            }
            set
            {
                m_UpdateInterval = value;
            }
        }

        protected int ParseFields(ref Stream dataStream, out IList<RFC822.Field> fields)
        {
            string headers;
            int cause = ReadHeaders(ref dataStream, out headers);
            m_Source.Append(headers);
            headers = m_FieldParser.Unfold(headers);
            fields = new List<RFC822.Field>();
            m_FieldParser.Parse(ref fields, ref headers);
            return cause;
        }

        /// <summary>
        /// 读消息的头
        /// </summary>
        /// <param name="dataStream">数据流</param>
        /// <param name="sHeaders">消息头的数据</param>
        /// <returns>说明是哪一个结尾描述接口匹配功能的</returns>
        protected int ReadHeaders(ref Stream dataStream, out string sHeaders)
        {
            sHeaders = string.Empty;            
            char[] headers;
            int fulfilledCriteria;

            m_Criterias.Clear();
            m_Criterias.Add(m_EndOfMessageStrategy);
            m_Criterias.Add(m_NullLineStrategy);
            
            headers = ReadData(ref dataStream, m_Criterias, out fulfilledCriteria);                                    
            sHeaders = new string(headers);
            return fulfilledCriteria;
        }      

        /// <summary>
        /// 读消息体
        /// </summary>
        /// <param name="dataStream">数据流</param>
        /// <param name="message"></param>
        protected void ReadBody(ref Stream dataStream, ref IMailMessage message)
        {            
            char[] buffer;            
            int fulFilledCriteria;

            m_Criterias.Clear();
            m_Criterias.Add(m_EndOfMessageStrategy);
           
            buffer = ReadData(ref dataStream, m_Criterias, out fulFilledCriteria);
            string body = new string(buffer);
            m_Source.Append(body);
            message.TextMessage = body;
        }                

        /// <summary>
        /// 从数据流中读数据直到发现指定的结尾
        /// </summary>
        /// <param name="dataStream">数据流</param>
        /// <param name="criterias">结尾的描述</param>
        /// <param name="fulfilledCritera">说明是哪一个结尾描述接口匹配功能的</param>
        /// <returns>所读取的数据</returns>
        protected char[] ReadData(ref Stream dataStream, IList<IEndCriteriaStrategy> criterias, out int fulfilledCritera)
        {
            fulfilledCritera = -1;
            int size, pos, c;
            char[] buffer, data;

            size = 1;
            pos = 0;
            buffer = new char[size];

            while ((c = dataStream.ReadByte()) != -1)
            {
                m_BytesRead++;
                if ((m_BytesRead % m_UpdateInterval) == 0 && pos > 0)
                {
                    DataReadEventArgs args = new DataReadEventArgs();
                    args.AmountRead = m_BytesRead;
                    OnDataRead(this, args);
                }

                if (pos >= (size - 1))
                {// 空间不够,重分配内存
                    size = size * 2;
                    char[] tmpBuffer = new char[size];
                    buffer.CopyTo(tmpBuffer, 0);
                    buffer = null;
                    buffer = tmpBuffer;
                }
                buffer[pos] = (char)c;

// 此处两种功能是一致，第一个容易读
#if READ
                if (pos > 0 && IsEndCriteria( buffer, pos,criterias, out fulfilledCritera  ))
                    break;// 检测是不是结束了
#else
                if (pos > 0)
                {// 检测是不是结束了
                    int i = 0;
                    foreach (IEndCriteriaStrategy criteria in criterias)
                    {
                        if (criteria.IsEndReached(buffer, pos))
                        {
                            fulfilledCritera = i;
                            break;
                        }
                        i++;
                    }
                }

                if (fulfilledCritera > -1)
                {
                    break;
                }
#endif
                pos++;
            }

            data = new char[pos + 1];
            Array.Copy(buffer, data, pos + 1);
            buffer = null;
            return data;
        }

        protected bool IsEndCriteria(char[] buffer, int pos, IEnumerable<IEndCriteriaStrategy> criterias, out int fulfilledCritera)
        {
            fulfilledCritera = -1;
            // 检测是不是结束了
            int i = 0;
            foreach (IEndCriteriaStrategy criteria in criterias)
            {
                if (criteria.IsEndReached(buffer, pos))
                {
                    fulfilledCritera = i;
                    break;
                }
                i++;
            }
            return (fulfilledCritera > -1);
        }

        protected void OnDataRead(object sender, DataReadEventArgs args)
        {
            if (DataRead != null)
            {
                DataRead(sender, args);
            }
        }
        
    }
}
