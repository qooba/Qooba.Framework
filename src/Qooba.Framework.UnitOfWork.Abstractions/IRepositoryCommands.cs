using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.UnitOfWork.Abstractions
{

    public interface IRepositoryCommands<T> : IRepository where T : class
    {
        Task<T> AddAndCommitAsync(T entity);
        
        Task<T> UpdateAndCommitAsync(T entity);
        
        Task RemoveAndCommitAsync(T entity);

        Task<T> MergeAndCommitAsync(T entity);

        Task AddAndCommitMultipleAsync(IList<T> entities);

        Task UpdateAndCommitMultipleAsync(IList<T> entities);

        Task RemoveAndCommitMultipleAsync(IList<T> entities);

        Task MergeAndCommitMultipleAsync(IList<T> entities);
    }
}
