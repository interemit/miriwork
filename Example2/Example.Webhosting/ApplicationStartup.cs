using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Example.Webhosting.Dependencyinjection;
using Example.Webhosting.Servicemodel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Miriwork;
using Miriwork.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Example.Webhosting
{
    public abstract class ApplicationStartup
    {
        internal static MultitenantContainer ApplicationContainer;

        public IConfiguration Configuration { get; }

        public ApplicationStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Type[] boundedContextTypes = GetBoundedContextTypes(
                "Example.BoundedContext.Bar.BarBoundedContext, Example.BoundedContext.Bar",
                "Example.BoundedContext.Foo.FooBoundedContext, Example.BoundedContext.Foo");

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddMiriwork(services, new MiriworkConfiguration
                    {
                        DependenciesRegistrationType = DependenciesRegistrationType.DependenciesRegisteredByApplication,
                        RegisterApplicationServicesByMiriwork = false,
                        RequestBaseType = typeof(IRequest),
                        ResponseBaseType = typeof(IResponse),
                        ApplicationServiceBaseType = typeof(IApplicationService)
                    }, 
                    boundedContextTypes);

            ApplicationContainer = CreateApplicationContainer(services);
            return new AutofacServiceProvider(ApplicationContainer);
        }

        private MultitenantContainer CreateApplicationContainer(IServiceCollection services)
        {
            // http://docs.autofac.org/en/latest/integration/aspnetcore.html#multitenant-support
            // https://github.com/autofac/Autofac.AspNetCore.Multitenant/tree/develop/samples/Sandbox
            services.AddHttpContextAccessor();

            // create the container builder.
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new InfrastructureDependencies());
            var container = builder.Build();

            var strategy = new BoundedContextIdentifier(container.Resolve<IRequestContextAccessor>());
            var applicationContainer = new MultitenantContainer(strategy, container);

            // register components of bounded contexts
            var boundedContextsAccessor = container.Resolve<IMiriBoundedContextsAccessor>();
            foreach (IMiriBoundedContext bc in boundedContextsAccessor.BoundedContexts)
            {
                var registrationResult = bc.RegisterDependencies(services);
                var boundedContextDependencies = registrationResult.DependenciesModuleAs<BoundedContextDependencies>();
                applicationContainer.ConfigureTenant(bc.Id, b => b.RegisterModule(boundedContextDependencies));
            }

            return applicationContainer;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();

            // activate wwwroot and default-file (index.html)
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = ""
            });
        }

        private Type[] GetBoundedContextTypes(params string[] boundedContextTypeNames)
        {
            List<Type> boundedContextTypes = new List<Type>();
            foreach (string fullTypeName in boundedContextTypeNames)
            {
                Type type = Type.GetType(fullTypeName);
                
                // TODO: in UnitTests kann nur 1 BoundedContext getestet werden, aber beim richtigen
                // Starten müssen(!) alle geladen werden -> wie dies unterscheiden?
                if (type == null)
                    type = GetBoundedContextTypeFromAssembly(fullTypeName);

                if (type == null)
                    throw new Exception($"Type {fullTypeName} not found");

                boundedContextTypes.Add(type);
            }

            return boundedContextTypes.ToArray();
        }

        private Type GetBoundedContextTypeFromAssembly(string fullTypeName)
        {
            return Type.GetType(fullTypeName, name => Assembly.LoadFrom(name.Name + ".dll"), null);
        }
    }
}