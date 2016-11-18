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
using Qooba.Framework.UnitOfWork.Abstractions;
using Qooba.Framework.Specification.Abstractions;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureBlobTableRepository<TModel> : IAzureBlobTableRepository<TModel>, IRepositoryCommands<TModel>, IRepositoryQueries<TModel>
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

        public IUnitOfWork UnitOfWork
        {
            get
            {
                throw new NotImplementedException();
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

        public async Task<TModel> AddAndCommitAsync(TModel entity)
        {
            await this.Add(entity);
            return entity;
        }

        public async Task<TModel> UpdateAndCommitAsync(TModel entity)
        {
            await this.AddOrReplace(entity);
            return entity;
        }

        public async Task RemoveAndCommitAsync(TModel entity)
        {
            await this.Delete(entity);
        }

        public async Task<TModel> MergeAndCommitAsync(TModel entity)
        {
            await this.AddOrMerge(entity);
            return entity;
        }

        public async Task AddAndCommitMultipleAsync(IList<TModel> entities)
        {
            await Task.WhenAll(entities.Select(e => this.Add(e)));
        }

        public async Task UpdateAndCommitMultipleAsync(IList<TModel> entities)
        {
            await Task.WhenAll(entities.Select(e => this.AddOrReplace(e)));
        }

        public async Task RemoveAndCommitMultipleAsync(IList<TModel> entities)
        {
            await Task.WhenAll(entities.Select(e => this.Delete(e)));
        }

        public async Task MergeAndCommitMultipleAsync(IList<TModel> entities)
        {
            await Task.WhenAll(entities.Select(e => this.AddOrMerge(e)));
        }

        public async Task<IList<TModel>> AllAsync()
        {
            return await this.All();
        }

        public async Task<IList<TResult>> AllAsync<TResult>(Expression<Func<TModel, TResult>> selector) where TResult : class
        {
            return (await Filtered(x => x.Select(selector)));
        }

        public async Task<bool> AnyAsync()
        {
            return (await Filtered(x => x.Select(e => e.RowKey))).Any();
        }

        public async Task<bool> AnyAsync(ISpecification<TModel> specification)
        {
            return (await Filtered(x => x.Where(specification.IsSatisfiedBy()).Select(e => e.RowKey))).Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TModel, bool>> condition)
        {
            return (await Filtered(x => x.Where(condition).Select(e => e.RowKey))).Any();
        }

        public async Task<int> CountAsync()
        {
            return (await Filtered(x => x.Select(e => e.RowKey))).Count();
        }

        public async Task<int> CountAsync(ISpecification<TModel> specification)
        {
            return (await Filtered(x => x.Where(specification.IsSatisfiedBy()).Select(e => e.RowKey))).Count();
        }

        public async Task<int> CountAsync(Expression<Func<TModel, bool>> condition)
        {
            return (await Filtered(x => x.Where(condition).Select(e => e.RowKey))).Count();
        }

        public async Task<TModel> SingleAsync(ISpecification<TModel> specification)
        {
            return (await Filtered(x => x.Where(specification.IsSatisfiedBy()))).Single();
        }

        public async Task<TModel> SingleAsync(Expression<Func<TModel, bool>> condition)
        {
            return (await Filtered(x => x.Where(condition))).Single();
        }

        public async Task<TModel> SingleOrDefaultAsync(ISpecification<TModel> specification)
        {
            return (await Filtered(x => x.Where(specification.IsSatisfiedBy()))).SingleOrDefault();
        }

        public async Task<TModel> SingleOrDefaultAsync(Expression<Func<TModel, bool>> condition)
        {
            return (await Filtered(x => x.Where(condition))).SingleOrDefault();
        }

        public async Task<IList<TModel>> FilterAsync(ISpecification<TModel> specification)
        {
            return await Filtered(x => x.Where(specification.IsSatisfiedBy()));
        }

        public async Task<IList<TModel>> FilterAsync(Expression<Func<TModel, bool>> condition)
        {
            return await Filtered(x => x.Where(condition));
        }


        public async Task<IList<TResult>> FilterAsync<TResult>(ISpecification<TModel> specification, Expression<Func<TModel, TResult>> selector)
            where TResult : class
        {
            return await Filtered(x => x.Where(specification.IsSatisfiedBy()).Select(selector));
        }

        public async Task<IList<TResult>> FilterAsync<TResult>(Expression<Func<TModel, bool>> condition, Expression<Func<TModel, TResult>> selector)
            where TResult : class
        {
            return await Filtered(x => x.Where(condition).Select(selector));
        }
    }
}
