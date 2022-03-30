﻿using Common.DTOs;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace API {
    /// <summary>
    /// Custom the Json converter for System.Text.Json, to avoid the inherit types can't be serialized
    /// </summary>
    /// <typeparam name="T">The generic types to add</typeparam>
    public class CustomGenericConverter<T> : JsonConverter<T> where T : IDTO {

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var jsonObject = JsonSerializer.Deserialize(ref reader, typeToConvert, options);
            if (jsonObject is not null) {
                return (T)jsonObject;
            } else {
                return default(T);
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
            JsonSerializer.Serialize(writer, Convert.ChangeType(value, value.GetType()), options);
        }
    }

    /// <summary>
    /// The extension method to add IDTO inheritances
    /// Has some problems in types reflection and instanciate, need to reconstruct
    /// </summary>
    public static class JsonConverterExtensions {
        public static void AddDTOConverters(this ICollection<JsonConverter> converters) {
            var types = Assembly.GetAssembly(typeof(IDTO))?.GetTypes()
                .Where(t => typeof(IDTO).IsAssignableFrom(t) && t.IsClass).ToList();
            types?.ForEach(t => {
                var makeGeneric = typeof(CustomGenericConverter<>).MakeGenericType(t);
                var instance = Activator.CreateInstance(makeGeneric);
                if (instance is not null) {
                    converters.Add((JsonConverter)instance);
                }
            });
        }
    }
}