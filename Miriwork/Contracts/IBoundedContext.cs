using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Interface for a bounded context. The instance will be created with Activator.CreateInstance() internally.
    /// </summary>
    public interface IBoundedContext
    {
        /// <summary>
        /// Id of bounded context, e.g. some enum.
        /// </summary>
        object Id { get; }

        /// <summary>
        /// Assembly which contains application service classes to handle requests.
        /// </summary>
        ApplicationServicesAssembly ApplicationServicesAssembly { get; }

        /// <summary>
        /// Register dependencies of bounded context.
        /// </summary>
        /// <param name="services">ServiceCollection of Asp.Net Core</param>
        /// <returns>Result of registration</returns>
        RegistrationResult RegisterDependencies(IServiceCollection services);
    }
}
