
using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace jingxian.core.runtime
{
using jingxian.core.runtime.Resources;

	[Serializable]
	public sealed class ComponentLookupException: RuntimeException
	{
		private readonly string _componentId;

		public ComponentLookupException()
		{
		}

		public ComponentLookupException(string componentId)
			: base(GetMessage(componentId))
		{
			_componentId = componentId;
		}

		public ComponentLookupException(string componentId, Exception inner)
			: base(GetMessage(componentId), inner)
		{
			_componentId = componentId;
		}

		private ComponentLookupException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		private static string GetMessage(string componentId)
		{
			return string.Format(CultureInfo.InvariantCulture,
				ExceptionMessages.ComponentNotFound,
				componentId);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ComponentId", ComponentId);
		}

		public string ComponentId
		{
            get { return _componentId ?? string.Empty; }
		}
	}
}
