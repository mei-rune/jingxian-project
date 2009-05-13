

namespace jingxian.core.runtime
{

	public interface IExtensionAware: IConfigurable<IExtension>
	{
        IExtension Extension { get; }
	}
}
