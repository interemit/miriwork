using Example.Webhosting;
using Microsoft.Extensions.DependencyInjection;
using Miriwork.Contracts;

namespace Example.BoundedContext.Bar
{
    public class BarBoundedContext : IMiriBoundedContext
    {
        public object Id => BoundedContextId.Bar;

        public ApplicationServicesAssembly ApplicationServicesAssembly => ApplicationServicesAssembly.FromCallingAssembly();

        public RegistrationResult RegisterDependencies(IServiceCollection services)
            => RegistrationResult.ReturnDependenciesModuleToRegister(new BarDependencies());
    }
}