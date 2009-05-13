

using System;
using System.Xml.Serialization;

namespace jingxian.core.runtime
{
	public interface ICacheService
	{
        string CachePath { get; }

		bool TryGetTimeOfSerialization(string key, out DateTime result);

		bool TryXmlDeserialize<T>(string key, T serializable) where T : IXmlSerializable;
        
		void XmlSerialize<T>(string key, T serializable);

		void Delete(string key);
	}
}