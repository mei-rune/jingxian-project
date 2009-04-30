

using System;
using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
	public class ObjectClassExpression : XmlExpression
	{
		private string _ObjectClassName;
		private Type _ObjectClassType;

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
			if (string.IsNullOrEmpty(classType))
			{
				throw new ArgumentNullException("classType");
			}
			_ObjectClassName = classType;
		}

		public string ObjectClassName
		{
			get { return _ObjectClassName; }
		}

		public Type ObjectClassType
		{
			get { return _ObjectClassType; }
		}


		private void ReadObjectClass(XmlReader reader, bool withEndElement)
		{
			_ObjectClassName = reader.GetAttribute("classType");
			reader.ReadStartElement(ObjectClassExpTag);
			if (withEndElement)
			{
				reader.ReadEndElement();
			}
		}

		public override bool IsTrueFor(object obj)
		{
			if (_ObjectClassType == null)
			{
				_ObjectClassType = Type.GetType(_ObjectClassName, true);
			}
			return _ObjectClassType.IsAssignableFrom(obj.GetType());
		}
	}
}