
using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net.Sockets;

namespace jingxian.mail.popper.RFC1939
{
    public abstract class POPCommand
    {
        protected readonly string NewLine = Environment.NewLine;

        protected readonly string OK = "+OK";
        
        protected readonly string ERR = "-ERR";

        protected readonly string m_Command;

        /// <summary>
        /// 数据流
        /// </summary>
        protected Stream m_Receiver;  

        protected string m_Arguments;

        protected string m_Request;

        protected string m_Response;

        public string Response
        {
            get { return m_Response; }           
        }

        public string Command
        {
            get { return m_Command; }
        }

        public POPCommand(Stream receiver, string command, string arguments)
        {
            if ( string.IsNullOrEmpty(command))
                throw new ArgumentNullException("command");
            if (receiver == null)
                throw new ArgumentNullException("receiver");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            m_Command = command;
            m_Receiver = receiver;
            m_Arguments = arguments;
            m_Response = string.Empty;         
        }

        protected void WriteRequest()
        {
            lock (m_Receiver)
            {
                byte[] data = System.Text.Encoding.ASCII.GetBytes(m_Request.ToCharArray());
                m_Receiver.Write(data, 0, data.Length);
            }
        }

        protected void ReadResponse()
        {
            lock (m_Receiver)
            {
                string res = ReadLine(ref m_Receiver);
                m_Response += res;
            }
        }

        /// <summary>
        /// 读一行以"\r\n"结尾的数据
        /// </summary>
        private string ReadLine(ref Stream dataStream)
        {            
            int size = 1024;
            int pos = 0;
            int c;
            char[] buffer = new char[size];
            char previous, current;

            
            while ((c = dataStream.ReadByte()) != -1)
            {
                if (pos >= (size - 1))
                {
                    size = size * 2;
                    char[] tmpBuffer = new char[size];
                    buffer.CopyTo(tmpBuffer, 0);
                    buffer = null;
                    buffer = tmpBuffer;
                }
                buffer[pos] = (char)c;

                if (pos > 0)
                {
                    previous = buffer[pos - 1];
                    current = buffer[pos];
                    if (previous == 13 && current == 10)
                        break;
                }               
                pos++;
            }

            char[] data = new char[pos + 1];
            Array.Copy(buffer, data, pos + 1);
            buffer = null;
            return new string(data);
        }

        
        public virtual bool Execute()
        {
            try
            {
                m_Request = string.Concat(m_Command, " ", NewLine);
                WriteRequest();
                ReadResponse();
            }
            catch (Exception ex)
            {
                ThrowException( ex);
            }

            if (string.IsNullOrEmpty(m_Response))
                return false;
            return m_Response.StartsWith(OK);
        }


        protected void ThrowException( Exception ex)
        {
            throw new POPException(string.Concat("执行 ", m_Command, " 命令发生错误!"), ex);
        }

        protected void ThrowException(string message, Exception ex)
        {
            throw new POPException(message, ex);
        }

        public override string ToString()
        {
            return m_Request;
        }
    }
}
