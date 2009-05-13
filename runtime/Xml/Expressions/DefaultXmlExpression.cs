

namespace jingxian.core.runtime.Xml.Expressions
{
	public class DefaultXmlExpression : XmlExpression
	{
		private static XmlExpression _instance = new DefaultXmlExpression();

		public static XmlExpression Instance
		{
			get { return _instance; }
		}

		private DefaultXmlExpression()
			: base()
		{
		}

		public override bool IsTrueFor(object obj)
		{
			return true;
		}
	}
}