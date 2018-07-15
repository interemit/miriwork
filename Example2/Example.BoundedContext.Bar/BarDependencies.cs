using Autofac;
using Example.Webhosting.Dependencyinjection;

namespace Example.BoundedContext.Bar
{
    public class BarDependencies : BoundedContextDependencies
    {
        protected override void OnLoad(ContainerBuilder builder) 
        {
            builder.RegisterType<BarService>().AsSelf();
            builder.RegisterType<SomeBarClass>().As<ISomeBarClass>();
        }
    }
}