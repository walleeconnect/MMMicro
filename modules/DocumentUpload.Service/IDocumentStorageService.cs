namespace DocumentUpload.Service
{
    // IDocumentStorageService.cs
    public interface IDocumentStorageService
    {
        Task<string> StoreDocument(DocumentMetadata metadata, Stream fileStream);
        Task<Stream> GetDocument(string documentId);
        Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query);
    }

}
