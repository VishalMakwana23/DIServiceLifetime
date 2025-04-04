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
