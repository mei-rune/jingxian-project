

using System;
using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime
{
	public interface IServiceRegistry: IServiceProvider
	{
		bool HasService(string id);
		bool HasService(Type serviceType);
		bool HasService<T>();

		T GetService<T>();
		T GetService<T>(string id);

		bool TryGetService(string id, out object service);
		bool TryGetService(Type serviceType, out object service);
		bool TryGetService<T>(out T service);
		bool TryGetService(Type serviceType, string id, out object service);
		bool TryGetService<T>(string id, out T service);
	}
}