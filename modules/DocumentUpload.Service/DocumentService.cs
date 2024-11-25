using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload.Service
{
    public class FileStoredResponse
    {
        public string DocumentId { get; set; }
        public string FilePath { get; set; }
    }
    public static class DI
    {

    }
    public interface IDocumentService
    {
        Task<string> StoreDocument(DocumentMetadata metadata, IFormFile fileStream);
        Task<Stream> GetDocument(string documentId);
        Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query);
        Task<MemoryStream> DownloadDocuments(List<string> documentIds);
    }
    // DocumentService.cs
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentStorageService _documentStorageService;
        private readonly IDocumentSearchService _documentSearchService;
        private readonly bool _useAlfresco;
        private MyDbContext myDbContext;
        public DocumentService(
            IDocumentStorageService documentStorageService,
            IDocumentSearchService documentSearchService,
            IConfiguration configuration)
        {
            _documentStorageService = documentStorageService;
            _documentSearchService = documentSearchService;
            myDbContext = new MyDbContext();
            //_useAlfresco = configuration.GetValue<bool>("DocumentStorage:UseAlfresco");
        }

        public async Task<string> StoreDocument(DocumentMetadata metadata, IFormFile fileStream)
        {
            using (var transaction = await myDbContext.Database.BeginTransactionAsync())
            {
                FileStoredResponse fileStoredResponse = null;
                try
                {
                    
                    if (fileStream is null)
                    {
                        throw new ArgumentNullException(nameof(fileStream));
                    }
                    fileStoredResponse = await _documentStorageService.StoreDocument(metadata, fileStream);
                    Tdhdocument tdhdocument = new Tdhdocument()
                    {
                        DocumentId = fileStoredResponse.DocumentId,
                        FileName = fileStream.FileName,
                        EntityId = metadata.EntityId,
                        GroupId = metadata.GroupId,
                        FilePath = fileStoredResponse.FilePath,
                        ModuleId = metadata.ModuleId,
                        UploadedBy = metadata.UploadedBy,
                        UploadedDate = metadata.UploadedOn.ToString()
                    };
                    myDbContext.Tdhdocuments.Add(tdhdocument);
                    myDbContext.SaveChanges();

                    await transaction.CommitAsync();
                    return fileStoredResponse.DocumentId;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // Delete blob if uploaded
                    await _documentStorageService.DeleteDocument(fileStoredResponse.FilePath);
                    throw; // Re-throw exception for further handling/logging
                }
                // Optionally, index the document metadata in Elasticsearch
                //await _documentSearchService.IndexDocument(documentId, metadata);
            }
            
        }

        public async Task<Stream> GetDocument(string documentId)
        {
            return await _documentStorageService.GetDocument(documentId);
        }

        public async Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query)
        {
            return await _documentSearchService.SearchDocuments(query);
        }

        public async Task<MemoryStream> DownloadDocuments(List<string> documentIds)
        {
            var documentGUIds = documentIds.Select(id=>id.Trim().ToLower()).ToList();
            var filePathsQuery = myDbContext.Tdhdocuments.Where(x => documentGUIds.Contains(x.DocumentId.Trim().ToLower()));
            var filePaths = await filePathsQuery.ToListAsync();
            return await _documentStorageService.DownloadDocuments(filePaths);
        }
    }
}
