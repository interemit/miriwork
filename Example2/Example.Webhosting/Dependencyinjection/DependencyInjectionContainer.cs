using System;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Webhosting.Dependencyinjection
{
    public class DependencyInjectionContainer : IDependencyInjectionContainer
    {
        private IServiceProvider serviceProvider;

        private DependencyInjectionContainer(IServiceProvider serviceProvider) 
        {
            this.serviceProvider = serviceProvider;
        }

        public object Get(Type type) => serviceProvider.GetService(type);

        public T Get<T>() => serviceProvider.GetService<T>();
    }
}