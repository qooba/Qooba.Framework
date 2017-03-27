using Microsoft.Azure.Documents.Client;
using Qooba.Framework.Configuration.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.DocumentDb.Abstractions
{
    public class DocumentDbRepository<TModel> : IDocumentDbRepository<TModel>
    {
        private IDocumentDbConfig config;

        private static string documentDbUri;

        private static string documentDbPrimaryKey;

        private string documentDbDatabaseName;

        private string documentDbCollectionName;

        private Uri documentCollectionUri;


        private static Lazy<DocumentClient> client = new Lazy<DocumentClient>(() => new DocumentClient(new Uri(documentDbUri), documentDbPrimaryKey));

        public DocumentDbRepository(IDocumentDbConfig config)
        {
            this.config = config;
            documentDbUri = this.config.DocumentDbUri;
            documentDbPrimaryKey = this.config.DocumentDbPrimaryKey;
            this.documentDbDatabaseName = this.config.DocumentDbDatabaseName;
            this.documentDbCollectionName = this.config.DocumentDbCollectionName;
            this.documentCollectionUri = UriFactory.CreateDocumentCollectionUri(this.documentDbDatabaseName, this.documentDbCollectionName);
        }

        public async Task Add(TModel model)
        {
            await client.Value.CreateDocumentAsync(this.documentCollectionUri, model);
        }
        
        public IOrderedQueryable<TModel> All()
        {
            return client.Value.CreateDocumentQuery<TModel>(this.documentCollectionUri);
        }

        public async Task Delete(string documentName)
        {
            await client.Value.DeleteDocumentAsync(UriFactory.CreateDocumentUri(this.documentDbDatabaseName, this.documentDbCollectionName, documentName));
        }

        public async Task<IList<TModel>> Filtered(Expression<Func<TModel, bool>> predicate)
        {
            return this.All().Where(predicate).ToList();
        }

        public async Task<IList<TResult>> Filtered<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query)
        {
            return query(this.All()).ToList();
        }

        public async Task<IList<TResult>> Filtered<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return this.All().Where(predicate).Select(selector).ToList();
        }

        public async Task<TModel> FirstOrDefault(Expression<Func<TModel, bool>> predicate)
        {
            return this.All().FirstOrDefault(predicate);
        }

        public async Task<TResult> FirstOrDefault<TResult>(Func<IQueryable<TModel>, IQueryable<TResult>> query)
        {
            return query(this.All()).FirstOrDefault();
        }

        public async Task<TResult> FirstOrDefault<TResult>(Expression<Func<TModel, bool>> predicate, Expression<Func<TModel, TResult>> selector)
        {
            return this.All().Where(predicate).Select(selector).FirstOrDefault();
        }
    }
}
