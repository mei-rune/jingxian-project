

using System;
using System.Collections.Generic;
using System.Text;

namespace jingxian.core.runtime.utilities.CommandLineParser
{
    using jingxian.core.runtime.Resources;

    // 命令行分析器
    // 选项前缀可以是 '/ ', '-', '--', 选项名的大小写不敏感，选项名和选项值之间用‘:’分隔
	public class CommandLineArgumentParser
	{
		private const string SlashCommandPrefix = "/";
		private const string HyphenCommandPrefix = "-";
		private const string LongCommandPrefix = "--";
		private const char TextToValueDivider = ':';
		private readonly char[] _TextToValueDividerCharArray = new char[1] { TextToValueDivider};

		public CommandLineArgumentParser()
		{
		}

		private readonly List<CommandLineArgument> _Arguments = new List<CommandLineArgument>();
		public IList<CommandLineArgument> Arguments
		{
			get
			{
				return _Arguments;
			}
		}

		public CommandLineParseResult Parse(string[] args)
		{
			CommandLineParseResult result = new CommandLineParseResult(args);

			for (int position=0; position<args.Length; position++)
			{
				string givenArg = args[position];

				bool validArgFound = false;

                
				if (string.IsNullOrEmpty(givenArg))
				{
                    // 选项不能为空
                    result.ParseErrors.Add(string.Format(Error.FailedToParseArgument, position));
					continue;
				}
                
				if (givenArg.Length<2)
				{
                    // 选项长度至少为2, 第一个为'-','/','--',第2个为选项名
                    result.ParseErrors.Add(string.Format(Error.ArgumentsMustHaveMinimumLength, givenArg, position));
					continue;
				}

				bool hasArgumentPrefix = givenArg.StartsWith(LongCommandPrefix) || givenArg.StartsWith(SlashCommandPrefix) || givenArg.StartsWith(HyphenCommandPrefix);
				if (!hasArgumentPrefix)
				{
                    result.ParseErrors.Add(string.Format(Error.ArgumentsMustHavePrefix, givenArg, position, SlashCommandPrefix, HyphenCommandPrefix, LongCommandPrefix));
					continue;
				}

				// 是不是以'--'开头
				bool isLong = givenArg.StartsWith(LongCommandPrefix);
                // 去除'-','/','--'开头
				string shrinkedArg = isLong ?	givenArg.Substring(2) : givenArg.Substring(1);
				
                // 用':'分隔选项
				string[] splittedArgs = shrinkedArg.Split(_TextToValueDividerCharArray, 2, StringSplitOptions.None);
				System.Diagnostics.Trace.Assert(splittedArgs.Length<=2);
				System.Diagnostics.Trace.Assert(splittedArgs.Length>0);
				string argumentKey = splittedArgs[0];
				string argumentValue = splittedArgs.Length > 1 ? splittedArgs[1] : string.Empty;

				foreach (CommandLineArgument validArg in Arguments)
				{
                    // 依次匹配预定义的选项
					CommandLineArgument parsedArg;
					if (TryMatch(validArg, argumentKey, argumentValue, position, isLong, out parsedArg))
					{
						validArgFound = true;
                        // 匹配成功，验证它是不是只能是唯一的
						if (parsedArg.OnlyOnce && result.HasArgument(parsedArg.Id))
						{
                            // 已经存在了，且该选项限制只能有一个,所以出错了
							CommandLineArgument existingArg = result.GetArgument(parsedArg.Id);
                            result.ParseErrors.Add(string.Format(Error.IgnoringArgument, givenArg, position, existingArg.Position));
						}
						else
						{
							result.AddArgument(parsedArg);
						}
						break;
					}
				}
                
				if (!validArgFound)
				{
                    // 没有找到匹配的参数
                    result.ParseErrors.Add(string.Format(Error.DoNotMatchAnyArgument, givenArg, position));					
				}
			}
			return result;
		}

		public bool TryMatch(CommandLineArgument arg, string key, string value, int position, bool isLong, out CommandLineArgument parsedArg)
		{
			bool matches = isLong ? key == arg.LongText : key == arg.Text;
			if (matches)
			{
				parsedArg = CommandLineArgument.CreateFromInstance(arg, value, position);
				return true;
			}
			parsedArg = null;
			return false;			
		}

		public string GetUsage()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append( Error.DefaultCommands + "\n");
			foreach (CommandLineArgument arg in _Arguments)
			{
				sb.AppendFormat("{0}{1} ({2}{3}) \t\t {4}", HyphenCommandPrefix, arg.Text, LongCommandPrefix, arg.LongText, arg.HelpText);
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}
}