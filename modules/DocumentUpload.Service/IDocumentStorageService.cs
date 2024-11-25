using Microsoft.AspNetCore.Http;

namespace DocumentUpload.Service
{
    // IDocumentStorageService.cs
    public interface IDocumentStorageService
    {
        Task<FileStoredResponse> StoreDocument(DocumentMetadata metadata, IFormFile fileStream);
        Task<Stream> GetDocument(string documentId);
        Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query);
        Task<MemoryStream> DownloadDocuments(List<Tdhdocument> documentIds);
        Task<bool> DeleteDocument(string filePath);
    }

}
