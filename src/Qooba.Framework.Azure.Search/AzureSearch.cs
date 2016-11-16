using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Qooba.Framework.Azure.Search.Abstractions;
using Qooba.Framework.Configuration.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Search
{
    public class AzureSearch : IAzureSearch
    {
        private readonly IConfig config;

        public AzureSearch(IConfig config)
        {
            this.config = config;
        }

        public async Task UploadDocuments(string indexName, IEnumerable<object> documents)
        {
            var client = this.CreateClient();
            var batch = IndexBatch.Upload(documents);
            var indexClient = client.Indexes.GetClient(indexName);
            await indexClient.Documents.IndexAsync(batch);
        }

        public async Task DeleteIndex(string indexName)
        {
            var client = this.CreateClient();
            if (await client.Indexes.ExistsAsync(indexName))
            {
                await client.Indexes.DeleteAsync(indexName);
            }
        }

        public async Task<IList<TModel>> Search<TModel>(string indexName, string searchText)
            where TModel : class
        {
            var client = this.CreateClient();
            var indexClient = client.Indexes.GetClient(indexName);
            var result = await indexClient.Documents.SearchAsync<TModel>(searchText);
            return result.Results.Select(x => x.Document).ToList();
        }

        private SearchServiceClient CreateClient()
        {
            return new SearchServiceClient(this.config.SearchServiceName, new SearchCredentials(""));
        }
    }
}
