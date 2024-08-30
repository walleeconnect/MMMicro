namespace DocumentUpload.Service
{
    public interface IDocumentSearchService
    {
        Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query);
        Task IndexDocument(string documentId, DocumentMetadata metadata);
        Task DeleteDocumentIndex(string documentId);
    }
}