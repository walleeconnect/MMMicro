using Admin.Presentation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddApplicationPart(typeof(DocumentUpload.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Admin.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Compliance.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Notify.Presentation.ForDI).Assembly);
builder.Services.AddControllers().AddApplicationPart(typeof(Users.Presentation.ForDI).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
