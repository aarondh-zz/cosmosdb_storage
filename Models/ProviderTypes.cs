
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProviderTypes
    {
        [EnumMember(Value = "UNKNOWN")]
        Unknown = 0,

        [EnumMember(Value = "AUTO")]
        Auto = 1,

        [EnumMember(Value = "USER")]
        User = 2,

        [EnumMember(Value = "GROUP")]
        Group = 3
    }
}