using Compliance.Contracts;
using DocumentUpload.Service;
using EventHandling.Common;
using MassTransit.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Compliance.Service
{
    public interface IComplianceService
    {
        Task<string> AddCompliance(ComplianceDTO compliance);
        Task<string> UpdateCompliance(string documentId);
        Task<string> UploadComplianceDocument(ComplianceDocumentDto query);
    }
    // DocumentService.cs
    public class ComplianceService : IComplianceService
    {
        private IEventPublisher _eventPublisher;

        public ComplianceService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task<string> AddCompliance(ComplianceDTO compliance)
        {
            Console.WriteLine("Compliance Added");
            string id = Guid.NewGuid().ToString();
            _eventPublisher.Publish(new ComplianceAddedEvent(id, 1, compliance.DueDate));
            return Task.FromResult(id);
        }

        public Task<string> UpdateCompliance(string documentId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadComplianceDocument(ComplianceDocumentDto query)
        {
            MyDbContext myDbContext = new MyDbContext();
            using (var transaction = await myDbContext.Database.BeginTransactionAsync())
            {
                List< TdhcomplianceDocument> documents = new List< TdhcomplianceDocument >();
                foreach (var item in query.formFiles)
                {
                    TdhcomplianceDocument document = new TdhcomplianceDocument();
                    document.ComplianceId = query.ComplianceId.ToString();
                    document.EntityId = query.EntityId;
                    document.FileName = item.FileName;
                    document.CreatedBy = query.CreatedBy;
                    document.CreatedDate = query.CreatedDateTime.ToString();
                    document.Status = "Pending";
                    document.IsActive = true;
                    documents.Add( document );
                    myDbContext.Add(document);
                }
                await myDbContext.SaveChangesAsync();

                List<string> documentId =  await StoreDocuments(query);
                
                foreach(var document in documents)
                {
                    document.Status = "Completed";
                    document.DocumentId = documentId[0];
                    myDbContext.TdhcomplianceDocuments.Update(document);
                }
                await myDbContext.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            return "null";
        }

        public async Task<List<string>> StoreDocuments(ComplianceDocumentDto query)
        {
            DocumentMetadata documentMetadata = new DocumentMetadata()
            {
                EntityId = query.EntityId,
                GroupId = query.GroupId,
                formFiles = query.formFiles,
                ModuleId = query.ModuleId,
                UploadedBy = query.CreatedBy,
                UploadedOn = query.CreatedDateTime
            };
            using var content = new MultipartFormDataContent();
            // Add each metadata property as a separate part
            content.Add(new StringContent(documentMetadata.GroupId ?? "STDA01"), nameof(documentMetadata.GroupId));
            content.Add(new StringContent(documentMetadata.EntityId ?? "2"), nameof(documentMetadata.EntityId));
            content.Add(new StringContent(documentMetadata.ModuleId ?? "1"), nameof(documentMetadata.ModuleId));
            content.Add(new StringContent(documentMetadata.UploadedBy ?? "SA"), nameof(documentMetadata.UploadedBy));
            content.Add(new StringContent(documentMetadata.UploadedOn.ToString("o")), nameof(documentMetadata.UploadedOn));
            foreach (var documentRequest in query.formFiles)
            {
                // Add the file as a separate content part
                if (documentRequest != null)
                {
                    var fileContent = new StreamContent(documentRequest.OpenReadStream());
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(documentRequest.ContentType);
                    content.Add(fileContent, "formFiles", documentRequest.FileName);
                }
            }

            HttpClient _client = new HttpClient { BaseAddress = new Uri("https://localhost:63488/") };
            var response = await _client.PostAsync("api/Documents/upload", content);
            var result = await response.Content.ReadAsStringAsync();
            var documentIds = JsonSerializer.Deserialize<UploadDocumentResponse>(result);
            return documentIds.documentId;
        }
    }
    public class UploadDocumentResponse
    {
        public List<string> documentId { get; set; }
    }
}
