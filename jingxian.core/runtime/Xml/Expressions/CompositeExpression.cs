

using System.Collections.Generic;

namespace jingxian.core.runtime.Xml.Expressions
{
	public abstract class CompositeExpression : XmlExpression
	{
		protected List<XmlExpression> _Expressions;

		protected CompositeExpression()
			: base()
		{
			_Expressions = new List<XmlExpression>();
		}


		public List<XmlExpression> Expressions
		{
			get { return _Expressions; }
		}
	}
}