using Autofac;
using Example.Webhosting.Dependencyinjection;

namespace Example.BoundedContext.Foo
{
    public class FooDependencies : BoundedContextDependencies
    {
        protected override void OnLoad(ContainerBuilder builder) 
        {
            builder.RegisterType<FooService>().AsSelf();
            builder.RegisterType<SomeFooClass>().As<ISomeFooClass>();
        }
    }
}