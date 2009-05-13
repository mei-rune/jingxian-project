

using System.Collections.Generic;
using jingxian.core.runtime.utilities.CommandLineParser;

namespace jingxian.core.runtime.simpl
{
	public class CommandLineArguments: ICommandLineArguments
	{
		#region command line argument ids
		public const string VerboseArgumentId = "verbose";
		public const string DebugArgumentId = "debug";
		public const string CleanArgumentId = "clean";
		public const string ConsolelogArgumentId = "consolelog";
		public const string HelpArgumentId = "help";
		public const string QuitArgumentId = "quit";
		public const string QuitWaitArgumentId = "quitwait";
		public const string VersionArgumentId = "version";
        public const string VariantArgumentId = "variant";
		#endregion

		#region command line argument prototypes
		private readonly CommandLineArgument _verboseArgument = new CommandLineArgument(VerboseArgumentId, "v", "verbose", "If to write verbose messages. Default is true.", true, bool.TrueString, true);
		private readonly CommandLineArgument _debugArgument = new CommandLineArgument(DebugArgumentId, "d", "debug", "Debug the platform. Has no effect yet.", true, false, true);
		private readonly CommandLineArgument _cleanArgument = new CommandLineArgument(CleanArgumentId, "c", "clean", "Clean up local application data and exit.", true, false, true);
		private readonly CommandLineArgument _consolelogArgument = new CommandLineArgument(ConsolelogArgumentId, "clog", "consolelog", "If to mirror the platform's error log to the console. Has no effect yet.", true, false, true);
		private readonly CommandLineArgument _helpArgument = new CommandLineArgument(HelpArgumentId, "h", "help", "Show help and exit.", true, false, true);
		private readonly CommandLineArgument _quitArgument = new CommandLineArgument(QuitArgumentId, "q", "quit", "Launch and quit as soon as possible.", true, false, true);
		private readonly CommandLineArgument _quitWaitArgument = new CommandLineArgument(QuitWaitArgumentId, "qw", "quitwait", "Launch and quit as soon as possible, but wait for user input.", true, false, true);
		private readonly CommandLineArgument _versionArgument = new CommandLineArgument(VersionArgumentId, "ver", "version", "Print version info and exit.", true, false, true);
		#endregion

		private readonly CommandLineArgumentParser _parser = new CommandLineArgumentParser();
		private readonly CommandLineParseResult _parseResult;


		public CommandLineArguments()
			: this(new string[0])
		{
		}

		public CommandLineArguments(string[] arguments)
		{
			_parser.Arguments.Add(_verboseArgument);
			_parser.Arguments.Add(_debugArgument);
			_parser.Arguments.Add(_cleanArgument);
			_parser.Arguments.Add(_consolelogArgument);
			_parser.Arguments.Add(_helpArgument);
			_parser.Arguments.Add(_quitArgument);
			_parser.Arguments.Add(_quitWaitArgument);
			_parser.Arguments.Add(_versionArgument);

			_parseResult = _parser.Parse(arguments);
		}

		#region Arguments
		public CommandLineArgument Verbose
		{
			get
			{
				return _parseResult.GetArgument(VerboseArgumentId);
			}
		}
		public CommandLineArgument Debug
		{
			get
			{
				return _parseResult.GetArgument(DebugArgumentId);
			}
		}
		public CommandLineArgument Clean
		{
			get
			{
				return _parseResult.GetArgument(CleanArgumentId);
			}
		}
		public CommandLineArgument Consolelog
		{
			get
			{
				return _parseResult.GetArgument(ConsolelogArgumentId);
			}
		}
		public CommandLineArgument Help
		{
			get
			{
				return _parseResult.GetArgument(HelpArgumentId);
			}
		}
		public CommandLineArgument Quit
		{
			get
			{
				return _parseResult.GetArgument(QuitArgumentId);
			}
		}
		public CommandLineArgument QuitWait
		{
			get
			{
				return _parseResult.GetArgument(QuitWaitArgumentId);
			}
		}
		public CommandLineArgument Version
		{
			get
			{
				return _parseResult.GetArgument(VersionArgumentId);
			}
		}
		#endregion

		#region IsDefined
		public bool IsVerboseDefined
		{
			get
			{
				return _parseResult.HasArgument(VerboseArgumentId);
			}
		}
		public bool IsDebugDefined
		{
			get
			{
				return _parseResult.HasArgument(DebugArgumentId);
			}
		}
		public bool IsCleanDefined
		{
			get
			{
				return _parseResult.HasArgument(CleanArgumentId);
			}
		}
		public bool IsConsoleLogDefined
		{
			get
			{
				return _parseResult.HasArgument(ConsolelogArgumentId);
			}
		}
		public bool IsHelpDefined
		{
			get
			{
				return _parseResult.HasArgument(HelpArgumentId);
			}
		}
		public bool IsQuitDefined
		{
			get
			{
				return _parseResult.HasArgument(QuitArgumentId);
			}
		}
		public bool IsQuitWaitDefined
		{
			get
			{
				return _parseResult.HasArgument(QuitWaitArgumentId);
			}
		}
		public bool IsVersionDefined
		{
			get
			{
				return _parseResult.HasArgument(VersionArgumentId);
			}
		}
		#endregion

		public bool Succeeded
		{
			get
			{
				return !(_parseResult.HasErrors);
			}
		}
		public string GetSummary()
		{
			return _parseResult.GetSummary();
		}
		public string GetUsage()
		{
			return _parser.GetUsage();
		}

        //public IDictionary<string, object> Context
        //{
        //    get { return _parseResult.GetArgument(VariantArgumentId); }
        //}
    }
}