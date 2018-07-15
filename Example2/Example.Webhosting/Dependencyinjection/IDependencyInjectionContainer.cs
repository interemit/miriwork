using System;

namespace Example.Webhosting.Dependencyinjection
{
    public interface IDependencyInjectionContainer
    {
        object Get(Type type);

        T Get<T>();
    }
}