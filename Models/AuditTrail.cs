using System;
using Newtonsoft.Json;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Models
{
    public class AuditTrail
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public ActionType ActionType { get; set; }
        public string ActionData { get; set; }
        public string UserId { get; set; }
        public string ActivityId { get; set; }
        public string RequestId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}