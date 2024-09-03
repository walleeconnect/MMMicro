using DocumentUpload.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload.Infra
{
    // FileSystemStorageService.cs
    public class FileSystemStorageService : IDocumentStorageService
    {
        private readonly string _storagePath;

        public FileSystemStorageService(IConfiguration configuration)
        {
            _storagePath = "D:\\Modulith\\FileStorage";
        }

        public async Task<string> StoreDocument(DocumentMetadata metadata, Stream fileStream)
        {
            var extension = Path.GetExtension(metadata.FileName);
            var documentId = Guid.NewGuid().ToString();
            var fileNameWithExtension = $"{documentId}{extension}";
            var filePath = Path.Combine(_storagePath, fileNameWithExtension);

            using (var fileStreamOut = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamOut);
            }

            // Save metadata in a database if necessary

            return documentId;
        }
        public async Task<Stream> GetDocument(string documentId)
        {
            var filePath = Path.Combine(_storagePath, documentId);

            if (!File.Exists(filePath))
                return null;

            // Open the file stream in read-only mode
            return new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        //public async Task<Stream> GetDocument(string documentId)
        //{
        //    var filePath = Path.Combine(_storagePath, documentId);
        //    var memoryStream = new MemoryStream();

        //    using (var fileStreamIn = new FileStream(filePath, FileMode.Open))
        //    {
        //        await fileStreamIn.CopyToAsync(memoryStream);
        //    }

        //    memoryStream.Position = 0; // Reset the stream position for reading
        //    return memoryStream;
        //}

        public async Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query)
        {
            // Integrate with Elasticsearch or another search engine
            throw new NotImplementedException();
        }
    }

}
