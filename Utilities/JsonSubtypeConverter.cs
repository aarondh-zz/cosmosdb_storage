
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Utilities
{
    public class JsonSubtypeConverter<TBase> : JsonConverter
    {
        private string discriminatorField;
        private IList<JsonSubtypeAttribute> subtypes;
        public JsonSubtypeConverter(string discriminatorField)
        {
            this.discriminatorField = discriminatorField;
            this.subtypes = typeof(TBase).GetCustomAttributes(true).ToList().Where(a => a is JsonSubtypeAttribute).Select(a => a as JsonSubtypeAttribute).ToList();
        }
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(TBase));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(TBase))
            {
                JObject jo = JObject.Load(reader);
                string discriminator = jo[this.discriminatorField].ToString();
                var subtype = this.subtypes.FirstOrDefault(s => s.Discriminator == discriminator);
                if (subtype == null)
                {
                    throw new InvalidCastException($"No subtypes defined for discriminator \"{this.discriminatorField}\" = \"{discriminator}\" for base type {typeof(TBase).Name}");
                }
                return jo.ToObject(subtype.Subtype);
            }
            else
            {
                Object result = Activator.CreateInstance(objectType);
                serializer.Populate(reader, result);
                return result;
            }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}