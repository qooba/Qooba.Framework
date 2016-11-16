using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Qooba.Framework.Configuration.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Qooba.Framework.Azure.Storage.Abstractions;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureBlobTableRepository<TModel> : IAzureBlobTableRepository<TModel>
        where TModel : class, ITableEntity, new()
    {
        private readonly IConfig config;

        public AzureBlobTableRepository(IConfig config)
        {
            this.config = config;
        }

        protected virtual string TableName
        {
            get
            {
                return typeof(TModel).Name;
            }
        }

        public async Task<IList<TModel>> All()
        {
            return await ExecuteAsync(this.Set());
        }

        public async Task<IList<TResult>> Filtered<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query)
        {
            return await ExecuteAsync(query(this.Set()).AsTableQuery());
        }

#if NET461

        public async Task<TResult> FirstOrDefault<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query)
        {
            return (await this.Filtered(query)).FirstOrDefault();
        }

        public async Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate)
        {
            return (await Filtered(x => x.Where(predicate))).FirstOrDefault();
        }

        public async Task<TModel> FirstOrDefault(string partitionKey, string rowKey)
        {
            TableOperation insertOperation = TableOperation.Retrieve(partitionKey, rowKey);
            var result = await this.PrepareTable().ExecuteAsync(insertOperation);
            return (TModel)result.Result;
        }

        public async Task<TModel> FirstOrDefault(string partitionKey, Func<TModel, bool> predicate)
        {
            return (await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && predicate(m)))).FirstOrDefault();
        }

        public async Task<TResult> FirstOrDefault<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return (await Filtered(x => x.Where(predicate).Select(selector))).FirstOrDefault();
        }

        public async Task<TResult> FirstOrDefault<TResult>(string partitionKey, string rowKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return (await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && m.RowKey == rowKey && predicate(m)).Select(selector))).FirstOrDefault();
        }

        public async Task<TResult> FirstOrDefault<TResult>(string partitionKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return (await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && predicate(m)).Select(selector))).FirstOrDefault();
        }
        
        public async Task<IList<TModel>> Filtered(Expression<Func<TModel, bool>> predicate)
        {
            return await Filtered(x => x.Where(predicate));
        }

        public async Task<IList<TModel>> Filtered(string partitionKey, string rowKey, Func<TModel, bool> predicate)
        {
            return await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && m.RowKey == rowKey && predicate(m)));
        }

        public async Task<IList<TModel>> Filtered(string partitionKey, Func<TModel, bool> predicate)
        {
            return await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && predicate(m)));
        }

        public async Task<IList<TResult>> Filtered<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return await Filtered(x => x.Where(predicate).Select(selector));
        }

        public async Task<IList<TResult>> Filtered<TResult>(string partitionKey, string rowKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && m.RowKey == rowKey && predicate(m)).Select(selector));
        }

        public async Task<IList<TResult>> Filtered<TResult>(string partitionKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return await Filtered(x => x.Where(m => m.PartitionKey == partitionKey && predicate(m)).Select(selector));
        }

#endif

        public async Task Add(IList<TModel> model)
        {
            TableBatchOperation insertOperation = new TableBatchOperation();
            model.ToList().ForEach(x => insertOperation.Insert(x));
            await this.PrepareTable().ExecuteBatchAsync(insertOperation);
        }

        public async Task AddOrMerge(IList<TModel> model)
        {
            TableBatchOperation insertOperation = new TableBatchOperation();
            model.ToList().ForEach(x => insertOperation.InsertOrMerge(x));
            await this.PrepareTable().ExecuteBatchAsync(insertOperation);
        }

        public async Task AddOrReplace(IList<TModel> model)
        {
            TableBatchOperation insertOperation = new TableBatchOperation();
            model.ToList().ForEach(x => insertOperation.InsertOrReplace(x));
            await this.PrepareTable().ExecuteBatchAsync(insertOperation);
        }

        public async Task Add(TModel model)
        {
            TableOperation insertOperation = TableOperation.Insert(model);
            await this.PrepareTable().ExecuteAsync(insertOperation);
        }

        public async Task AddOrMerge(TModel model)
        {
            TableOperation insertOperation = TableOperation.InsertOrMerge(model);
            await this.PrepareTable().ExecuteAsync(insertOperation);
        }

        public async Task AddOrReplace(TModel model)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(model);
            await this.PrepareTable().ExecuteAsync(insertOperation);
        }

        public async Task Delete(TModel model)
        {
            TableOperation deleteOperation = TableOperation.Delete(model);
            await this.PrepareTable().ExecuteAsync(deleteOperation);
        }

        protected TableQuery<TModel> Set()
        {
            return PrepareTable().CreateQuery<TModel>();
        }

        private async Task<IList<TResult>> ExecuteAsync<TResult>(TableQuery<TResult> query)
        {
            TableContinuationToken continuationToken = null;
            IList<TResult> results = new List<TResult>();
            do
            {
                var res = await query.ExecuteSegmentedAsync(continuationToken);
                foreach (var item in res)
                {
                    results.Add(item);
                }

                continuationToken = res.ContinuationToken;
            } while (continuationToken != null);

            return results;
        }

        private CloudTable PrepareTable()
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.StorageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(this.TableName);
            return table;
        }
    }
}
