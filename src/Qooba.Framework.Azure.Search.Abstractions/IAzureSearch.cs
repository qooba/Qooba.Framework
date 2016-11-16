using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Search.Abstractions
{
    public interface IAzureSearch
    {
        Task UploadDocuments(string indexName, IEnumerable<object> documents);

        Task DeleteIndex(string indexName);

        Task<IList<TModel>> Search<TModel>(string indexName, string searchText)
            where TModel : class;
    }
}
