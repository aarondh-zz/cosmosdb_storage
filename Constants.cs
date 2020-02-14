
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public class Constants
    {
        public const string PARTITION_KEY_NONE = "::none::";
        public const string PARTITION_KEY_NULL = "::null::";
    }
}