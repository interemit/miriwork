using Microsoft.Extensions.DependencyInjection;
using Miriwork.Contracts;

namespace Example.Simple
{
    public class SimpleBoundedContext : IMiriBoundedContext
    {
        public object Id => null;

        public ApplicationServicesAssembly ApplicationServicesAssembly => ApplicationServicesAssembly.FromCallingAssembly();

        public RegistrationResult RegisterDependencies(IServiceCollection services)
        {
            // register some dependencies
            return RegistrationResult.EverythingRegistered();
        }
    }
}
