using Microsoft.AspNetCore.Http;

namespace DocumentUpload.Service
{
    public class DocumentMetadata
    {
        public List<IFormFile> formFiles { get; set; }
        public string EntityId { get; set; }
        public string GroupId { get; set; }
        public string ModuleId { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        //public List<string> Tags { get; set; } // E.g., keywords for search
    }

}