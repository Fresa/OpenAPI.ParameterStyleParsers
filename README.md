# OpenAPI.ParameterStyleParsers
Parameter style parsers for OpenAPI.

The examples in the OpenAPI specification doesn't match [RFC6570](https://www.rfc-editor.org/rfc/rfc6570) fully, in those cases [the examples](https://spec.openapis.org/oas/v3.1.0#style-examples) in the specifications are followed.

[![Continuous Delivery](https://github.com/Fresa/OpenAPI.ParameterStyleParsers/actions/workflows/cd.yml/badge.svg)](https://github.com/Fresa/OpenAPI.ParameterStyleParsers/actions/workflows/cd.yml)

## Installation
```Shell
dotnet add package ParameterStyleParsers.OpenAPI
```

https://www.nuget.org/packages/ParameterStyleParsers.OpenAPI/

## Getting Started
Create a parser by providing the OpenAPI parameter specification.

### [OpenAPI 3.2](https://spec.openapis.org/oas/v3.2.0.html#parameter-object)
```dotnet
var parser = OpenAPI.ParameterStyleParsers.ParameterValueParserFactory.OpenApi32(
    """
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
    """);
```

### [OpenAPI 3.1](https://spec.openapis.org/oas/v3.1.0.html#parameter-object)
```dotnet
var parser = OpenAPI.ParameterStyleParsers.ParameterValueParserFactory.OpenApi31(
    """
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
    """);
```

### [OpenAPI 2.0](https://spec.openapis.org/oas/v2.0.html#parameter-object)
```dotnet
var parser = OpenAPI.ParameterStyleParsers.ParameterValueParserFactory.OpenApi20(
    """
    {
        "name": "color",
        "in": "query",
        "required": false,
        "type": "array",
        "items": {
            "type": "string"
        },
        "collectionFormat": "multi"
    }
    """);
```


### Parse a style serialized parameter
```dotnet
string styleSerializedParameter = "color=blue&color=black&color=brown";
Console.WriteLine(
    parser.TryParse(styleSerializedParameter, out JsonNode? json, out string? error)
        ? json?.ToJsonString()
        : error);
// ["blue","black","brown"]
```

### Serialize json to a parameter style.
```dotnet
var json = JsonNode.Parse("""
    ["blue","black","brown"]
""");
var styleSerializedParameter = parser.Serialize(json);
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