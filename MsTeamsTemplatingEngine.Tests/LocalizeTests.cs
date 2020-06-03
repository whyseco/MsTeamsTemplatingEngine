using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MsTeamsTemplatingEngine.Tests
{
	public class LocalizeUnitTests
	{
		private string NormalizeJson(string json)
		{
			var obj = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(obj);
		}

		[Fact]
		public void WhenLocalizeAnItemThenItWorks()
		{
			var expectedJson = NormalizeJson("{ \"label\" : \"ValueResult\" }");
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddMsTeamsTemplateEngine();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("LocalizeTests/main.tjson");
			var result = engine.Render(template, new { tata = "tata" });
			var normalizedResult = NormalizeJson(result);

			Assert.Equal(expectedJson, normalizedResult);
		}
	}
}
