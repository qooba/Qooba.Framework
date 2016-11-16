using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.DocumentDb.Abstractions
{
    public interface IDocumentDbRepository<TModel>
    {
        IOrderedQueryable<TModel> All();

        Task<TResult> FirstOrDefault<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query);

        Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate);
        
        Task<TResult> FirstOrDefault<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector);
        
        Task<IList<TResult>> Filtered<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query);

        Task<IList<TModel>> Filtered(Expression<Func<TModel, bool>> predicate);
        
        Task<IList<TResult>> Filtered<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector);
        
        Task Add(TModel model);

        Task Delete(string documentName);
    }
}
