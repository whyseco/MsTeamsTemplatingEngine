using Scriban;
using Scriban.Parsing;
using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MsTeamsTemplatingEngine
{
	public class DefaultTemplateLoader : ITemplateLoader
	{
		const string defaultSharedPath = "Shared";

		public string GetPath(TemplateContext context, SourceSpan callerSpan, string templateName)
		{
			var rootPath = Path.GetFullPath(Path.GetDirectoryName(context.CurrentSourceFile));
			var currentExtension = Path.GetExtension(context.CurrentSourceFile);
			var templateExtension = Path.GetExtension(templateName);

			if (currentExtension != null && string.IsNullOrEmpty(templateExtension))
				templateName += currentExtension;

			var testedPath = Path.Combine(rootPath, templateName);
			if (File.Exists(testedPath)) return testedPath;

			var fullPath = Environment.CurrentDirectory;
			while (rootPath.StartsWith(fullPath))
			{
				testedPath = Path.Combine(rootPath, defaultSharedPath, templateName);
				if (File.Exists(testedPath)) return testedPath;
				rootPath = Path.GetFullPath(Path.Combine(rootPath, ".."));
			}

			return templateName;
		}

		public string Load(TemplateContext context, SourceSpan callerSpan, string templatePath)
		{
			return File.ReadAllText(templatePath);
		}

		public ValueTask<string> LoadAsync(TemplateContext context, SourceSpan callerSpan, string templatePath)
		{
			var content = File.ReadAllText(templatePath);

			return new ValueTask<string>(content);
		}
	}
}
