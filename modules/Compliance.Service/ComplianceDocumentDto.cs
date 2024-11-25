using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Service
{
    public class DocumentMetadata
    {
        public List<IFormFile> formFiles;
        public string EntityId { get; set; }
        public string GroupId { get; set; }
        public string ModuleId { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        public List<string> Tags { get; set; } // E.g., keywords for search
    }
    public class ComplianceDocumentDto
    {
        public List<IFormFile> formFiles { get; set; }
        public int ComplianceId {  get; set; }
        public string EntityId {  get; set; }
        public string GroupId { get; set; }
        public string TypeOfLawId { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public string ModuleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
