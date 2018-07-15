using Autofac;

namespace Example.Webhosting.Dependencyinjection
{
    public abstract class BoundedContextDependencies : Module
    {
        protected sealed override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DependencyInjectionContainer>().As<IDependencyInjectionContainer>();

            OnLoad(builder);
        }

        protected abstract void OnLoad(ContainerBuilder builder);
    }
}