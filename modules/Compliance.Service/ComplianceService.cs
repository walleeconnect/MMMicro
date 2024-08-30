using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Service
{
    public interface IComplianceService
    {
        Task<string> AddCompliance(string cf);
        Task<string> UpdateCompliance(string documentId);
        Task<string> UploadComplianceDocument(string query);
    }
    // DocumentService.cs
    public class ComplianceService : IComplianceService
    {
        public Task<string> AddCompliance(string cf)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateCompliance(string documentId)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadComplianceDocument(string query)
        {
            throw new NotImplementedException();
        }
    }

}
