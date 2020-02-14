using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Com.DaisleyHarrison.CosmosDb.Storage.Models;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public static class StorageLayerExtensions
    {
        public static void AddStorageLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosDbConfig>(configuration.GetSection("Storage:CosmosDb"));

            services.Configure<CosmosDbCollectionConfig>("AuditTrail", configuration.GetSection("Storage:CosmosDb:Collection:AuditTrail"));

            services.Configure<CosmosDbCollectionConfig>("Profile", configuration.GetSection("Storage:CosmosDb:Collection:Profile"));

            services.Configure<CosmosDbCollectionConfig>("TaskModel", configuration.GetSection("Storage:CosmosDb:Collection:TaskModel"));

            services.AddSingleton<ICosmosDb, CosmosDb>();

            services.AddScoped<IDbCollection<AuditTrail>, CosmosDbCollection<AuditTrail>>(provider =>
            {
                var cosmosDb = provider.GetService<ICosmosDb>();
                var config = provider.GetService<IOptionsSnapshot<CosmosDbCollectionConfig>>().Get("AuditTrail");
                return new CosmosDbCollection<AuditTrail>(cosmosDb, config, model =>
                {
                    return new PartitionKey(model.UserId);
                });

            });

            services.AddScoped<IDbCollection<Profile>, CosmosDbCollection<Profile>>(provider =>
            {
                var cosmosDb = provider.GetService<ICosmosDb>();
                var config = provider.GetService<IOptionsSnapshot<CosmosDbCollectionConfig>>().Get("Profile");
                return new CosmosDbCollection<Profile>(cosmosDb, config, model =>
                {
                    return new PartitionKey(model.Name);
                });

            });

            services.AddScoped<IDbCollection<TaskModel>, CosmosDbCollection<TaskModel>>(provider =>
            {
                var cosmosDb = provider.GetService<ICosmosDb>();
                var config = provider.GetService<IOptionsSnapshot<CosmosDbCollectionConfig>>().Get("TaskModel");
                return new CosmosDbCollection<TaskModel>(cosmosDb, config, model =>
                {
                    return new PartitionKey(model.Title);
                });

            });
        }
    }
}