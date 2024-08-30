using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload.Service
{
    // ElasticSearchService.cs
    public class ElasticSearchService : IDocumentSearchService
    {
        //private readonly IElasticClient _elasticClient;

        public ElasticSearchService()//(IElasticClient elasticClient)
        {
           // _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query)
        {
            return null;
            //var searchResponse = await _elasticClient.SearchAsync<DocumentMetadata>(s => s
            //    .Query(q => q
            //        .QueryString(qs => qs
            //            .Query(query)
            //        )
            //    )
            //);

            //return searchResponse.Documents;
        }

        //public async Task<object> IndexDocument(string documentId, DocumentMetadata metadata)
        //{
        //    return null; //await _elasticClient.IndexDocumentAsync(metadata);
        //}

        public Task DeleteDocumentIndex(string documentId)
        {
            throw new NotImplementedException();
        }

        Task IDocumentSearchService.IndexDocument(string documentId, DocumentMetadata metadata)
        {
            throw new NotImplementedException();
        }
    }

}
