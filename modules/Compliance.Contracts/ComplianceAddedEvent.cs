using MediatR;

namespace Compliance.Contracts
{
    public class ComplianceAddedEvent : INotification
    {
        public string ComplianceId { get; }
        public int UserId { get; }
        public DateTime DueDate { get; }

        public ComplianceAddedEvent(string complianceId, int userId, DateTime dueDate)
        {
            ComplianceId = complianceId;
            UserId = userId;
            DueDate = dueDate;
        }
    }
}
