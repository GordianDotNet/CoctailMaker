using System.Text.Json;

namespace CoctailMakerApp.Lib
{
    public static class JsonHelper
    {
        private static JsonSerializerOptions _deserializeOptions = new JsonSerializerOptions
        {
        };

        private static JsonSerializerOptions _serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
        };

        private static JsonSerializerOptions _serializeOptionsIndented = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        public static T Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, _deserializeOptions);
        }

        public static string Serialize<T>(T instance, bool prettyPrint = true)
        {
            return System.Text.Json.JsonSerializer.Serialize(instance, prettyPrint ? _serializeOptionsIndented : _serializeOptions);
        }
    }
}
