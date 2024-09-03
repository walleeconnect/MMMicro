using Admin.Presentation;
using Appointment.Service;
using Compliance.Contracts;
using Compliance.Presentation;
using DocumentUpload.Presentation;
using EventHandling.Common;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ModuleA1.API;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCompliance();
builder.Services.AddDocumentService();
builder.Services.AddAppointment();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 200_000_000; // Set to 100 MB or desired size
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200_000_000; // Set to 100 MB, adjust as needed
});
builder.Services.AddControllers();
builder.Services.AddControllers().AddApplicationPart(typeof(DocumentUpload.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Admin.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Compliance.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Notify.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Users.Presentation.ForDI).Assembly);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEventPublisher, MediatorEventPublisher>();// Register in-process event handlers
//builder.Services.AddScoped<INotificationHandler<ComplianceAddedEvent>, MediatrComplianceAddedEventHandler>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
