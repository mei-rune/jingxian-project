

using System;
using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
    using jingxian.core.runtime.Resources;

	public class NotExpression : SingleExpression
	{

		public NotExpression(XmlReader reader)
			: base()
		{
			if (reader.IsEmptyElement)
			{
				throw new PlatformConfigurationException(
					Messages.InvalidXmlExpression + Environment.NewLine + Messages.NotEmptyElement);
			}
			else
			{
				reader.ReadStartElement(XmlExpression.NotExpTag);
				if (reader.IsStartElement())
				{
					_Expression = Create(reader);
				}
				reader.ReadEndElement();
			}
		}

		public override bool IsTrueFor(object obj)
		{
			return !_Expression.IsTrueFor(obj);
		}
	}
}