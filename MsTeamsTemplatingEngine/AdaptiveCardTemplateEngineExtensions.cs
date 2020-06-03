using AdaptiveCards;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsTeamsTemplatingEngine
{
	public static class AdaptiveCardTemplateEngineExtensions
	{
		public static AdaptiveCard RenderToAdaptiveCard(this AdaptiveCardTemplateEngine engine, AdaptiveCardTemplate template, object model)
		{
			var result = engine.Render(template, model);

			var card = AdaptiveCard.FromJson(result);

			return card.Card;
		}
	}
}
