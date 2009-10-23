

using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace jingxian.core.runtime.simpl
{
	public interface IExtensionBuilder
	{
		T Build<T>(IExtension extension);
	}
}