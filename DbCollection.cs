
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public delegate IQueryable<TModel> QueryBuilder<TModel>(IOrderedQueryable<TModel> query);
    public interface IDbCollection<TModel> where TModel : class
    {
        bool IsConnected { get; }
        Task ConnectAsync();
        Task<TModel> CreateAsync(TModel model);
        Task<TModel> ReadAsync(string id, string partitionKey = null);
        Task<TModel> DeleteAsync(string id, string partitionKey = null);
        Task<TModel> UpsertAsync(TModel model);
        IOrderedQueryable<TModel> Query(string continuationToken = null, string partitionKeyValue = null, int maxCount = 0);
        Task<IListResult<TModel>> ListAsync(QueryBuilder<TModel> builder = null, string continuationToken = null, string partitionKeyValue = null, int maxCount = 0);
        Task<int> CountAsync(QueryBuilder<TModel> builder = null, string partitionKeyValue = null);
    }
}