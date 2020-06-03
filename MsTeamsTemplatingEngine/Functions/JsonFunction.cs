using Newtonsoft.Json;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MsTeamsTemplatingEngine.Functions
{
	public class JsonFunction : IScriptCustomFunction
	{
		public object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			return JsonConvert.SerializeObject(arguments[0]);
		}

		public ValueTask<object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			var result = this.Invoke(context, callerContext, arguments, blockStatement);
			return new ValueTask<object>(result);
		}
	}
}
