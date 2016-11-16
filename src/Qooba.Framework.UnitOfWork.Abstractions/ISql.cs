using System.Collections.Generic;

namespace Qooba.Framework.UnitOfWork.Abstractions
{
    public interface ISql
    {
        IEnumerable<T> ExecuteQuery<T>(string sqlQuery, params object[] parameters) where T : class;

        void ExecuteCommand(string sqlCommand, params object[] parameters);
    }
}
