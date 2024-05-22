# OpenAPI.ParameterStyleParsers
[Parameter style](https://spec.openapis.org/oas/v3.1.0#style-values) parsers for OpenAPI 3.1.

The examples in the OpenAPI specification doesn't match [RFC6570](https://www.rfc-editor.org/rfc/rfc6570) fully, in those cases [the examples](https://spec.openapis.org/oas/v3.1.0#style-examples) in the specification are followed.

[![Continuous Delivery](https://github.com/Fresa/OpenAPI.ParameterStyleParsers/actions/workflows/cd.yml/badge.svg)](https://github.com/Fresa/OpenAPI.ParameterStyleParsers/actions/workflows/cd.yml)

## Installation
```Shell
dotnet add package ParameterStyleParsers.OpenAPI
```

https://www.nuget.org/packages/ParameterStyleParsers.OpenAPI/

## Getting Started
Parse the OpenAPI 3.1 parameter from the specification, and create a parameter value parser.
```dotnet
/* Parameter specification example:
{
    "name": "color",
    "in": "query",
    "schema": {
        "type": "array",
        "items": {
            "type": "string"
        }
    },
    "style": "form",
    "explode": true
}
*/

var parameter = OpenAPI.ParameterStyleParsers.Parameter.Parse(
    name: "color",
    style: "form",
    location: "query",
    explode: true,
    JsonSchema.FromText("""
    {
        "type": "array",
        "items": {
            "type": "string"
    }
    """)
);
var parser = OpenAPI.ParameterStyleParsers.ParameterParsers.ParameterValueParser.Create(parameter);
```

### Parse a style serialized parameter
```dotnet
string styleSerializedParameter = "color=blue&color=black&color=brown";
parser.TryParse(styleSerializedParameter, out JsonNode? parameter, out string? error);
Console.WriteLine(parameter.ToJsonString());
// ["blue","black","brown"]
```

### Serialize json to a parameter style.
```dotnet
var json = JsonNode.Parse("""
    ["blue","black","brown"]
""");
string styleSerializedParameter = parser.Serialize(json);
Console.WriteLine(styleSerializedParameter);
// color=blue&color=black&color=brown
``` 

# Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

# License
[MIT](LICENSE)