
using System;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Utilities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class JsonSubtypeAttribute : Attribute
    {
        public Type Subtype { get; set; }
        public string Discriminator { get; set; }
        public JsonSubtypeAttribute()
        {

        }
        public JsonSubtypeAttribute(string discriminator)
        {
            this.Discriminator = discriminator;
        }

        public JsonSubtypeAttribute(string discriminator, Type subtype)
        {
            this.Discriminator = discriminator;
            this.Subtype = subtype;
        }
    }
}