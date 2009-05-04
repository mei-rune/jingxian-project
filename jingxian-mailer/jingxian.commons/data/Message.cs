using System;

namespace jingxian.data
{
    public class Message
    {
        private int _id;
        private int _idnewsserver;
        private int _itemgroup;
        private string _headerFrom;
        private string _headerNewsgroup;
        private string _headerMessageid;
        private int _category;
        private string _idparent;
        private int _infoRead;
        private string _infoReply;
        private string _headerMime;
        private string _headerCc;
        private string _headerBcc;
        private string _headerSubject;
        private string _headerReferences;
        private string _headerTo;
        private int _headerSize;
        private int _attachmentSize;
        private int _messageindex;
        private int _folder;
        private int _textcolor;
        private int _backgroundcolor;
        private int _messagetype;
        private DateTime _headerdate;
        private DateTime _receiveddate;
        private int _isspam;
        private int _importance;
        private string _uid;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Parent
        {
            get { return _idparent; }
            set { _idparent = value; }
        }

        public int NewsServer
        {
            get { return _idnewsserver; }
            set { _idnewsserver = value; }
        }

        public int ItemGroup
        {
            get { return _itemgroup; }
            set { _itemgroup = value; }
        }
        public int Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public int Importance
        {
            get { return _importance; }
            set { _importance = value; }
        }
        public string UId
        {
            get { return _uid; }
            set { _uid = value; }
        }
        public int MessageType
        {
            get { return _messagetype; }
            set { _messagetype = value; }
        }
        public int InfoRead
        {
            get { return _infoRead; }
            set { _infoRead = value; }
        }
        public string InfoReply
        {
            get { return _infoReply; }
            set { _infoReply = value; }
        }
        public string HeaderFrom
        {
            get { return _headerFrom; }
            set { _headerFrom = value; }
        }
        public string HeaderNewsGroup
        {
            get { return _headerNewsgroup; }
            set { _headerNewsgroup = value; }
        }
        public string HeaderMessageId
        {
            get { return _headerMessageid; }
            set { _headerMessageid = value; }
        }
        public string HeaderMime
        {
            get { return _headerMime; }
            set { _headerMime = value; }
        }

        public string HeaderCC
        {
            get { return _headerCc; }
            set { _headerCc = value; }
        }

        public string HeaderBCC
        {
            get { return _headerBcc; }
            set { _headerBcc = value; }
        }

        public string HeaderSubject
        {
            get { return _headerSubject; }
            set { _headerSubject = value; }
        }

        public string HeaderReferences
        {
            get { return _headerReferences; }
            set { _headerReferences = value; }
        }

        public string HeaderTO
        {
            get { return _headerTo; }
            set { _headerTo = value; }
        }

        public int HeaderSize
        {
            get { return _headerSize; }
            set { _headerSize = value; }
        }
        public int AttachmentSize
        {
            get { return _attachmentSize; }
            set { _attachmentSize = value; }
        }
        public int MessageIndex
        {
            get { return _messageindex; }
            set { _messageindex = value; }
        }
        public int Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }
        public int TextColor
        {
            get { return _textcolor; }
            set { _textcolor = value; }
        }
        public int BackgroundColor
        {
            get { return _backgroundcolor; }
            set { _backgroundcolor = value; }
        }
        public int IsSpam
        {
            get { return _isspam; }
            set { _isspam = value; }
        }
        public DateTime HeaderDate
        {
            get { return _headerdate; }
            set { _headerdate = value; }
        }
        public DateTime ReceivedDate
        {
            get { return _receiveddate; }
            set { _receiveddate = value; }
        }
    }
}