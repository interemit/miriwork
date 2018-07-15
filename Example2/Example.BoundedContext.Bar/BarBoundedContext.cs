using System;
using System.Collections.Generic;
using Example.Webhosting;
using Microsoft.Extensions.DependencyInjection;
using Miriwork;
using Miriwork.Contracts;

namespace Example.BoundedContext.Bar
{
    public class BarBoundedContext : IBoundedContext
    {
        public object Id => BoundedContextId.Bar;

        public ApplicationServicesAssembly ApplicationServicesAssembly => ApplicationServicesAssembly.FromCallingAssembly();

        public RegistrationResult RegisterDependencies(IServiceCollection services)
            => RegistrationResult.ReturnDependenciesModuleToRegister(new BarDependencies());
    }
}