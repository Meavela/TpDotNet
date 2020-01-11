using System.Collections.Generic;
using System.Text.Json;

namespace App.SousTypes
{
    public class JsonRepo<T> where T : class
    {
        public List<T> Read(string data)
        {
            var jsonSerializeOpt = new JsonSerializerOptions();
            jsonSerializeOpt.Converters.Add(new Converter<T>());
            return JsonSerializer.Deserialize<List<T>>(data, jsonSerializeOpt);
        }

        public string Write(IEnumerable<T> data)
        {
            var jsonSerializeOpt = new JsonSerializerOptions();
            jsonSerializeOpt.Converters.Add(new Converter<T>());
            return JsonSerializer.Serialize(data, jsonSerializeOpt);
        }
    }
}
