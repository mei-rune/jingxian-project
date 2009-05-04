
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.mime
{
    public delegate void DataReadEventHandler(object sender, DataReadEventArgs args);

    public class DataReadEventArgs : EventArgs
    {
        private long m_AmountRead;

        public long AmountRead
        {
            get { return m_AmountRead; }
            set { m_AmountRead = value; }
        }

        public DataReadEventArgs() : base() { }

    }
    
}
