using DIServiceLifetime.Services;
using Microsoft.OpenApi.Models;
using static DIServiceLifetime.Services.ILoggerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "A simple demo API using Swagger"
    });
});

// Register services with different lifetimes
builder.Services.AddTransient<ITransientService, TransientService>(); // Transient: Created each time they are requested
builder.Services.AddScoped<IScopedService, ScopedService>(); // Scoped: Created once per request
builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Singleton: Created once and shared

var app = builder.Build();

// Enable Swagger in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;  // Makes Swagger open at root URL (http://localhost:5000)
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
