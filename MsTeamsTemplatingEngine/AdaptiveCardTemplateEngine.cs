using MsTeamsTemplatingEngine.Functions;
using Scriban;
using Scriban.Runtime;
using System;
using System.IO;

namespace MsTeamsTemplatingEngine
{
	public class AdaptiveCardTemplateEngine
	{
		private readonly ITemplateLoader templateLoader;
		private readonly Func<MsTeamsBuiltinFunctions> msteamsBuiltinFunctionsFactory;

		public AdaptiveCardTemplateEngine(ITemplateLoader templateLoader, Func<MsTeamsBuiltinFunctions> msteamsBuiltinFunctionsFactory)
		{
			this.templateLoader = templateLoader;
			this.msteamsBuiltinFunctionsFactory = msteamsBuiltinFunctionsFactory;
		}

		public AdaptiveCardTemplate Parse(string template)
		{
			var parsedTemplate = Template.Parse(template);
			return new AdaptiveCardTemplate(parsedTemplate);
		}

		public AdaptiveCardTemplate ParseFile(string templateSourcePath)
		{
			var template = File.ReadAllText(templateSourcePath);
			var parsedTemplate = Template.Parse(template, sourceFilePath: templateSourcePath);
			return new AdaptiveCardTemplate(parsedTemplate);
		}


		public string Render(AdaptiveCardTemplate template, object model)
		{
			var scriptObject = new ScriptObject();
			if (model != null)
			{
				scriptObject.Import(model, renamer: r => r.Name, filter: null);
			}
			var context = new TemplateContext()
			{
				TemplateLoader = templateLoader,
				EnableRelaxedMemberAccess = true,
				MemberRenamer = r => r.Name,
				MemberFilter = null
			};
			context.PushGlobal(scriptObject);

			var builtinFunctions = this.msteamsBuiltinFunctionsFactory();
			context.PushGlobal(builtinFunctions);
			var result = template.Template.Render(context);

			return result;
		}
	}
}
