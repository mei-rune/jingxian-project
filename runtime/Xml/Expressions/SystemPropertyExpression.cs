

using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
	public class SystemPropertyExpression : XmlExpression
	{
		private string _SystemPropertyName;
		private string _SystemPropertyValue;


		public SystemPropertyExpression(XmlReader reader) : base()
		{
			if (reader.IsEmptyElement)
			{
				ReadSystemProperty(reader, false);
			}
			else
			{
				ReadSystemProperty(reader, true);
			}
		}


		public string SystemPropertyName
		{
			get { return _SystemPropertyName; }
		}


		public string SystemPropertyValue
		{
			get { return _SystemPropertyValue; }
		}

		private void ReadSystemProperty(XmlReader reader, bool withEndElement)
		{
			_SystemPropertyName = reader.GetAttribute("name");
			_SystemPropertyValue = reader.GetAttribute("value");
			reader.ReadStartElement(XmlExpression.SystemPropertyExpTag);
			if (withEndElement)
			{
				reader.ReadEndElement();
			}
		}

		public override bool IsTrueFor(object obj)
		{
			/// @TODO: 请实现它
			return true;
		}
	}
}