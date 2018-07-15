using System;
using System.Collections.Generic;
using Example.Webhosting;
using Microsoft.Extensions.DependencyInjection;
using Miriwork;
using Miriwork.Contracts;

namespace Example.BoundedContext.Foo
{
    public class FooBoundedContext : IBoundedContext
    {
        public object Id => BoundedContextId.Foo;

        public ApplicationServicesAssembly ApplicationServicesAssembly => ApplicationServicesAssembly.FromCallingAssembly();

        public RegistrationResult RegisterDependencies(IServiceCollection services) 
            => RegistrationResult.ReturnDependenciesModuleToRegister(new FooDependencies());
    }
}