

using System;
using System.Collections;
using System.Reflection;
using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
	public class ObjectPropertyExpression : XmlExpression
	{
		private string _ObjectPropertyName = string.Empty;
		private string _ObjectPropertyValue = string.Empty;
		private string _ObjectPropertyType;

		public ObjectPropertyExpression(XmlReader reader)
			: base()
		{
			if (reader.IsEmptyElement)
			{
				ReadObjectState(reader, false);
			}
			else
			{
				ReadObjectState(reader, true);
			}
		}

		public ObjectPropertyExpression(string name, string value)
			: base()
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name"); 
			}
			_ObjectPropertyName = name;
			_ObjectPropertyValue = value;
		}

		public string ObjectPropertyName
		{
			get { return _ObjectPropertyName; }
		}

		public string ObjectPropertyValue
		{
			get { return _ObjectPropertyValue; }
		}

		public string ObjectPropertyType
		{
			get { return _ObjectPropertyType; }
		}

		private void ReadObjectState(XmlReader reader, bool withEndElement)
		{
			if (!string.IsNullOrEmpty(reader.GetAttribute("name")))
			{
				_ObjectPropertyName = reader.GetAttribute("name");
			}
			if (!string.IsNullOrEmpty(reader.GetAttribute("value")))
			{
				_ObjectPropertyValue = reader.GetAttribute("value");
			}
			if (!string.IsNullOrEmpty(reader.GetAttribute("type")))
			{
				_ObjectPropertyType = reader.GetAttribute("type");
			}
			reader.ReadStartElement(XmlExpression.ObjectStateExpTag);
			if (withEndElement)
			{
				reader.ReadEndElement();
			}
		}

		public override bool IsTrueFor(object obj)
		{
			PropertyInfo propertyInfo = null;
			if (CheckTypeForProperty(obj.GetType(), out propertyInfo))
			{
				return EvaluateProperty(propertyInfo, obj);
			}
			else
			{
				Type[] interfaces = obj.GetType().GetInterfaces();
				foreach (Type iface in interfaces)
				{
					if (CheckTypeForProperty(iface, out propertyInfo))
					{
						return EvaluateProperty(propertyInfo, obj);
					}
				}
			}
			return false;
		}

		private bool EvaluateProperty(PropertyInfo propertyInfo, object obj)
		{
			object value = propertyInfo.GetValue(obj, null);
			if (value == null)
			{
				return false;
			}
			else if (string.IsNullOrEmpty(_ObjectPropertyValue) && !string.IsNullOrEmpty(_ObjectPropertyType))
			{
				return value.GetType().FullName == _ObjectPropertyType;
			}
			else if (string.IsNullOrEmpty(_ObjectPropertyType) && !string.IsNullOrEmpty(_ObjectPropertyValue) && _ObjectPropertyValue != "*")
			{
				return value.ToString() == _ObjectPropertyValue;
			}
			else if (_ObjectPropertyValue == "*")
			{
				IEnumerable enumerable = value as IEnumerable;
				if (enumerable != null)
				{
					if (enumerable.GetEnumerator().MoveNext())
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private bool CheckTypeForProperty(Type type, out PropertyInfo propertyInfo)
		{
			propertyInfo = type.GetProperty(_ObjectPropertyName);
			if (propertyInfo != null)
			{
				return true;
			}

			return false;
		}
	}
}