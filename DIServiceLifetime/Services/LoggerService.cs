using static DIServiceLifetime.Services.ILoggerService;

namespace DIServiceLifetime.Services
{
    // Implementation of a transient service
    // A new instance is created each time it is requested
    public class TransientService : ITransientService
    {
        private readonly Guid _operationId;

        public TransientService()
        {
            _operationId = Guid.NewGuid();
            Console.WriteLine($"[Transient] Instance created: {_operationId}");
        }

        public Guid GetOperationId() => _operationId;
    }

    // Implementation of a scoped service
    // A new instance is created once per request
    public class ScopedService : IScopedService
    {
        private readonly Guid _operationId;

        public ScopedService()
        {
            _operationId = Guid.NewGuid();
            Console.WriteLine($"[Scoped] Instance created: {_operationId}");
        }

        public Guid GetOperationId() => _operationId;
    }

    // Implementation of a singleton service
    // A single instance is created and shared throughout the application's lifetime
    public class SingletonService : ISingletonService
    {
        private readonly Guid _operationId;

        public SingletonService()
        {
            _operationId = Guid.NewGuid();
            Console.WriteLine($"[Singleton] Instance created: {_operationId}");
        }

        public Guid GetOperationId() => _operationId;
    }
}
