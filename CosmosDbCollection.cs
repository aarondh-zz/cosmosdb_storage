
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public delegate PartitionKey PartitionKeyProvider<TModel>(TModel model);
    public class CosmosDbCollection<TModel> : IDbCollection<TModel> where TModel : class
    {
        private ICosmosDb cosmosDb;
        private ICosmosDbCollectionConfig config;
        private Database database;
        private Container container;
        private PartitionKeyProvider<TModel> partitionKeyProvider;

        public CosmosDbCollection(ICosmosDb cosmosDb, ICosmosDbCollectionConfig config, PartitionKeyProvider<TModel> partitionKeyProvider = null)
        {
            if (cosmosDb == null)
            {
                throw new ArgumentNullException(nameof(cosmosDb));
            }
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            if (config.DatabaseId == null)
            {
                throw new ArgumentNullException($"Missing configuration for DatabaseId: {this.GetType().Name} ");
            }
            if (config.ContainerId == null)
            {
                throw new ArgumentNullException($"Missing configuration for ContainerId: {this.GetType().Name} ");
            }
            this.cosmosDb = cosmosDb;
            this.config = config;
            this.partitionKeyProvider = partitionKeyProvider;
        }

        public async Task ConnectAsync()
        {
            this.database = await cosmosDb.GetDatabaseAsync(config.DatabaseId);
            this.container = this.database.GetContainer(config.ContainerId);
            this.IsConnected = true;
        }

        protected virtual PartitionKey CreatePartitionKey(TModel model = default)
        {
            if (this.partitionKeyProvider == null)
            {
                return PartitionKey.None;
            }
            return this.partitionKeyProvider(model);
        }
        protected virtual PartitionKey CreatePartitionKey(string partitionKeyValue)
        {
            if (partitionKeyValue == Constants.PARTITION_KEY_NONE)
            {
                return PartitionKey.None;
            }
            else if (partitionKeyValue == Constants.PARTITION_KEY_NULL)
            {
                return PartitionKey.Null;
            }
            return new PartitionKey(partitionKeyValue);
        }

        protected virtual QueryRequestOptions BuildQueryRequestOptions(string partitionKeyValue = null, int maxCount = 0)
        {
            var options = new QueryRequestOptions();
            if (partitionKeyValue != null)
            {
                options.PartitionKey = CreatePartitionKey(partitionKeyValue);
            }
            if (maxCount > 0)
            {
                options.MaxItemCount = maxCount;
            }
            return options;
        }

        public bool IsConnected { get; private set; }

        public async Task<TModel> CreateAsync(TModel model)
        {
            var response = await container.CreateItemAsync<TModel>(model, CreatePartitionKey(model));
            return response.Resource;
        }

        public async Task<TModel> DeleteAsync(string id, string partitionKeyValue = null)
        {
            var response = await container.DeleteItemAsync<TModel>(id, CreatePartitionKey(partitionKeyValue));
            return response.Resource;
        }

        public IOrderedQueryable<TModel> Query(string continuationToken = null, string partitionKeyValue = null, int maxCount = 0)
        {

            return container.GetItemLinqQueryable<TModel>(false, continuationToken, BuildQueryRequestOptions(partitionKeyValue, maxCount));
        }

        private class ListResult : IListResult<TModel>
        {
            public ListResult()
            {
                this.Items = new List<TModel>();
            }
            public string ContinuationToken { get; set; }
            public List<TModel> Items { get; private set; }

            IList<TModel> IListResult<TModel>.Items => this.Items;
        }

        public async Task<IListResult<TModel>> ListAsync(QueryBuilder<TModel> builder = null, string continuationToken = null, string partitionKeyValue = null, int maxCount = 0)
        {
            var query = builder == null ? this.Query(continuationToken, partitionKeyValue, maxCount) : builder(this.Query(continuationToken, partitionKeyValue, maxCount));

            var iterator = query.ToFeedIterator();

            var result = new ListResult();

            while (iterator.HasMoreResults)
            {
                var item = await iterator.ReadNextAsync();
                var profiles = item.Resource;
                result.Items.AddRange(item.Resource);
                result.ContinuationToken = item.ContinuationToken;
            }

            return result;
        }

        public async Task<int> CountAsync(QueryBuilder<TModel> builder = null, string partitionKeyValue = null)
        {
            var requestOptions = new QueryRequestOptions();
            requestOptions.EnableScanInQuery = true;
            string queryText;
            if (builder == null)
            {
                queryText = "SELECT VALUE COUNT(1) FROM root";
            }
            else
            {
                var query = builder(this.Query());
                var queryDefinition = query.ToQueryDefinition();
                queryText = queryDefinition.QueryText.Replace("VALUE root", "VALUE COUNT(1)", StringComparison.OrdinalIgnoreCase);
            }
            var queryIterator = this.container.GetItemQueryIterator<int>(queryText, null, BuildQueryRequestOptions(partitionKeyValue));
            var response = await queryIterator.ReadNextAsync();
            return response.Resource.FirstOrDefault();
        }

        public async Task<TModel> ReadAsync(string id, string partitionKeyValue = null)
        {
            var response = await this.container.ReadItemAsync<TModel>(id, CreatePartitionKey(partitionKeyValue));
            return response.Resource;
        }

        public async Task<TModel> UpsertAsync(TModel model)
        {
            var response = await this.container.UpsertItemAsync<TModel>(model, CreatePartitionKey(model));
            return response.Resource;
        }
    }
};