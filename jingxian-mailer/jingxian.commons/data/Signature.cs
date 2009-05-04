using System;

namespace jingxian.data
{
    public class Signature
    {

        private int _id;
        private string _name;
        private string _owner;
        private string _type;
        private string _signature;
        private string _signaturehtml;
        private string _param;


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Content
        {
            get { return _signature; }
            set { _signature = value; }
        }

        public string Html
        {
            get { return _signaturehtml; }
            set { _signaturehtml = value; }
        }

        public string Param
        {
            get { return _param; }
            set { _param = value; }
        }
    }
}