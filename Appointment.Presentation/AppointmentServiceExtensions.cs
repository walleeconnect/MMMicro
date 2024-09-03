using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ModuleA1.API
{
    public static class AppointmentServiceExtensions
    {
        public static IServiceCollection AddAppointment(this IServiceCollection services)
        {
            
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(typeof(Appointment.Service.DI).Assembly));
            // Register other ModuleA services
            return services;
        }
    }
}
