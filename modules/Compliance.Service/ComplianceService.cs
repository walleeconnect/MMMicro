using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compliance.Service
{
    public interface IComplianceService
    {
        Task<string> AddCompliance(ComplianceDTO compliance);
        Task<string> UpdateCompliance(string documentId);
        Task<string> UploadComplianceDocument(string query);
    }
    // DocumentService.cs
    public class ComplianceService : IComplianceService
    {
        public Task<string> AddCompliance(ComplianceDTO compliance)
        {
            Console.WriteLine("Compliance Added");
            return Task.FromResult(Guid.NewGuid().ToString());
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
