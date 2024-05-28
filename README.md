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
Create the parser by providing the OpenAPI 3.1 parameter specification.
```dotnet
var parser = OpenAPI.ParameterStyleParsers.ParameterParsers.ParameterValueParser.FromOpenApi31ParameterSpecification(
    JsonNode.Parse("""
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
    """)!.AsObject());
```
It's also possible to manually construct the parser.
```dotnet
var parameter = OpenAPI.ParameterStyleParsers.Parameter.Parse(
    name: "color",
    style: "form",
    location: "query",
    explode: true,
    new JsonSchema202012(JsonNode.Parse("""
    {
        "type": "array",
        "items": {
            "type": "string"
    }
    """))
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

### Schema References
[Json pointers represented as URI fragments](https://www.rfc-editor.org/rfc/rfc6901#section-6) are supported, other URI's are currently not. It is possible to bring your own Json Schema implementation though.

# Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

# License
[MIT](LICENSE)