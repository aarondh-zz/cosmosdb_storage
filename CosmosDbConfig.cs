
namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public interface ICosmosDbConfig
    {
        string EndPointUrl { get; }
        string AuthorizationKey { get; }
    }
    public class CosmosDbConfig : ICosmosDbConfig
    {
        public string EndPointUrl { get; set; }
        public string AuthorizationKey { get; set; }
    }
}