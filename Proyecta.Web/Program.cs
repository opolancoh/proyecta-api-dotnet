using Microsoft.OpenApi.Models;
using Proyecta.Repository.EntityFramework;
using Proyecta.Web.Extensions;
using Proyecta.Web.Swagger;
using Proyecta.Web.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.ConfigurePersistenceServices();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureControllers();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proyecta API", Version = "v1" });
    c.DocumentFilter<ExcludeVersionRoutesDocumentFilter>();
});

var app = builder.Build();

app.ConfigureExceptionHandler(app.Logger);

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    SwaggerConfiguration.ConfigureSwaggerUI(app);
}

// DB EnsureCreated and Seed
app.MigrateDatabase<AuthDbContext>();
app.MigrateDatabase<ApiDbContext>();

app.UseApiVersioning();

app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// For testing purposes
public partial class Program
{
}