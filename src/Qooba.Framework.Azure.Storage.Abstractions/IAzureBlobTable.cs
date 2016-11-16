using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Storage.Abstractions
{
    public interface IAzureBlobTable
    {
        Task CreateTable(string tableName);

        Task DeleteTable(string tableName);
    }
}
