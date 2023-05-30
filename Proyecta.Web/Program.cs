using Proyecta.Repository.EntityFramework;
using Proyecta.Web.Extensions;
using Proyecta.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHealthChecks();
builder.Services.ConfigurePersistenceServices();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.AddControllers();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseApiVersioning();

// app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase<AppDbContext>();

app.Run();

// For testing purposes
public partial class Program {}