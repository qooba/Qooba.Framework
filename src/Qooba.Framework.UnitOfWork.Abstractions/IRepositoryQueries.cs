using Qooba.Framework.Specification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qooba.Framework.UnitOfWork.Abstractions
{
    public interface IRepositoryQueries<T> : IRepository where T : class
    {
        Task<IList<T>> AllAsync();
        
        Task<bool> AnyAsync();

        Task<bool> AnyAsync(ISpecification<T> specification);

        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);

        Task<int> CountAsync();

        Task<int> CountAsync(ISpecification<T> specification);

        Task<int> CountAsync(Expression<Func<T, bool>> condition);

        Task<T> SingleAsync(ISpecification<T> specification);
        
        Task<T> SingleAsync(Expression<Func<T, bool>> condition);
        
        Task<T> SingleOrDefaultAsync(ISpecification<T> specification);
        
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> condition);

        Task<IList<T>> FilterAsync(ISpecification<T> specification);

        Task<IList<TResult>> FilterAsync<TResult>(ISpecification<T> specification, Expression<Func<T, TResult>> selector) where TResult : class;

        Task<IList<T>> FilterAsync(Expression<Func<T, bool>> condition);

        Task<IList<TResult>> FilterAsync<TResult>(Expression<Func<T, bool>> condition, Expression<Func<T, TResult>> selector) where TResult : class;
    }
}
