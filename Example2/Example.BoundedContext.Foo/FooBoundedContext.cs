using Example.Webhosting;
using Microsoft.Extensions.DependencyInjection;
using Miriwork.Contracts;

namespace Example.BoundedContext.Foo
{
    public class FooBoundedContext : IMiriBoundedContext
    {
        public object Id => BoundedContextId.Foo;

        public ApplicationServicesAssembly ApplicationServicesAssembly => ApplicationServicesAssembly.FromCallingAssembly();

        public RegistrationResult RegisterDependencies(IServiceCollection services) 
            => RegistrationResult.ReturnDependenciesModuleToRegister(new FooDependencies());
    }
}