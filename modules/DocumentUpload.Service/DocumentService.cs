using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload.Service
{
    public static class DI
    {

    }
    public interface IDocumentService
    {
        Task<string> StoreDocument(DocumentMetadata metadata, Stream fileStream);
        Task<Stream> GetDocument(string documentId);
        Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query);
    }
    // DocumentService.cs
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentStorageService _documentStorageService;
        private readonly IDocumentSearchService _documentSearchService;
        private readonly bool _useAlfresco;

        public DocumentService(
            IDocumentStorageService documentStorageService,
            IDocumentSearchService documentSearchService,
            IConfiguration configuration)
        {
            _documentStorageService = documentStorageService;
            _documentSearchService = documentSearchService;
            //_useAlfresco = configuration.GetValue<bool>("DocumentStorage:UseAlfresco");
        }

        public async Task<string> StoreDocument(DocumentMetadata metadata, Stream fileStream)
        {
            var documentId = await _documentStorageService.StoreDocument(metadata, fileStream);

            // Optionally, index the document metadata in Elasticsearch
            await _documentSearchService.IndexDocument(documentId, metadata);

            return documentId;
        }

        public async Task<Stream> GetDocument(string documentId)
        {
            return await _documentStorageService.GetDocument(documentId);
        }

        public async Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query)
        {
            return await _documentSearchService.SearchDocuments(query);
        }
    }

}
