using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.SousTypes.Models;

namespace App.SousTypes
{
    public class Converter<T> : JsonConverter<T> where T : class
    {
        private Dictionary<string, Type> typeDeserialize;

        public Converter()
        {
            typeDeserialize = new Dictionary<string, Type>();

            var list = Attribute.GetCustomAttributes(typeof(T)).Select(x => (JsonSousTypes)x);

            foreach (var element in list)
            {
                typeDeserialize.Add(element.Name, element.Type);
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // chargement dans un document json
            var doc = JsonDocument.ParseValue(ref reader);

            // récupération de la propriété "AnimalType"
            if (!doc.RootElement.TryGetProperty("Type", out var typeProp))
                throw new Exception("No Type in json");
            var typeName = typeProp.GetString();

            // désérialisation dans la bonne classe
            return JsonSerializer.Deserialize(doc.RootElement.GetRawText(), typeDeserialize[typeName]) as T;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, typeDeserialize[typeof(T).GetProperty("Type").GetValue(value).ToString()], options);
        }

    }
}
