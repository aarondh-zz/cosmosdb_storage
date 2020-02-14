
using System;
using Newtonsoft.Json;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Models
{
    public class Profile : SchemaBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public int ReviewsReceived { get; set; }
        public double AvgRating { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}