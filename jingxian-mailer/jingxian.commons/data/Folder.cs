namespace jingxian.data
{
    public class Folder
    {
        private int _id;
        private string _name;
        private int _idparent;
        private string _className;
        private string _icon;
        private int _ordering;

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

        public int Parent
        {
            get { return _idparent; }
            set { _idparent = value; }
        }

        public string Type
        {
            get { return _className; }
            set { _className = value; }
        }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public int Ordering
        {
            get { return _ordering; }
            set { _ordering = value; }
        }

        public override string ToString()
        {
            return _name;
        }

        public override int GetHashCode()
        {
            return _id;
        }
    }
}