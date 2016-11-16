using System;
using System.Threading.Tasks;

namespace Qooba.Framework.UnitOfWork.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        Task CommitAsync();

        void CommitAndRefreshChanges();

        void RollbackChanges();
    }
}
