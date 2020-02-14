
namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public interface ICosmosDbCollectionConfig
    {
        string ContainerId { get; }
        string DatabaseId { get; }
    }
    public class CosmosDbCollectionConfig : ICosmosDbCollectionConfig
    {
        public string ContainerId { get; set; }
        public string DatabaseId { get; set; }
    }
}