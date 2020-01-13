//https://stackoverflow.com/questions/17369278/convert-long-number-as-string-in-the-serialization
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebFrontend.Helpers
{
    public class Int64ToStringConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jt = JValue.ReadFrom(reader);

            return jt.Value<long>();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Int64) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }
    }
}