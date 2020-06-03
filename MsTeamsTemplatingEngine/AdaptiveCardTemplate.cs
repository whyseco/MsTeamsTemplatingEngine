using Scriban;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsTeamsTemplatingEngine
{
	public class AdaptiveCardTemplate
	{
		internal AdaptiveCardTemplate(Template template)
		{
			Template = template;
		}

		internal Template Template { get; }
	}
}
