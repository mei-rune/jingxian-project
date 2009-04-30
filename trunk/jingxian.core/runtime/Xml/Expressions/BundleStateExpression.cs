

using System.Xml;

namespace jingxian.core.runtime.Xml.Expressions
{
	public class BundleStateExpression : XmlExpression
	{
		private string _bundleId;
		private BundleState _BundleState;

		public BundleStateExpression(XmlReader reader)
			: base()
		{
			if (reader.IsEmptyElement)
			{
				ReadBundleState(reader, false);
			}
			else
			{
				ReadBundleState(reader, true);
			}
		}

		public string BundleId
		{
			get { return _bundleId; }
		}

		public BundleState BundleState
		{
			get { return _BundleState; }
		}

		private void ReadBundleState(XmlReader reader, bool withEndElement)
		{
			_bundleId = reader.GetAttribute("id");
			string bundleStateValue = reader.GetAttribute("value");
			reader.ReadStartElement(XmlExpression.BundleStateExpTag);
			if (withEndElement)
			{
				reader.ReadEndElement();
			}

			_BundleState = ParseBundleState(bundleStateValue);
		}

		private BundleState ParseBundleState(string bundleStateValue)
		{
			BundleState bundleState;

			switch (bundleStateValue)
			{
				case "installed":
					bundleState = BundleState.Installed;
					break;

				case "activated":
					bundleState = BundleState.Activated;
					break;

				default:
					bundleState = BundleState.Unknown;
					break;
			}

			return bundleState;
		}


		public override bool IsTrueFor(object obj)
		{
			IBundle bundle = obj as IBundle;
			if (bundle != null)
			{
				if (BundleId == "*")
				{
					return BundleState == bundle.State;
				}
				else
				{
					if (BundleId == bundle.Id)
					{
						return BundleState == bundle.State;
					}
				}
			}
			return false;
		}
	}
}