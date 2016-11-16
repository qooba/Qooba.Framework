using Microsoft.EntityFrameworkCore;
using Qooba.Framework.UnitOfWork.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.UnitOfWork.EntityFramework
{
    public class UnitOfWork<T> : IQueryableUnitOfWork
        where T : DbContext

    {
        public T Context { get; private set; }

        public UnitOfWork(T context)
        {
            Context = context;
        }

        public void Commit()
        {
            AddTrasactionCompletedHandler();
            Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            AddTrasactionCompletedHandler();
            await Context.SaveChangesAsync();
        }

        public void CommitAndRefreshChanges()
        {
            AddTrasactionCompletedHandler();
            var saveFailed = false;
            do
            {
                try
                {
                    Context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    //TODO:
                    //ex.Entries.ToList().ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                    //ex.Entries.ToList().ForEach(entry => entry.ResetToOriginalValue());

                }
            }
            while (saveFailed);
        }

        public async Task CommitAndRefreshChangesAsync()
        {
            AddTrasactionCompletedHandler();
            var saveFailed = false;
            do
            {
                try
                {
                    await Context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    //TODO:
                    //ex.Entries.ToList().ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));
                    //ex.Entries.ToList().ForEach(entry => entry.ResetToOriginalValue());

                }
            }
            while (saveFailed);
        }

        public void RollbackChanges()
        {
            //TODO:
            //Context.ChangeTracker.Entries().Where(x => x.State != EntityState.Added).ToList().ForEach(e => e.Reload());
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters) where T : class
        {
            //TODO:
            //return Context.Database.SqlQuery<T>(sqlQuery, parameters).ToList();
            return Context.Set<T>().FromSql(sqlQuery, parameters);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string sqlQuery, params object[] parameters) where T : class
        {
            //TODO:
            //return Context.Database.SqlQuery<T>(sqlQuery, parameters).ToList();
            return await Task.FromResult(Context.Set<T>().FromSql(sqlQuery, parameters));
        }

        public void ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            Context.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }

        public async Task ExecuteCommandAsync(string sqlCommand, params object[] parameters)
        {
            await Context.Database.ExecuteSqlCommandAsync(sqlCommand, parameters: parameters);
        }

        private void AddTrasactionCompletedHandler()
        {
            //TODO:
            //if(Transaction.Current != null)
            //{
            //    Transaction.Current.TransactionCompleted += Current_TransactionCompleted;
            //}
        }

        //private void Current_TransactionCompleted(object sender, TransactionEventArgs e)
        //{
        //    if (e.Transaction != null)
        //    {
        //        e.Transaction.TransactionCompleted -= Current_TransactionCompleted;
        //        if (e.Transaction.TransactionInformation.Status == TransactionStatus.Aborted)
        //        {
        //            RollbackChanges();
        //        }
        //    }
        //}
    }
}
