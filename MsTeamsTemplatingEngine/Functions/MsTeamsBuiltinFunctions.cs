using Microsoft.Extensions.DependencyInjection;
using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsTeamsTemplatingEngine.Functions
{
	public class MsTeamsBuiltinFunctions : ScriptObject
	{
		public MsTeamsBuiltinFunctions(IServiceProvider serviceProvider)
		{
			SetValue("localize", serviceProvider.GetRequiredService<LocalizeFunction>(), true);
			SetValue("t", serviceProvider.GetRequiredService<LocalizeFunction>(), true);
			SetValue("locAssembly", serviceProvider.GetRequiredService<LocalizeAssemblyFunction>(), true);
			SetValue("tAssembly", serviceProvider.GetRequiredService<LocalizeAssemblyFunction>(), true);
			SetValue("json", serviceProvider.GetRequiredService<JsonFunction>(), true);
			SetValue("invokeSubmitData", serviceProvider.GetRequiredService<InvokeSubmitDataFunction>(), true);
		}
	}
}
