

using System;
using System.Xml;
using jingxian.core.runtime.Resources;

namespace jingxian.core.runtime.Xml.Expressions
{
	public class AndExpression : CompositeExpression
	{

		public AndExpression(XmlReader reader)
			: base()
		{
			if (reader.IsEmptyElement)
			{
				throw new PlatformConfigurationException(
					Messages.InvalidXmlExpression + Environment.NewLine + Messages.AndEmptyElement);
			}
			else
			{
				reader.ReadStartElement(XmlExpression.AndExpTag);
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
				eval &= exp.IsTrueFor(obj);

				if (!eval)
				{
					break;
				}
			}

			return eval;
		}
	}
}