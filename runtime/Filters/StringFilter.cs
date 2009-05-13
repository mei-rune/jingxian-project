

using System;
using System.Text.RegularExpressions;
using System.Xml;

namespace jingxian.core.runtime.Filters
{
    using jingxian.core.runtime.Xml.Serialization;
    using jingxian.core.runtime.utilities;

    [Serializable]
    public abstract class StringFilter : XmlSerializable, IFilter<string>
    {
        private Regex _Expression;
        private bool _IsRegex;
        private string _name;


        protected StringFilter()
        {
        }

        protected StringFilter(string name, bool isRegex)
        {
            _name = Enforce.ArgumentNotNullOrEmpty(name, "name");
            _IsRegex = isRegex;
        }

        public string Name
        {
            get
            {
                if (_name == null)
                    _name = string.Empty;

                return _name;
            }
            private set { _name = value; }
        }

        public bool IsRegex
        {
            get { return _IsRegex; }
            private set { _IsRegex = value; }
        }

        protected virtual Regex Expression
        {
            get
            {
                if (_Expression == null && IsRegex)
                    _Expression = new Regex(Name);

                return _Expression;
            }
        }

        #region IFilter<string> Members

        public virtual bool CanFilter(Type type)
        {
            return typeof(string).IsAssignableFrom(type);
        }


        public virtual bool MeetsCriteria(string obj)
        {
            if (IsRegex)
            {
                return Expression.IsMatch(obj);
            }
            else
            {
                return obj == Name;
            }
        }

        #endregion

        #region XML Serialization related

        protected override void ReadXmlAttributes(XmlReader reader)
        {
            base.ReadXmlAttributes(reader);
            Name = XmlUtils.ReadRequiredAttributeString(reader, "name");
            IsRegex = XmlUtils.ReadAttributeAsBoolean(reader, "regex");
        }

        protected override void WriteXmlAttributes(XmlWriter writer)
        {
            base.WriteXmlAttributes(writer);
            XmlUtils.WriteAttribute(writer, "name", Name);
            XmlUtils.WriteAttribute(writer, "regex", IsRegex);
        }

        #endregion

    }
}