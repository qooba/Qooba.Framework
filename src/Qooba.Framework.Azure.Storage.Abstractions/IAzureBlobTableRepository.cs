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

        Task<IList<TResult>> Filtered<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query);

#if NET461

        Task<TResult> FirstOrDefault<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query);

        Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate);

        Task<TModel> FirstOrDefault(string partitionKey, string rowKey);

        Task<TModel> FirstOrDefault(string partitionKey, Func<TModel, bool> predicate);

        Task<TResult> FirstOrDefault<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector);

        Task<TResult> FirstOrDefault<TResult>(string partitionKey, string rowKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task<TResult> FirstOrDefault<TResult>(string partitionKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task<IList<TModel>> Filtered(Expression<Func<TModel, bool>> predicate);

        Task<IList<TModel>> Filtered(string partitionKey, string rowKey, Func<TModel, bool> predicate);

        Task<IList<TModel>> Filtered(string partitionKey, Func<TModel, bool> predicate);

        Task<IList<TResult>> Filtered<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector);

        Task<IList<TResult>> Filtered<TResult>(string partitionKey, string rowKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);

        Task<IList<TResult>> Filtered<TResult>(string partitionKey, Func<TModel, bool> predicate, Expression<Func<TModel, TResult>> selector);
#endif

        Task Add(IList<TModel> model);

        Task AddOrMerge(IList<TModel> model);

        Task AddOrReplace(IList<TModel> model);

        Task Add(TModel model);

        Task AddOrMerge(TModel model);

        Task AddOrReplace(TModel model);

        Task Delete(TModel model);
    }
}
