

using System;
using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
    using jingxian.core.runtime.Resources;

	public class OrExpression : CompositeExpression
	{
		public OrExpression(XmlReader reader)
			: base()
		{
			if (reader.IsEmptyElement)
			{
				throw new PlatformConfigurationException(
					Messages.InvalidXmlExpression + Environment.NewLine + Messages.OrEmptyElement);
			}
			else
			{
				reader.ReadStartElement(XmlExpression.OrExpTag);
				while (reader.IsStartElement())
				{
					_Expressions.Add(Create(reader));
				}
				reader.ReadEndElement();
			}
		}

		public override bool IsTrueFor(object obj)
		{
			bool eval = true;

			foreach (XmlExpression exp in _Expressions)
			{
				eval |= exp.IsTrueFor(obj);

				if (!eval)
				{
					break;
				}
			}

			return eval;
		}
	}
}