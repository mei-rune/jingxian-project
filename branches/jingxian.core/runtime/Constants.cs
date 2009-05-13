


namespace jingxian.core.runtime
{
	public static class Constants
	{
		public static class Bundles
		{
			public const string API = "jingxian.core.runtime"; 

			public const string Internal = "jingxian.core.runtime.simpl";  
		}

		public static class Points
		{
			public const string Applications = "jingxian.core.runtime.applications";

            public const string Initializer = "jingxian.core.runtime.initializer"; 

			public const string Services = "jingxian.core.runtime.services"; 

            /// <summary>
            /// 所有本扩展点的扩展将作为ioc容器的组件，注册到ioc容器中
            /// </summary>
			public const string Components = "jingxian.core.runtime.components";

			public const string XmlSchemas = "jingxian.core.runtime.xmlSchemas";

			public const string CommandLineArguments = "jingxian.core.runtime.commandLineArguments";

		}
	}
}
