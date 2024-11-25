using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DocumentUpload.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace DocumentUpload.Infra
{
    public class AzureBlobStorageService : IDocumentStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _containerName = configuration["AzureBlobStorage:ContainerName"] ?? "default-container";

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<FileStoredResponse> StoreDocument(DocumentMetadata metadata, IFormFile fileStream)
        {
         
                var extension = Path.GetExtension(fileStream.FileName);
                var fileName = Path.GetFileNameWithoutExtension(fileStream.FileName);
                var documentId = Guid.NewGuid().ToString();
                var fileNameWithExtension = $"{fileName}-{documentId}{extension}";

                var relativePath = string.Format(RelativePathGenerator.RelativePaths["Compliance"], metadata.GroupId, metadata.EntityId);
                var blobName = Path.Combine(relativePath, fileNameWithExtension).Replace("\\", "/");
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);

                var blobClient = containerClient.GetBlobClient(blobName);

                using (var stream = fileStream.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileStream.ContentType });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
            
            return new FileStoredResponse { DocumentId = documentId, FilePath = blobName };
        }

        public async Task<Stream> GetDocument(string documentId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(documentId);

            if (!await blobClient.ExistsAsync())
                return null;

            return await blobClient.OpenReadAsync();
        }

        public async Task<IEnumerable<DocumentMetadata>> SearchDocuments(string query)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobs = containerClient.GetBlobsAsync();

            var result = new List<DocumentMetadata>();
            await foreach (var blob in blobs)
            {
                if (blob.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    //result.Add(new DocumentMetadata
                    //{
                    //    FileName = Path.GetFileName(blob.Name),
                    //    GroupId = GetGroupId(blob.Name),
                    //    EntityId = GetEntityId(blob.Name),
                    //    FilePath = blob.Name
                    //});
                }
            }

            return result;
        }

        public async Task<MemoryStream> DownloadDocuments(List<Tdhdocument> documents)
        {
            var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var document in documents)
                {
                    var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                    var blobClient = containerClient.GetBlobClient(document.FilePath);

                    if (await blobClient.ExistsAsync())
                    {
                        var zipEntry = zipArchive.CreateEntry(document.FileName, CompressionLevel.Fastest);

                        using (var zipEntryStream = zipEntry.Open())
                        using (var blobStream = await blobClient.OpenReadAsync())
                        {
                            await blobStream.CopyToAsync(zipEntryStream);
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
            return memoryStream;
        }

        public async Task<bool> DeleteDocument(string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(filePath);

            return await blobClient.DeleteIfExistsAsync();
        }

        private string GetGroupId(string blobName)
        {
            var parts = blobName.Split('/');
            return parts.Length > 0 ? parts[0] : string.Empty;
        }

        private string GetEntityId(string blobName)
        {
            var parts = blobName.Split('/');
            return parts.Length > 1 ? parts[1] : string.Empty;
        }
    }
}
