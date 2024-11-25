using DocumentUpload.Infra;
using DocumentUpload.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DocumentUpload.Presentation
{
    public static class DocumentServiceExtensions
    {
        public static IServiceCollection AddDocumentService(this IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Service.DI).Assembly));
            services.AddScoped<IDocumentService, DocumentService>();
            //services.AddScoped<IDocumentStorageService, AzureBlobStorageService>();
            services.AddScoped<IDocumentStorageService, FileSystemStorageService>();
            services.AddScoped<IDocumentSearchService, ElasticSearchService>();
            // Register other ModuleA services
            return services;
        }
    }
}
