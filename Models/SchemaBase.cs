
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Com.DaisleyHarrison.CosmosDb.Storage.Utilities;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Models
{
    public class SchemaBase
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "_etag")]
        public string ETag { get; set; }
        [JsonProperty(Constants.SCHEMA_NAME_KEY)]
        public string SchemaName { get; set; }
        [JsonProperty(Constants.SCHEMA_VERSION_KEY)]
        public string SchemaVersion { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}