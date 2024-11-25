using DocumentUpload.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
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

        public async Task<FileStoredResponse> StoreDocument(DocumentMetadata metadata, IFormFile fileStream)
        {
            var extension = Path.GetExtension(fileStream.FileName);
            var fileName = Path.GetFileNameWithoutExtension(fileStream.FileName);
            var documentId = Guid.NewGuid().ToString();


            var fileNameWithExtension = $"{fileName}"+"-"+$"{documentId}{extension}";
            var relativePath = string.Format(RelativePathGenerator.RelativePaths["Compliance"], metadata.GroupId, metadata.EntityId);
            var dirPath = _storagePath + relativePath;
            var filePath = Path.Combine(dirPath, fileNameWithExtension);

            if (!Directory.Exists(dirPath)) 
            {
                Directory.CreateDirectory(filePath);
            }
            using (var stream = fileStream.OpenReadStream())
            {
                using (var fileStreamOut = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStreamOut);
                }
            }

            // Save metadata in a database if necessary

            return new FileStoredResponse() { DocumentId=  documentId, FilePath= relativePath };
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

        public Task<MemoryStream> DownloadDocuments(List<Tdhdocument> documents)
        {
            var memoryStream = new MemoryStream();
            
            {
                using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var document in documents)
                    {
                       // string filePath = GetFilePathFromDocumentId(documentId);

                        if (document.FilePath != null)
                        {
                            string fileName = Path.GetFileName(document.FilePath);
                            var zipEntry = zipArchive.CreateEntry(document.FileName, CompressionLevel.Fastest);

                            using (var zipEntryStream = zipEntry.Open())
                            using (var fileStream = System.IO.File.OpenRead(document.FilePath))
                            {
                                fileStream.CopyTo(zipEntryStream);
                            }
                        }
                        else
                        {
                            // Log or handle missing files
                            Console.WriteLine($"File not found for Document ID: {document.DocumentId}");
                        }
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                // Return the ZIP file
                return Task.FromResult(memoryStream);
            }
        }

        private string GetFilePathFromDocumentId(string documentId)
        {
            var filePath = Path.Combine(_storagePath, documentId);

            if (!File.Exists(filePath))
                return null;

            return filePath;
        }

        public Task<bool> DeleteDocument(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            } catch { }
            return Task.FromResult(false);
        }
    }

}
