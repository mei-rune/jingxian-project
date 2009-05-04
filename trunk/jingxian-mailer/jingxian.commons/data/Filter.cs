namespace jingxian.data
{

    public class Filter
    {
        private int _id;
        private string _name;
        private string _type;
        private int _itemgroup;
        private int _idparent;
        private int _ordering;
        private string _filterdata;
        
        
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

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        
        public int ItemGroup
        {
            get { return _itemgroup; }
            set { _itemgroup = value; }
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
        
        public string Data
        {
            get { return _filterdata; }
            set { _filterdata = value; }
        }
    }
}