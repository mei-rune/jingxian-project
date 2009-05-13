

using System.Diagnostics.CodeAnalysis;

namespace jingxian.core.runtime
{
	[ExtensionContract(Constants.Points.Services)]
	public interface IService: IRuntimePart
	{
		void Start();

		void Stop();
	}
}