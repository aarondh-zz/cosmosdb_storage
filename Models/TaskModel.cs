
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Com.DaisleyHarrison.CosmosDb.Storage.Utilities;

namespace Com.DaisleyHarrison.CosmosDb.Storage.Models
{
    [JsonConverter(typeof(JsonSubtypeConverter<TaskModel>), Constants.SCHEMA_VERSION_KEY)]
    [JsonSubtype("4", typeof(TaskModelV4))]
    [JsonSubtype("5", typeof(TaskModelV5))]
    [JsonSubtype("6", typeof(TaskModelV6))]
    public class TaskModel : SchemaBase
    {
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastModifiedDate { get; set; }
        public string OwnerUserId { get; set; }
        [JsonProperty("#for")]
        public IList<string> VisibleTo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Purpose { get; set; }
        public string Price { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public IList<string> SampleData { get; set; }
        public int Quantity { get; set; }
        public string DataDetails { get; set; }
        public IList<string> Formats { get; set; }
        public IList<ProviderTypes> ProviderTypesAllowed { get; set; }
        public string LegalRequirements { get; set; }
        public string OtherRequirements { get; set; }
        public string LicenseType { get; set; }
        public string ParticipationAgreement { get; set; }
    }

    public class TaskModelV4 : TaskModel
    {
        public TaskModelV4()
        {
            this.DataDetails = "- old task, no data -";
            this.Formats = new List<string>() { "OLD TASK NO DATA" };
            this.LegalRequirements = "- old task, no data -";
            this.LicenseType = "- old task, no data -";
            this.OtherRequirements = "- old task, no data -";
            this.ParticipationAgreement = "- old task, no data -";
            this.ProviderTypesAllowed = new List<ProviderTypes>() { ProviderTypes.User, ProviderTypes.Group };
            this.Quantity = -1;
        }
    }
    public class TaskModelV5 : TaskModel
    {
        public TaskModelV5()
        {
            this.ParticipationAgreement = "- old task, no data -";
            this.ProviderTypesAllowed = new List<ProviderTypes>() { ProviderTypes.User, ProviderTypes.Group };
        }
    }
    public class TaskModelV6 : TaskModel
    {
        public TaskModelV6()
        {
        }
    }
}