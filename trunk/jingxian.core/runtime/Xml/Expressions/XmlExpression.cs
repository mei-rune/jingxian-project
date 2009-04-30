
using System.IO;
using System.Xml;
using jingxian.core.runtime.Resources;

namespace jingxian.core.runtime.Xml.Expressions
{

	public abstract class XmlExpression
	{
		#region constant

		public const string EnablementTag = "enablement";
		public const string AndExpTag = "and";
		public const string OrExpTag = "or";
		public const string NotExpTag = "not";
		public const string SystemPropertyExpTag = "systemProperty";
		public const string ObjectClassExpTag = "object";
		public const string ObjectStateExpTag = "property";
		public const string BundleStateExpTag = "bundleState";

		#endregion

		protected XmlExpression()
		{
		}


		public abstract bool IsTrueFor(object obj);



		public static XmlExpression Create(string expressionConfig)
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			settings.IgnoreWhitespace = true;

			using (StringReader sr = new StringReader(expressionConfig))
			{
				using (XmlReader reader = XmlReader.Create(sr, settings))
				{
					return Create(reader);
				}
			}
		}

		public static XmlExpression Create(XmlReader reader)
		{
			if (reader.IsStartElement(AndExpTag))
			{
				return new AndExpression(reader);
			}
			if (reader.IsStartElement(OrExpTag))
			{
				return new OrExpression(reader);
			}
			if (reader.IsStartElement(NotExpTag))
			{
				return new NotExpression(reader);
			}
			if (reader.IsStartElement(SystemPropertyExpTag))
			{
				return new SystemPropertyExpression(reader);
			}
			if (reader.IsStartElement(ObjectClassExpTag))
			{
				return new ObjectClassExpression(reader);
			}
			if (reader.IsStartElement(ObjectStateExpTag))
			{
				return new ObjectPropertyExpression(reader);
			}
			if (reader.IsStartElement(BundleStateExpTag))
			{
				return new BundleStateExpression(reader);
			}

			throw new PlatformConfigurationException(
				Messages.UnrecognizedXmlExpressionElement + reader.LocalName);
		}
	}
}