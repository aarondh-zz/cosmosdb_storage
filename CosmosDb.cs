
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public interface ICosmosDb : IDisposable
    {
        Container GetContainer(string database, string containerId);
        Task<Database> GetDatabaseAsync(string database);
    }

    public class CosmosDb : ICosmosDb
    {
        private ICosmosDbConfig config;
        private CosmosClient client;
        public CosmosDb(IOptions<CosmosDbConfig> config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            this.config = config.Value;
            this.client = new CosmosClient(this.config.EndPointUrl, this.config.AuthorizationKey);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.client.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Container GetContainer(string database, string containerId)
        {
            var cosmosDatabase = client.GetDatabase(database);
            return cosmosDatabase.GetContainer(containerId);
        }

        public async Task<Database> GetDatabaseAsync(string databaseId)
        {
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(databaseId);
            return databaseResponse.Database;
        }

        ~CosmosDb()
        {
            Dispose(false);
        }
    }
}