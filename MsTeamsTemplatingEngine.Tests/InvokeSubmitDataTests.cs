using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MsTeamsTemplatingEngine.Tests
{
	public class InvokeSubmitDataTests
	{
		private string NormalizeJson(string json)
		{
			var obj = JsonConvert.DeserializeObject(json);
			return JsonConvert.SerializeObject(obj);
		}

		[Fact]
		public void WhenConvertingStringThenStringIsConverted()
		{
			var guid = Guid.NewGuid();
			var expectedJson = JsonConvert.SerializeObject(new { data = new { msteams = new { type = "invoke", value = 
				new {
						action = "QA.CancelAskQuestion",
						question = guid 
				} 
			} } });
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddMsTeamsTemplateEngine();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("InvokeSubmitDataTests/main.tjson");
			var result = engine.Render(template, new { data = new { question = guid } });
			var normalizedResult = NormalizeJson(result);

			Assert.Equal(expectedJson, normalizedResult);
		}
	}
}
