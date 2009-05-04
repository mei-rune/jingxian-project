using System;

namespace jingxian.data
{
    public class Note
    {
        private int _id;
        private string _noteguid;
        private string _title;
        private string _content;
        private string _param;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string UId
        {
            get { return _noteguid; }
            set { _noteguid = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public string Param
        {
            get { return _param; }
            set { _param = value; }
        }
    }
}