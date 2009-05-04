
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;
using System.Runtime.Remoting.Messaging;

namespace jingxian.mail.popper
{
    using jingxian.mail.mime;

    public class POPClient: IEndCriteriaStrategy
    {
        private string m_Server;
        private ushort m_Port;
        private string m_User;
        private string m_Password;
        private bool m_UseSSL;
        private double m_KeepAliveInterval;
        private int m_MaxHistoryLenght = 20;        

        private POPState m_CurrentState;        
        private Queue<RFC1939.POPCommand> m_CommandHistory;
        
        private IMailReader m_MailReader;
        private TcpClient m_TcpClient;
        private Stream m_Stream;        
        private System.Timers.Timer m_PollTimer;

        internal delegate IMailMessage ReadMailDelegate(ref Stream stream, IEndCriteriaStrategy criteria);

        public event DataReadEventHandler DataRead;

        public event EventHandler StateChanged;

        public event CommandIssuedEventHandler CommandIssued;

        public POPClient():this(new mime.RFC2045.MailReader())
        {
        }

        public POPClient(IMailReader mailreader)
        {
            m_CurrentState = DisconnectedState.GetInstane();
            m_TcpClient = new TcpClient();            
            m_CommandHistory = new Queue<jingxian.mail.popper.RFC1939.POPCommand>();
            m_PollTimer = new System.Timers.Timer();
            m_MailReader = mailreader;

            mailreader.DataRead += new DataReadEventHandler(OnDataRead);
            m_PollTimer.Elapsed += new System.Timers.ElapsedEventHandler(m_PollTimer_Elapsed);
        }        

        #region Fields

        public string Server
        {
            get { return this.m_Server; }
            set { this.m_Server = value; }
        }

        public ushort Port
        {
            get { return this.m_Port; }
            set { this.m_Port = value; }
        }

        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        public bool UseSSL
        {
            get { return m_UseSSL; }
            set { m_UseSSL = value; }
        }

        public double KeepAliveInterval
        {
            get { return m_KeepAliveInterval; }
            set { m_KeepAliveInterval = value; }
        }

        internal void LoggCommand(RFC1939.POPCommand command)
        {
            if (m_CommandHistory.Count > m_MaxHistoryLenght)
            {                
                m_CommandHistory.Dequeue();
            }
            m_CommandHistory.Enqueue(command);
        }

        public RFC1939.POPCommand[] CommandHistory
        {
            get 
            {
                 return m_CommandHistory.ToArray();                
            }
        }

        public POPState CurrentState
        {
            get
            {
                return m_CurrentState;
            }
            internal set
            {
                m_CurrentState = value;
                OnStateChanged(this, new StateChangedEventArgs(m_CurrentState));
            }
        }

        public int MaxHistoryLenght
        {
            get { return m_MaxHistoryLenght; }
            set { m_MaxHistoryLenght = value; }
        }

        public long UpdateInterval
        {
            get { return m_MailReader.DataReadUpdateInterval; }
            set { m_MailReader.DataReadUpdateInterval = value; }
        }        
        
        #endregion        

