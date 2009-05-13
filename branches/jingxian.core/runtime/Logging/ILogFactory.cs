

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace jingxian.logging
{
	public interface ILogFactory
	{
        ILog GetLogger(Type classType);

        ILog GetLogger(String className);

        ILogFactory NewFactory(string className);

        ILogFactory NewFactory(Type classType);
	}

    public interface ILogConfiguration
    {
        void Init(string path);
    }
}
