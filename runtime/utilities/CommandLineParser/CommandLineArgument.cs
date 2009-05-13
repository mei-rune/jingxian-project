
using System;
using System.Text;

namespace jingxian.core.runtime.utilities.CommandLineParser
{
	public class CommandLineArgument : ICloneable
	{
		private readonly string _Id;
		private readonly string _Text;
		private readonly string _LongText;
		private readonly string _HelpText;
		private readonly bool _IsOptional;
		private readonly bool _OnlyOnce;
		private readonly bool _SupportsValue;
		private string _Value;
		private int _Position = -1;

		protected CommandLineArgument(CommandLineArgument that)
		{
			_Id = that._Id;
			_HelpText = that._HelpText;
			_IsOptional = that._IsOptional;
			_LongText = that._LongText;
			_Text = that._Text;
			_Value = that._Value;
			_SupportsValue = that._SupportsValue;
			_Position = that._Position;
			_OnlyOnce = that.OnlyOnce;
		}

		public CommandLineArgument(string id, string text, string longText, string helpText, bool isOptional, bool onlyOnce)
		{
			_Id = id;
			_Text = text;
			_LongText = longText;
			_HelpText = helpText;
			_IsOptional = isOptional;
			_OnlyOnce = onlyOnce;
		}

		public CommandLineArgument(string id, string text, string longText, string helpText, bool isOptional, bool supportsValue, bool onlyOnce)
			: this(id, text, longText, helpText, isOptional, onlyOnce)
		{
			_SupportsValue = supportsValue;
		}

		public CommandLineArgument(string id, string text, string longText, string helpText, bool isOptional, string defaultValue, bool onlyOnce)
			: this(id, text, longText, helpText, isOptional, onlyOnce)
		{
			_SupportsValue = true;
			_Value = defaultValue;
		}


		public string Id
		{
			get { return _Id; }
		}

		public string Text
		{
			get { return _Text; }
		}

		public string LongText
		{
			get { return _LongText; }
		}

		public string HelpText
		{
			get { return _HelpText; }
		}

		public bool IsOptional
		{
			get { return _IsOptional; }
		}

		public bool OnlyOnce
		{
			get { return _OnlyOnce; }
		}

		public bool SupportsValue
		{
			get { return _SupportsValue; }
		}

		public string Value
		{
			get { return _Value; }
			protected set { _Value = value; }
		}

		public int Position
		{
			get { return _Position; }
			protected set { _Position = value;}
		}

		public static CommandLineArgument CreateFromInstance(CommandLineArgument instance, string value, int position)
		{
			CommandLineArgument result = (CommandLineArgument)((ICloneable)instance).Clone();
			result.Value = value;
			result.Position = position;
			return result;
		}

		object ICloneable.Clone()
		{
			return new CommandLineArgument(this);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendFormat("{0} id='{1}'", Position, Id);
			if (SupportsValue)
			{
				sb.AppendFormat("\tvalue='{0}'", Value);
			}
			return sb.ToString();
		}
	}
}