        public bool IssueCommand(RFC1939.POPCommand command)
        {
            try
            {
                if (m_CurrentState.IssueCommand(command, this))
                {
                    OnCommandIssued(this, 
                        new CommandIssuedEventArgs(command));
                    return true;
                }                
            }
            catch (RFC1939.POPException ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        if (ex.InnerException.InnerException is System.Net.Sockets.SocketException)
                        {
                            CloseConnection();
                            CurrentState = DisconnectedState.GetInstane();
                            return false;
                        }
                    }
                }
                throw ex;
            }            
            return false;
       
        }

        public bool Connect()
        {
            if (m_TcpClient == null || !m_TcpClient.Connected)
            {
                m_Stream = MakeConnection();
            }

            if (m_KeepAliveInterval >= 1000)
            {
                m_PollTimer.Interval = m_KeepAliveInterval;
                m_PollTimer.Enabled = true;
            }

            return IssueCommand(new RFC1939.GreetingCommand(m_Stream));
        }

        public bool Authenticate()
        {
            RFC1939.ConnectCommand command = new RFC1939.ConnectCommand(m_Stream, m_User, m_Password);
            return IssueCommand(command);            
        }

        public int CountMessage()
        {
            if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return -1;
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return -2;
            }
            
            RFC1939.StatCommand command = new RFC1939.StatCommand(m_Stream);
            if (IssueCommand(command))
            {
                return command.Count;
            }
            return -3;            
        }

        public long[] ListMessages()
        {
            if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return new long[] { -1 };
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return new long[] { -2 };
            }

            RFC1939.ListCommand command = new RFC1939.ListCommand(m_Stream);
            if (IssueCommand(command))
            {
                return command.List;
            }
            return new long[] { -3 };
        }

        public long GetMessageSize(int message )
        {
             if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return -1;
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return -2;
            }

            RFC1939.ListCommand command = new RFC1939.ListCommand(m_Stream, message);
            if (IssueCommand(command))
            {
                return command.List[0];
            }
            return -3;
        }

        public IMailMessage FetchMail(int messageNumber)
        {
            if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return new mime.RFC2045.NullMessage();
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return new mime.RFC2045.NullMessage();
            }
            
            RFC1939.RetriveCommand command = new RFC1939.RetriveCommand(m_Stream, messageNumber);
            if (IssueCommand(command))
            {
                IMailMessage message;
                lock (m_Stream)
                {
                    message = m_MailReader.Read(ref m_Stream, this);
                }
                this.CurrentState = TransactionState.GetInstance();
                return message;
            }
            return new mime.RFC2045.NullMessage();            
        }

        public virtual IAsyncResult BeginFetchMail(int messageNumber, AsyncCallback callBack, object state)
        {
            if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return null;
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return null;
            }
            
            RFC1939.RetriveCommand command = new RFC1939.RetriveCommand(m_Stream, messageNumber);            
            if (IssueCommand(command))
            {                
                ReadMailDelegate readelegate = new ReadMailDelegate(m_MailReader.Read);
                return readelegate.BeginInvoke(ref m_Stream, this, callBack, state);                
            }            
            return null;
        }

        public IMailMessage EndFetchMail(IAsyncResult result)
        {
            ReadMailDelegate readDelegate = ((AsyncResult)result).AsyncDelegate as ReadMailDelegate;
            this.CurrentState = TransactionState.GetInstance();
            if (readDelegate != null)
            {
                return readDelegate.EndInvoke(ref m_Stream, result);
            }
            return new mime.RFC2045.NullMessage();
        }
        
        public IMailMessage FetchMailHeader(int messageNumber)
        {
            if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return new mime.RFC2045.NullMessage();
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return new mime.RFC2045.NullMessage();
            }
            
            RFC1939.TopCommand command = new RFC1939.TopCommand(m_Stream, messageNumber, 0); 
            if (IssueCommand(command))
            {
                IMailMessage message;
                lock (m_Stream)
                {
                    message = m_MailReader.Read(ref m_Stream, this);
                }
                this.CurrentState = TransactionState.GetInstance();
                return message;
            }
            return new mime.RFC2045.NullMessage();                        
        }

        public bool DeleteMessage(int messageNumber)
        {
            if (m_CurrentState is DisconnectedState)
            {
                if (!Connect())
                    return false;
            }

            if (m_CurrentState is AuthorizationState)
            {
                if (!Authenticate())
                    return false;
            }

            RFC1939.DeleteCommand command = new RFC1939.DeleteCommand(m_Stream,
                messageNumber);
            return IssueCommand(command);            
        }

        public bool UndoDelete()
        {
            if (m_CurrentState is TransactionState)
            {
                RFC1939.ResetCommand command = new RFC1939.ResetCommand(m_Stream);
                return IssueCommand(command);                
            }
            else
            {
                return false;
            }

        }

        public bool Disconnect()
        {
            RFC1939.QuitCommand command = new RFC1939.QuitCommand(m_Stream);
            bool success = IssueCommand(command);            
            CloseConnection();
            return success;
        }

        private Stream MakeConnection()
        {
            if (m_Server != null && m_Port > 0)
            {
                m_TcpClient = new TcpClient(m_Server, m_Port);
                Stream s = m_TcpClient.GetStream();
                if (m_UseSSL)
                {
                    SslStream sStream = new SslStream(s, false);
                    sStream.AuthenticateAsClient(m_Server);
                    s = sStream;
                }
                return s; 
            }
            return null;
        }

        private void CloseConnection()
        {
            m_PollTimer.Enabled = false;

            if (m_TcpClient != null)
            {
                if (m_TcpClient.Client != null)
                    m_TcpClient.Client.Close();
                m_TcpClient.Close();
            }
        }

        #region 事件处理

        protected virtual void OnStateChanged(object sender, EventArgs e)
        {
            if (StateChanged != null)
                StateChanged(sender, e);
        }

        protected virtual void OnDataRead(object sender, DataReadEventArgs e)
        {
            if (DataRead != null)
            {
                DataRead(sender, e);
            }
        }

        protected virtual void OnCommandIssued(object sender, CommandIssuedEventArgs e)
        {
            if (CommandIssued != null)
                CommandIssued(sender, e);
        }

        void m_PollTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RFC1939.NoopCommand command = new RFC1939.NoopCommand(m_Stream);
            IssueCommand(command);
        }

        #endregion

        #region IEndCriteriaStrategy 成员

        public bool IsEndReached(char[] data, int size)
        {
            if (size >= 4)
            {
                int fifth = data[size - 4];
                int fourth = data[size - 3];
                int third = data[size - 2];
                int second = data[size - 1];
                int first = data[size];

                if (fifth == 13 && fourth == 10 && third == 46 &&
                    second == 13 && first == 10)
                {
                    return true;
                }
            }

            if (size >= 2)
            {
                int third = data[0];
                int second = data[1];
                int first = data[2];

                if (third == 46 && second == 13 && first == 10)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
