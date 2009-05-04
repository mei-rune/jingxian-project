
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.mail.popper
{
    public class StateChangedEventArgs:EventArgs
    {
        private POPState m_NewState;

        public POPState NewState
        {
            get { return m_NewState; }
            set { m_NewState = value; }
        }

        public StateChangedEventArgs(POPState newState)
        {
            m_NewState = newState;
        }
    }
}
