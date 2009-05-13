

using System;

namespace jingxian.core.runtime
{
	public struct PredefinedService: IEquatable<PredefinedService>
	{

		private readonly string _Id;
		private readonly Type _implementation;
		private readonly Type _Service;

		public PredefinedService(string id, Type serviceType, Type implementationType)
		{
			_Id = id;
			_implementation = implementationType;
			_Service = serviceType;
		}

		public string Id
		{
            get { return _Id; }
		}

		public Type Implementation
		{
            get { return _implementation; }
		}

		public Type Service
		{
            get { return _Service; }
		}

		public override string ToString()
		{
			return Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is PredefinedService))
				return false;

			return Equals((PredefinedService) obj);
		}

		public static bool operator ==(PredefinedService svc1, PredefinedService svc2)
		{
			return svc1.Equals(svc2);
		}

		public static bool operator !=(PredefinedService svc1, PredefinedService svc2)
		{
			return !svc1.Equals(svc2);
		}

		public bool Equals(PredefinedService other)
		{
			return Id == other.Id;
		}

	}
}