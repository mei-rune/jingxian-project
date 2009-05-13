

using System;
using System.Xml;
using jingxian.core.runtime.utilities;

namespace jingxian.core.runtime.Xml.Serialization
{
	[Serializable]
	public abstract class XmlSerializableIdentifiable: XmlSerializable, IRuntimePart
	{


		protected const string IdPropertyName = "Id";
		public const string XmlIdAttributeName = "id";
		private string _Id;

		public string Id
		{
            get { return _Id; }
            protected set { _Id = value; }
		}


		protected XmlSerializableIdentifiable()
		{
		}

        protected XmlSerializableIdentifiable(string id)
        {
            Id = Enforce.ArgumentNotNullOrEmpty(id, "id");
        }

		protected override void ReadXmlAttributes(XmlReader reader)
		{
			base.ReadXmlAttributes(reader);
			Id = XmlUtils.ReadRequiredAttributeString(reader, XmlIdAttributeName);
		}

		protected override void WriteXmlAttributes(XmlWriter writer)
		{
			base.WriteXmlAttributes(writer);
			XmlUtils.WriteAttribute(writer, XmlIdAttributeName, Id);
		}

		public override string ToString()
		{
			return Id;
		}

	}
}