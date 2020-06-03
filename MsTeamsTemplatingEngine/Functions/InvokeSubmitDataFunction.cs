using Newtonsoft.Json.Linq;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsTeamsTemplatingEngine.Functions
{
	public class InvokeSubmitDataFunction : IScriptCustomFunction
	{
		public object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			var submitData = new
			{
				msteams = new
				{
					type = "invoke",
					value = (object)null
				}
			};

			var jsonSubmitData = JObject.FromObject(submitData);
			var invokeData = JObject.FromObject(arguments[1]);
			var actionData = JObject.FromObject(new { action = arguments[0] });
			actionData.Merge(invokeData);

			jsonSubmitData["msteams"]["value"] = actionData;

			return "\"data\" : " + jsonSubmitData.ToString();
		}

		public ValueTask<object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments, ScriptBlockStatement blockStatement)
		{
			var result = this.Invoke(context, callerContext, arguments, blockStatement);

			return new ValueTask<object>(result);
		}
	}
}
