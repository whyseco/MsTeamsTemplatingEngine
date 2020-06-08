using Microsoft.Extensions.DependencyInjection;
using MsTeamsTemplatingEngine.Functions;
using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MsTeamsTemplatingEngine
{
	public static class RegisterMsTeamsTemplateEngine
	{
		public static void AddMsTeamsTemplateEngine(this IServiceCollection services)
		{
			services.AddSingleton<LocalizeFunction>();
			services.AddSingleton<LocalizeAssemblyFunction>();
			services.AddSingleton<JsonFunction>();
			services.AddSingleton<InvokeSubmitDataFunction>();
			services.AddTransient<AdaptiveCardTemplateEngine>();
			services.AddTransient<ITemplateLoader, DefaultTemplateLoader>();
		}
	}
}
