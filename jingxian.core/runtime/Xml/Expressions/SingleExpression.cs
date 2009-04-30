

namespace jingxian.core.runtime.Xml.Expressions
{
	public abstract class SingleExpression : XmlExpression
	{
		protected XmlExpression _Expression;

		protected SingleExpression()
			: base()
		{
		}


		public XmlExpression Expression
		{
			get { return _Expression; }
		}
	}
}