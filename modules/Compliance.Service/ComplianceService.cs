using Compliance.Contracts;
using EventHandling.Common;
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

        public Task<string> UploadComplianceDocument(string query)
        {
            throw new NotImplementedException();
        }
    }

}
