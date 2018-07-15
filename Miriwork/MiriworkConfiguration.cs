using System;

namespace Miriwork
{
    /// <summary>
    /// Configuration for Miriwork.
    /// </summary>
    public class MiriworkConfiguration
    {
        /// <summary>
        /// Type how dependencies of bounded contexts are registered (default is registered by Miriwork).
        /// </summary>
        public DependenciesRegistrationType DependenciesRegistrationType { get; set; }

        /// <summary>
        /// Should the application services be registered automatically? 
        /// The application services are registered in transient mode by Asp.Net Core dependency injection.
        /// Default value is true.
        /// </summary>
        public bool RegisterApplicationServicesByMiriwork { get; set; }

        /// <summary>
        /// Base type of all requests, e.g. some interface. If not set every class which name ends on "Request" is used as request.
        /// </summary>
        public Type RequestBaseType { get; set; }

        /// <summary>
        /// Base type of all responses, e.g. some interface. If not set every class which name ends on "Response" is used as request.
        /// </summary>
        public Type ResponseBaseType { get; set; }

        /// <summary>
        /// Base type of all application services, e.g. some interface. If not set every class which contains requests and responses is used.
        /// </summary>
        public Type ApplicationServiceBaseType { get; set; }
    }
}