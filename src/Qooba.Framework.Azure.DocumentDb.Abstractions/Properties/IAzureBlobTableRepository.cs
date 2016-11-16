using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Storage.Abstractions
{
    public interface IAzureBlobTableRepository<TModel>
    {
        Task<IList<TModel>> All();

        Task<TResult> FirstOrDefault<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query);

        Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate);

        Task<TModel> FirstOrDefault(string partitionKey, string rowKey);

        Task<TModel> FirstOrDefault(string partitionKey, Func<TModel, bool> predicate);

        Task<TResult> FirstOrDefault<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector);

        Task<TResult> FirstOrDefault<TResult>(string partitionKey, string rowKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task<TResult> FirstOrDefault<TResult>(string partitionKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task<IList<TResult>> Filtered<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query);

        Task<IList<TModel>> Filtered(Expression<Func<TModel, bool>> predicate);

        Task<IList<TModel>> Filtered(string partitionKey, string rowKey, Func<TModel, bool> predicate);

        Task<IList<TModel>> Filtered(string partitionKey, Func<TModel, bool> predicate);

        Task<IList<TResult>> Filtered<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector);

        Task<IList<TResult>> Filtered<TResult>(string partitionKey, string rowKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task<IList<TResult>> Filtered<TResult>(string partitionKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task Add(TModel model);

        Task AddOrMerge(TModel model);

        Task AddOrReplace(TModel model);

        Task Delete(TModel model);
    }
}
