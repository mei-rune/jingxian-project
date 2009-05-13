

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


namespace jingxian.core.runtime.Filters
{
    using jingxian.core.runtime.Xml.Serialization;
    using jingxian.core.runtime.utilities;

    [Serializable]
    public abstract class IncludeExcludeSet : XmlSerializable, IFilter<string>
    {
        private List<Include> _Includes;
        private List<Exclude> _Excludes;
        private bool _positiveIncludeByDefault = true;

        #region IncludesAndExcludes

        public ICollection<Include> Includes
        {
            get
            {
                if (_Includes == null)
                    _Includes = new List<Include>();
                return _Includes;
            }
        }

        public ICollection<Exclude> Excludes
        {
            get
            {
                if (_Excludes == null)
                    _Excludes = new List<Exclude>();
                return _Excludes;
            }
        }

        public bool PositiveIncludeByDefault
        {
            get { return _positiveIncludeByDefault; }
            set { _positiveIncludeByDefault = value; }
        }

        #endregion

        #region IFilter<string> Members

        public bool CanFilter(Type type)
        {
            return typeof(string).IsAssignableFrom(type);
        }

        public bool MeetsCriteria(string obj)
        {
            bool included = PositiveIncludeByDefault;
            foreach (Include include in Includes)
            {
                if (include.MeetsCriteria(obj))
                {
                    included = true;
                    break;
                }
            }
            foreach (Exclude exclude in Excludes)
            {
                if (exclude.MeetsCriteria(obj))
                {
                    included = false;
                    break;
                }
            }

            return included;
        }

        #endregion

        #region XML Serialization related

        protected override void ReadXmlElements(XmlReader reader)
        {
            base.ReadXmlElements(reader);
            while (reader.IsStartElement(Include.XmlElementName))
            {
                Include include = new Include();
                (include as IXmlSerializable).ReadXml(reader);
                Includes.Add(include);
            }
            while (reader.IsStartElement(Exclude.XmlElementName))
            {
                Exclude exclude = new Exclude();
                (exclude as IXmlSerializable).ReadXml(reader);
                Excludes.Add(exclude);
            }
        }

        protected override void WriteXmlElements(XmlWriter writer)
        {
            base.WriteXmlElements(writer);
            foreach (Include include in Includes)
            {
                XmlUtils.WriteElement(writer, Include.XmlElementName, include);
            }
            foreach (Exclude exclude in Excludes)
            {
                XmlUtils.WriteElement(writer, Exclude.XmlElementName, exclude);
            }
        }

        #endregion
    }
}