
namespace jingxian.data
{
    public class DtPropertiesKey
    {
        private int _id;
        private string _property;


        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }
    }

    public class DtProperties : DtPropertiesKey
    {
        private int _objectid;
        private string _value;
        private string _uvalue;
        private int _version;
        private byte[] _lvalue;

        public int ObjectId
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public string UValue
        {
            get { return _uvalue; }
            set { _uvalue = value; }
        }
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }
        public byte[] LValue
        {
            get { return _lvalue; }
            set { _lvalue = value; }
        }
    }
}