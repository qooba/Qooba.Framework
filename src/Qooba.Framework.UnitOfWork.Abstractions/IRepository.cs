using Qooba.Framework.Specification.Abstractions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qooba.Framework.UnitOfWork.Abstractions
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }

    public interface IRepository<T> : IRepository where T : class
    {
        IQueryable<T> All();

        bool Any();

        Task<bool> AnyAsync();

        int Count();

        Task<int> CountAsync();

        void Add(T entity);

        void AddOrUpdate(T entity);

        void Update(T entity);

        void TrackItem(T entity);

        void Remove(T entity);

        void Merge(T persisted, T currents);

        void Clear();

        IQueryable<T> Filter(ISpecification<T> specification);

        IQueryable<T> Filter(Expression<Func<T, bool>> condition);

        T Single(ISpecification<T> specification);

        Task<T> SingleAsync(ISpecification<T> specification);

        T Single(Expression<Func<T, bool>> condition);

        Task<T> SingleAsync(Expression<Func<T, bool>> condition);

        T SingleOrDefault(ISpecification<T> specification);

        Task<T> SingleOrDefaultAsync(ISpecification<T> specification);

        T SingleOrDefault(Expression<Func<T, bool>> condition);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> condition);

        bool Any(ISpecification<T> specification);

        Task<bool> AnyAsync(ISpecification<T> specification);

        bool Any(Expression<Func<T, bool>> condition);

        Task<bool> AnyAsync(Expression<Func<T, bool>> condition);

        int Count(ISpecification<T> specification);

        Task<int> CountAsync(ISpecification<T> specification);

        int Count(Expression<Func<T, bool>> condition);

        Task<int> CountAsync(Expression<Func<T, bool>> condition);
    }
}
