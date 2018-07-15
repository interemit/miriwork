using Autofac;

namespace Example.Webhosting.Dependencyinjection
{
    public class InfrastructureDependencies : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationServiceBus>().As<IApplicationServiceBus>().SingleInstance();
        }
    }
}