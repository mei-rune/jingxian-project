

using System.Collections.Generic;
using jingxian.core.runtime.utilities.CommandLineParser;


namespace jingxian.core.runtime
{
	public interface ICommandLineArguments
	{
        CommandLineArgument Verbose { get;  }
        CommandLineArgument Debug { get; }
        CommandLineArgument Clean { get; }
        CommandLineArgument Consolelog { get; }
        CommandLineArgument Help { get; }
        CommandLineArgument Quit { get; }
        CommandLineArgument QuitWait { get; }
        CommandLineArgument Version { get; }

        bool IsVerboseDefined { get; }
        bool IsDebugDefined { get; }
        bool IsCleanDefined { get; }
        bool IsConsoleLogDefined { get; }
        bool IsHelpDefined { get; }
        bool IsQuitDefined { get; }
        bool IsQuitWaitDefined { get; }
        bool IsVersionDefined { get; }
        bool Succeeded { get; }

		string GetSummary();

		string GetUsage();
	}
}
