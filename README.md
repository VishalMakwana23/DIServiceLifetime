
![ChatGPT Image Apr 4, 2025, 06_04_19 PM](https://github.com/user-attachments/assets/b616a27c-bef5-4635-bb78-7c0db195a1f1)


# **Dependency Injection (DI) in .NET Core - Transient, Scoped, Singleton**

## **Overview**
This repository demonstrates the different **service lifetimes** in .NET Core's **built-in Dependency Injection (DI)** system. It covers:

- **Transient**: A new instance is created every time it is requested.
- **Scoped**: A single instance is created per HTTP request.
- **Singleton**: A single instance is created and shared across the entire application lifetime.

## **Technologies Used**
- .NET Core 6/7/8/9
- ASP.NET Web API
- Swagger UI

## **Getting Started**
### **Clone the Repository**
```sh
git clone https://github.com/your-repo-name.git
cd your-repo-name
```

### **Run the Project**
```sh
dotnet run
```
The API will be available at **http://localhost:5000** (or your configured port).

### **Open Swagger UI**
Navigate to:
```
http://localhost:5000
```
Swagger UI will automatically open, allowing you to test the API endpoints.

## **Registering Services in Program.cs**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services with different lifetimes
builder.Services.AddTransient<ITransientService, TransientService>();
builder.Services.AddScoped<IScopedService, ScopedService>();
builder.Services.AddSingleton<ISingletonService, SingletonService>();

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

## **Service Implementations**

### **Transient Service** (New instance per request)
```csharp
public class TransientService : ITransientService
{
    private readonly string _operationId = Guid.NewGuid().ToString();
    public string GetOperationId() => _operationId;
}
```

### **Scoped Service** (One instance per request)
```csharp
public class ScopedService : IScopedService
{
    private readonly string _operationId = Guid.NewGuid().ToString();
    public string GetOperationId() => _operationId;
}
```

### **Singleton Service** (One instance for the application lifetime)
```csharp
public class SingletonService : ISingletonService
{
    private readonly string _operationId = Guid.NewGuid().ToString();
    public string GetOperationId() => _operationId;
}
```

## **Testing the Service Lifetimes**
### **Controller Implementation**
```csharp
using DIServiceLifetime.Services;
using Microsoft.AspNetCore.Mvc;
using static DIServiceLifetime.Services.ILoggerService;

namespace DIServiceLifetime.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly ITransientService _transientService;
        private readonly IScopedService _scopedService;
        private readonly ISingletonService _singletonService;
        private readonly IServiceProvider _serviceProvider;

        // Constructor to inject the services
        public DemoController(
            ITransientService transientService,
            IScopedService scopedService,
            ISingletonService singletonService,
            IServiceProvider serviceProvider
            )
        {
            _transientService = transientService;
            _scopedService = scopedService;
            _singletonService = singletonService;
            _serviceProvider = serviceProvider;
        }

        // Endpoint to get the operation IDs of the services
        [HttpGet]
        public IActionResult GetServiceIds()
        {
            return Ok(new
            {
                Transient = _transientService.GetOperationId(), // Transient service ID
                Scoped = _scopedService.GetOperationId(), // Scoped service ID
                Singleton = _singletonService.GetOperationId() // Singleton service ID
            });
        }

        // Endpoint to get multiple instances of the services
        [HttpGet("multiple")]
        public IActionResult GetMultiple()
        {
            // Resolving services again to demonstrate their lifetimes
            var transientService2 = _serviceProvider.GetRequiredService<ITransientService>();
            var scopedService2 = _serviceProvider.GetRequiredService<IScopedService>();
            var singletonService2 = _serviceProvider.GetRequiredService<ISingletonService>();

            return Ok(new
            {
                Transient_1 = _transientService.GetOperationId(), // First transient service ID
                Transient_2 = transientService2.GetOperationId(),  // Second transient service ID (should be different)

                Scoped_1 = _scopedService.GetOperationId(), // First scoped service ID
                Scoped_2 = scopedService2.GetOperationId(),  // Second scoped service ID (should be same)

                Singleton_1 = _singletonService.GetOperationId(), // First singleton service ID
                Singleton_2 = singletonService2.GetOperationId() // Second singleton service ID (should be same)
            });
        }
    }
}

```

### **Expected Output (for one request)**
```json
{
  "transient_1": "3f4a67a3-1234-4d6a-b72c-3e1e527b9421",
  "transient_2": "89c6d2e3-5678-4d9c-8b1a-2e3d456a8976",
  "scoped_1": "b7f3e1c4-4321-4c9a-b123-7a5e6d3a2145",
  "scoped_2": "b7f3e1c4-4321-4c9a-b123-7a5e6d3a2145",
  "singleton_1": "a1b2c3d4-5678-4e9a-b3c4-d5e6f7g8h9i0",
  "singleton_2": "a1b2c3d4-5678-4e9a-b3c4-d5e6f7g8h9i0"
}
```

### **Key Observations**
- **Transient**: Generates a new ID every time it is requested.
- **Scoped**: The same ID within a request, but different across requests.
- **Singleton**: Always the same ID throughout the application.

## **When to Use Which Lifetime?**
| Lifetime    | Best for |
|------------|----------|
| **Transient** | Lightweight, stateless operations (e.g., email sender) |
| **Scoped** | Database operations (e.g., DbContext in Entity Framework) |
| **Singleton** | Caching, logging, configuration settings |

## **Conclusion**
Understanding DI lifetimes helps optimize memory and performance. Choose the correct lifetime based on your application's needs!


🚀 Happy Coding!
