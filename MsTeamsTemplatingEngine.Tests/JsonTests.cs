using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MsTeamsTemplatingEngine.Tests
{
	public class JsonTests
	{
		private string NormalizeJson(string json)
		{
			var obj = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(obj);
		}

		[Fact]
		public void WhenConvertingStringThenStringIsConverted()
		{
			var expectedJson = JsonConvert.SerializeObject(new { label = "LabelContent" });
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddMsTeamsTemplateEngine();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("JsonTests/main.tjson");
			var result = engine.Render(template, new { label = "LabelContent" });
			var normalizedResult = NormalizeJson(result);

			Assert.Equal(expectedJson, normalizedResult);
		}

		[Fact]
		public void WhenConvertingComplexObjectThenObjectIsConverted()
		{
			var expectedJson = JsonConvert.SerializeObject(new { content = new { label = "LabelContent" } });
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddMsTeamsTemplateEngine();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("JsonTests/complex.tjson");
			var result = engine.Render(template, new { data = new { label = "LabelContent" } });
			var normalizedResult = NormalizeJson(result);

			Assert.Equal(expectedJson, normalizedResult);
		}
	}
}
