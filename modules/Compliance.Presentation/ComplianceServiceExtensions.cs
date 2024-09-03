using Compliance.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Compliance.Presentation
{
    public static class ComplianceServiceExtensions
    {
        public static IServiceCollection AddCompliance(this IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Service.DI).Assembly));
            services.AddScoped<IComplianceService, ComplianceService>();
            // Register other ModuleA services
            return services;
        }
    }
}
