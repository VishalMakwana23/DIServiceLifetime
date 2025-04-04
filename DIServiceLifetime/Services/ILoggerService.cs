namespace DIServiceLifetime.Services
{
    /// <summary>
    /// Interface for logging services with different lifetimes.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Interface for a transient service.
        /// Transient services are created each time they are requested.
        /// </summary>
        public interface ITransientService
        {
            /// <summary>
            /// Gets the operation ID for the transient service.
            /// </summary>
            /// <returns>A unique identifier for the operation.</returns>
            Guid GetOperationId();
        }

        /// <summary>
        /// Interface for a scoped service.
        /// Scoped services are created once per request.
        /// </summary>
        public interface IScopedService
        {
            /// <summary>
            /// Gets the operation ID for the scoped service.
            /// </summary>
            /// <returns>A unique identifier for the operation.</returns>
            Guid GetOperationId();
        }

        /// <summary>
        /// Interface for a singleton service.
        /// Singleton services are created once and shared throughout the application's lifetime.
        /// </summary>
        public interface ISingletonService
        {
            /// <summary>
            /// Gets the operation ID for the singleton service.
            /// </summary>
            /// <returns>A unique identifier for the operation.</returns>
            Guid GetOperationId();
        }
    }
}
