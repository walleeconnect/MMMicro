using DocumentUpload.Service;
using Microsoft.AspNetCore.Http;

namespace DocumentUpload.Infra
{
    // AlfrescoStorageService.cs
    public class AlfrescoStorageService : IDocumentStorageService
    {
        //private readonly CustomApiClient _alfrescoClient; // Assuming you have a client to interact with Alfresco API

        //public AlfrescoStorageService(CustomApiClient alfrescoClient)
        //{
        //    _alfrescoClient = alfrescoClient;
        //}

        public async Task<string> StoreDocument(DocumentMetadata metadata, Stream fileStream)
        {
            // Call Alfresco API to store document
            //return await _alfrescoClient.UploadDocument(metadata, fileStream);
            return null;
        }

        public async Task<Stream> GetDocument(string documentId)
        {
            // Call Alfresco API to get document
            // return await _alfrescoClient.DownloadDocument(documentId);
            return null;
        }

        public async Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query)
        {
            // Handle search via Alfresco's own search capabilities or use Elasticsearch
            //return await _alfrescoClient.SearchDocuments(query);
            return null;
        }

        public Task<string> StoreDocument(DocumentMetadata metadata, IFormFile fileStream)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> DownloadDocuments(List<string> documentIds)
        {
            throw new NotImplementedException();
        }


        public Task<MemoryStream> DownloadDocuments(List<Tdhdocument> documentIds)
        {
            throw new NotImplementedException();
        }

        Task<FileStoredResponse> IDocumentStorageService.StoreDocument(DocumentMetadata metadata, IFormFile fileStream)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDocument(string filePath)
        {
            throw new NotImplementedException();
        }
    }

}
