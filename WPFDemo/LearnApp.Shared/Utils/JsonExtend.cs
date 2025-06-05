using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace LearnApp.Shared.Utils
{
    public static class JsonExtend
    {
        public static string J2String<T>(T obj)
        {
            if (obj == null)
                return string.Empty;

            var jsonOption = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            jsonOption.Converters.Add(new DatetimeJsonConverter());

            string str = JsonSerializer.Serialize<T>(obj, jsonOption);

            return str;

        }

        public static T ToJson<T>(this string jsonText)
        {
            var jsonOption = new JsonSerializerOptions();
            jsonOption.PropertyNameCaseInsensitive = true;
            jsonOption.Converters.Add(new DatetimeJsonConverter());
            return JsonSerializer.Deserialize<T>(jsonText, jsonOption);
        }

        public class DatetimeJsonConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    if (DateTime.TryParse(reader.GetString(), out DateTime date))
                        return date;
                }
                return reader.GetDateTime();
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));

            }
        }
    }
}
