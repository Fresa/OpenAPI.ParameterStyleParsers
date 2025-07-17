using System.Text.Json.Nodes;

namespace OpenAPI.ParameterStyleParsers.Json;

internal static class JsonObjectExtensions
{
    internal static JsonObjectCopyContext CopyFrom(this JsonObject destination, JsonObject source)
    {
        return new JsonObjectCopyContext(source, destination);
    }
    
    internal sealed class JsonObjectCopyContext(JsonObject source, JsonObject destination)
    {
        private readonly JsonObject _destination = destination;

        internal JsonObjectCopyContext Property(string name, Func<JsonObject, JsonObject>? propertyFilter = null)
        {
            if (!source.TryGetPropertyValue(name, out var value))
            {
                return this;
            }

            value = value?.DeepClone();
            _destination.Add(
                name,
                propertyFilter != null && value != null
                    ? propertyFilter(value.AsObject())
                    : value);

            return this;
        }
        
        internal JsonObjectCopyContext PropertiesStartingWith(string pattern)
        {
            foreach (var property in source.Where(pair =>
                         pair.Key.StartsWith(pattern)))
            {
                _destination.Add(property.Key, property.Value?.DeepClone());
            }

            return this;
        }
        
        public static implicit operator JsonObject(JsonObjectCopyContext context) => context._destination;
    }
}