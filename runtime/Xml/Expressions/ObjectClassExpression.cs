

using System;
using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
	public class ObjectClassExpression : XmlExpression
	{
		private string _objectClassName;
		private Type _objectClassType;

		public ObjectClassExpression(XmlReader reader)
		{
			if (reader.IsEmptyElement)
			{
				ReadObjectClass(reader, false);
			}
			else
			{
				ReadObjectClass(reader, true);
			}
		}

        public ObjectClassExpression(string classType)
        {
            _objectClassName = Enforce.ArgumentNotNullOrEmpty(classType, "classType");
        }

		public string ObjectClassName
		{
			get { return _objectClassName; }
		}

		public Type ObjectClassType
		{
			get { return _objectClassType; }
		}


		private void ReadObjectClass(XmlReader reader, bool withEndElement)
		{
			_objectClassName = reader.GetAttribute("classType");
			reader.ReadStartElement(ObjectClassExpTag);
			if (withEndElement)
			{
				reader.ReadEndElement();
			}
		}

		public override bool IsTrueFor(object obj)
		{
			if (_objectClassType == null)
			{
				_objectClassType = Type.GetType(_objectClassName, true);
			}
			return _objectClassType.IsAssignableFrom(obj.GetType());
		}
	}
}