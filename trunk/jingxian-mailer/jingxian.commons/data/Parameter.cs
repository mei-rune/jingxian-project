using System;

namespace jingxian.data
{
    public class Parameter
    {
        private string _paramname;
        private string _paramvalue;

        public Parameter()
        { }

        public Parameter( string nm, string value)
        {
            _paramname = nm;
            _paramvalue = value;
        }
        
        public string Name
        {
            get { return _paramname; }
            set { _paramname = value; }
        }

        public string Value
        {
            get { return _paramvalue; }
            set { _paramvalue = value; }
        }
    }
}