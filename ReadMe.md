# MsTeams Template Engine

This project use [Scriban](https://github.com/lunet-io/scriban/) to create a template engine for [AdaptiveCards](https://adaptivecards.io/)

## Why

Using adaptive cards website we can have a json with data binding. 
A non technical person can create a complex adaptive card using only WYSIWYG !!
But a technical person need to integrate this JSON inside an application.
To do so you need an engine that can parse and interpret the json to send it using the bot framework.

Microsoft already provide a template engine but it does not have the following requirements :

- Include other templates in one template (usefull to separate your template in small parts)
- I18n function : We don't want to have a template per language, so we must have a way to translate part of the template
- JSON helpers : To have a reliable way of adding variable inside the json they must be encoded using json helpers

## Installation

```powershell
Add-Package MsTeams.TemplatingEngine
Add-Package Microsoft.Extensions.Localization
```

In `Startup.cs` add in `ConfigureServices`

```csharp
services.AddMsTeamsTemplateEngine()
services.AddLocalization()
```

## How to parse and render a template

```csharp
public class ProductController
{
	readonly AdaptiveCardTemplateEngine engine;

	public ProductController(AdaptiveCardTemplateEngine engine)
	{
		this.engine = engine;
	}

	public void Show()
	{
		var template = engine.ParseFile("Product/show.tjson");
		var card = engine.RenderToAdaptiveCard(template, new { id = 5, title = "Chorizo", price = 5.99 });
		// Send card to MsTeams using Bot Framework
	}
}
```

## The template language

The document syntax is available on [Scriban page](https://github.com/lunet-io/scriban/blob/master/doc/language.md)

### Include
You can include a template in a template. 

#### Simple case

Folder structure
```
.
├── main.tjson
└── myinclude.tjson
```

Include syntax
```
include 'myinclude.tjson'
````

Extension is not mandatory, if not provided it will use the includer extension
```
include 'myinclude'
````


#### Shared case

The template loader will look in a folder call `Shared` if the template is not algonside the current template.

Folder structure
```
.
├── main.tjson
└── Shared/
    └── myinclude.tjson
```

Include syntax
```
include 'myinclude'
````


Also it will look in descendent folder for a `Shared` containing your template

Folder structure
```
.
├── Views/
│   └── Product/
│       └── main.tjson
└── Shared/
    └── myinclude.tjson
```

Include syntax
```
include 'myinclude'
````

### JSON Encoding

To encode a value to json you must use the `json` function

#### Example
#### Simple object
```csharp
var result = engine.Render(template, new { label = "LabelContent" });
```


Template :
```json
{
	"label": {{ json label }}
}
```

Output :
```json
{
	"label": "LabelContent"
}
```

#### Complex object
```csharp
var result = engine.Render(template, new { data = new { label = "LabelContent" } });
```


Template :
```json
{
	"content": {{ json data }}
}
```

Output :
```json
{
	"content": {
		"label": "LabelContent"
	}
}
```

### i18n Function

We use the .Net Localization mechanism `IStringLocalizer` and `IStringLocalizerFactory`. 
To get a localized string, you must first declare at the assembly from where the resx will be loaded using `tAssembly 'AssemblyName'`.
And then you can get a translated string by using the `t` function : `t 'TranslateKeyInResx'`

```json
{{ tAssembly 'MsTeamsTemplatingEngine.Tests' }}
{
	"label": "{{ t 'TestKey' }}"
}
```

_See LocalizeTests_ for full example

### Invoke action helpers

This is tightly linked to **Whyse** routing system for MsTeams action.
When using adaptive cards you can get the full `data` payload for an *invoke* submit action using the `invokeSubmitData` function.

Code :
```csharp
var result = engine.Render(template, new { productData = new { productId = 5 } });
```

Template :
```json
{
	"type": "ActionSet",
	"actions": [
		{
			"title": "Buy Product",
			"type": "Action.Submit",
			"style": "positive",
			"id": "send",
			{{ invokeSubmitData '/product/buy' productData }}
		}
	]
}
```

Output
```json
{
	"type": "ActionSet",
	"actions": [
		{
			"title": "Buy Product",
			"type": "Action.Submit",
			"style": "positive",
			"id": "send",
			"data": {
				"msteams": {
					"type": "invoke",
					"value": {
						"action": "/product/buy",
						"productId": 5
					}
				}
			}
		}
	]
}
```