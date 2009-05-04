using System;

namespace jingxian.data
{
    using jingxian.collections;

    public class MessageWithBLOBs : Message
    {
        private byte[] _rawHeader;
        private byte[] _rawBody;
        private Properties _param;

        public byte[] RawHeader
        {
            get { return _rawHeader; }
            set { _rawHeader = value; }
        }

        public byte[] RawBody
        {
            get { return _rawBody; }
            set { _rawBody = value; }
        }

        public Properties Misc
        {
            get { return _param; }
            set { _param = value; }
        }
    }
}