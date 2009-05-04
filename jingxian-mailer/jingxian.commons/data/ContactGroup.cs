
namespace jingxian.data
{
    public class ContactGroup
    {
        private int _id;
        private int _idparent;
        private int _ordering;
        private string _name;
        private string _remark;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Parent
        {
            get { return _idparent; }
            set { _idparent = value; }
        }

        public int Ordering
        {
            get { return _ordering; }
            set { _ordering = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}