
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.DaisleyHarrison.CosmosDb.Storage
{
    public interface IListResult<TModel> where TModel : class
    {
        string ContinuationToken { get; }
        IList<TModel> Items { get; }
    }
}