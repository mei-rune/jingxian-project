
using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace jingxian.core.runtime
{
    using jingxian.core.runtime.Resources;

	[Serializable]
	public sealed class WriteOnceViolatedException: InvalidOperationException
	{
		private readonly string _MemberName;

		public WriteOnceViolatedException()
			: base(ExceptionMessages.WriteOnceSemanticViolated)
		{
		}

		public WriteOnceViolatedException(string memberName)
			: base(GetMessage(memberName))
		{
			_MemberName = memberName;
		}

		public WriteOnceViolatedException(string memberName, Exception inner)
			: base(GetMessage(memberName), inner)
		{
			_MemberName = memberName;
		}

		private WriteOnceViolatedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		private static string GetMessage(string memberName)
		{
			return string.Format(CultureInfo.InvariantCulture,
				ExceptionMessages.WriteOnceSemanticViolatedMember,
				memberName);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("MemberName", MemberName);
		}

		public string MemberName
		{
            get { return _MemberName ?? string.Empty; }
		}

	}
}