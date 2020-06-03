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
	public class LocalizeFunction : IScriptCustomFunction
	{
		private readonly IStringLocalizerFactory stringLocalizerFactory;

		public LocalizeFunction(IStringLocalizerFactory stringLocalizerFactory)
		{
			this.stringLocalizerFactory = stringLocalizerFactory;
		}

		public object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			if (arguments.Count == 0)
				throw new ArgumentException("Argument can't be empty", nameof(arguments));

			var baseName = Path.Combine(
				Path.GetDirectoryName(context.CurrentSourceFile),
				Path.GetFileNameWithoutExtension(context.CurrentSourceFile)
				)
				.Replace("\\", ".");

			context.TagsCurrentLocal.TryGetValue(LocalizeAssemblyFunction.CurrentAssemblyKey, out object assembly);
			if (assembly is null)
				context.Tags.TryGetValue(LocalizeAssemblyFunction.CurrentAssemblyKey, out assembly);
			if (assembly is null)
				throw new ArgumentNullException("To use localize function you must specify an assembly from where to load the ressource \"locassembly 'assembly name'\"");

			var stringLocalizer = this.stringLocalizerFactory.Create(baseName, (assembly as Assembly).FullName);

			var key = arguments[0].ToString();
			var args = arguments.Skip(1).ToList();

			return stringLocalizer.GetString(key, args);
		}

		public ValueTask<object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			var result = this.Invoke(context, callerContext, arguments, blockStatement);

			return new ValueTask<object>(result);
		}
	}
}
