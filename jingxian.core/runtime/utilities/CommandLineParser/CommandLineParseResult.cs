
using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.utilities.CommandLineParser
{
	public class CommandLineParseResult
	{
		private readonly string[] _InitialArguments;
		private readonly List<CommandLineArgument> _Arguments = new List<CommandLineArgument>();
		private readonly List<string> _ParseErrors = new List<string>();

		public CommandLineParseResult(string[] initialArguments)
		{
			_InitialArguments = initialArguments;
		}

		public string[] InitialArguments
		{
			get { return _InitialArguments; }
		}

		public IList<string> ParseErrors
		{
			get { return _ParseErrors; }
		}

		public bool HasErrors
		{
			get { return ParseErrors.Count > 0; }
		}

		private void AppendSummary(StringBuilder sb)
		{
			if (InitialArguments == null || InitialArguments.Length == 0)
			{
                sb.Append(Error.NoArgumentsGiven);
			}
			if (_Arguments.Count > 0)
			{
				sb.Append( Error.Arguments + ":\n");
				foreach (CommandLineArgument arg in _Arguments)
				{
					sb.Append(arg + "\n");
				}
			}
			if (HasErrors)
			{
				sb.Append( Error.Errors + ":\n");
				foreach (string error in ParseErrors)
				{
					sb.Append(error + "\n");
				}
			}
		}

		public string GetSummary()
		{
			StringBuilder sb = new StringBuilder();
			AppendSummary(sb);
			return sb.ToString();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append( Error.CommandLineParseResults + ":\n");
			sb.AppendFormat( Error.ArgumentSuccessfullyParsed + "\n",
			                _Arguments.Count,
			                InitialArguments != null ? InitialArguments.Length : 0
				);
			if (HasErrors)
			{
				sb.AppendFormat( Error.ParseErrorDetected + "\n", ParseErrors.Count);
			}
			AppendSummary(sb);
			return sb.ToString();
		}
        
		public void AddArgument(CommandLineArgument argument)
		{
			if (argument.OnlyOnce && HasArgument(argument.Id))
			{
                throw new ArgumentException(string.Format(Error.ArgumentIsAllowedOnlyOnceAndWasAlreadyGiven, argument, argument.Position, GetArgument(argument.Id).Position), "argument");
			}
			_Arguments.Add(argument);
		}
        
		public CommandLineArgument[] GetArguments(string identifier)
		{
			return _Arguments.FindAll(delegate(CommandLineArgument arg) { return arg.Id == identifier; }).ToArray();
		}
        
		public bool HasArgument(string identifier)
		{
			return _Arguments.Exists(delegate(CommandLineArgument arg) { return arg.Id == identifier; });
		}
        
		public CommandLineArgument GetArgument(string identifier)
		{
			CommandLineArgument[] args = GetArguments(identifier);
			if (args.Length == 0)
			{
                throw new ArgumentException(string.Format(Error.UknownIdentifier, identifier), "identifier");
			}
			if (args.Length == 1)
			{
				return args[0];
			}
			{
                throw new ArgumentException(string.Format(Error.MultipleArgumentsWithIdentifierFound, identifier), "identifier");
			}
		}
	}
}