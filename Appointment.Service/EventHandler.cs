using Compliance.Contracts;
using EventHandling.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Service
{
    public static class DI { }
    public class MediatrComplianceAddedEventHandler : INotificationHandler<ComplianceAddedEvent>
    {
        public Task Handle(ComplianceAddedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("***********************DO HANDLED**************** Mediatr" + notification.ComplianceId );
            return Task.CompletedTask;
        }
    }

    public class RabbitComplianceAddedEventHandler : IEventHandler<ComplianceAddedEvent>
    {
        public Task Handle(ComplianceAddedEvent eventToHandle)
        {
            Console.WriteLine("***********************DO HANDLED**************** Rabbit" + eventToHandle.ComplianceId);
            return Task.CompletedTask;
        }
    }
}
