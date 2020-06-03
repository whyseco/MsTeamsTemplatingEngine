using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MsTeamsTemplatingEngine.Tests
{
	public class AdaptiveCardTests
	{
		[Fact]
		public void WhenRenderingAdaptiveCardThenItWorks()
		{
			var serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging();
			serviceCollection.AddLocalization();
			serviceCollection.AddMsTeamsTemplateEngine();
			var provider = serviceCollection.BuildServiceProvider();
			var engine = new AdaptiveCardTemplateEngine(new DefaultTemplateLoader(), () => new Functions.MsTeamsBuiltinFunctions(provider));

			var template = engine.ParseFile("AdaptiveCardTests/main.tjson");
			var card = engine.RenderToAdaptiveCard(template, new { asker = new { realName = "Yann ROBIN", jobInfo = "CTO @ Whyse" } });

			Assert.NotNull(card);
		}
	}
}
