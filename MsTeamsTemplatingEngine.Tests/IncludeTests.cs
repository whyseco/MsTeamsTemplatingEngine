using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using Xunit;

namespace MsTeamsTemplatingEngine.Tests
{
	public class IncludeTests
	{
		private string NormalizeJson(string json)
		{
			var obj = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(obj);
		}

		[Fact]
		public void WhenIncludingAFileItWorks()
		{
			var expectedJson = JsonConvert.SerializeObject(new { test = 55, toto = "tata" });
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			serviceCollection.AddMsTeamsTemplateEngine();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("IncludeTests/WhenIncludingAFileItWorks/main.tjson");
			var result = engine.Render(template, new { tata = "tata" });
			var normalizedResult = NormalizeJson(result);

			Assert.Equal(expectedJson, normalizedResult);
		}

		[Fact]
		public void WhenIncludingASharedFileItWorks()
		{
			var expectedJson = JsonConvert.SerializeObject(new { test = 55, toto = "tata" });
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			serviceCollection.AddMsTeamsTemplateEngine();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("IncludeTests/WhenIncludingASharedFileItWorks/main.tjson");
			var result = engine.Render(template, new { tata = "tata" });
			var normalizedResult = NormalizeJson(result);

			Assert.Equal(expectedJson, normalizedResult);
		}
	}
}
