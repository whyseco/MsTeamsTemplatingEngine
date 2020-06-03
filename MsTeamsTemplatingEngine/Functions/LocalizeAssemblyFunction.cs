using Microsoft.Extensions.Localization;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MsTeamsTemplatingEngine.Functions
{
	public class LocalizeAssemblyFunction : IScriptCustomFunction
	{
		public const string CurrentAssemblyKey = "LocalizeCurrentAssemblyKey";

		public LocalizeAssemblyFunction()
		{
		}

		public object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			if (arguments.Count == 0)
				throw new ArgumentException("Argument can't be empty", nameof(arguments));

			var assembly = Assembly.Load(arguments[0].ToString());
			if (!context.Tags.ContainsKey(CurrentAssemblyKey))
				context.Tags.Add(CurrentAssemblyKey, assembly);
			else
				context.TagsCurrentLocal.Add(CurrentAssemblyKey, assembly);
			return null;
		}

		public ValueTask<object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			var result = this.Invoke(context, callerContext, arguments, blockStatement);

			return new ValueTask<object>(result);
		}
	}
}
