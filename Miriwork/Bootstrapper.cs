using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Miriwork.Contracts;
using Miriwork.Modelbinding;

namespace Miriwork
{
    internal static class Bootstrapper
    {
        public static void InitMiriwork(IMvcBuilder mvcbuilder, IServiceCollection services, 
            string[] boundedContextTypeNames)
        {
            InitMiriwork(mvcbuilder, services, null, boundedContextTypeNames, null);
        }

        public static void InitMiriwork(IMvcBuilder mvcbuilder, IServiceCollection services, 
            MiriworkConfiguration miriworkConfiguration, string[] boundedContextTypeNames)
        {
            InitMiriwork(mvcbuilder, services, miriworkConfiguration, boundedContextTypeNames, null);
        }

        public static void InitMiriwork(IMvcBuilder mvcbuilder, IServiceCollection services, 
            Type[] boundedContextTypes)
        {
            InitMiriwork(mvcbuilder, services, null, null, boundedContextTypes);
        }

        public static void InitMiriwork(IMvcBuilder mvcbuilder, IServiceCollection services, 
            MiriworkConfiguration miriworkConfiguration, Type[] boundedContextTypes)
        {
            InitMiriwork(mvcbuilder, services, miriworkConfiguration, null, boundedContextTypes);
        }

        private static void InitMiriwork(IMvcBuilder mvcbuilder, IServiceCollection services, 
            MiriworkConfiguration miriworkConfiguration, string[] boundedContextTypeNames, Type[] boundedContextTypes)
        {
            // add memorycache to cache http-requests
            services.AddMemoryCache();

            // add the assembly as an application part so that MiriController can be found
            mvcbuilder.AddApplicationPart(Assembly.GetExecutingAssembly());

            // create and register the Miriwork-classes
            services.AddHttpContextAccessor();
            var tempServiceProvider = services.BuildServiceProvider();
            var boundedContextManager = CreateBoundedContextManager(services, miriworkConfiguration,
                boundedContextTypeNames, boundedContextTypes);
            var requestIdFromHttpContextAccessor = CreateRequestIdFromHttpContextProvider(tempServiceProvider);
            var requestContextManager = CreateRequestContextAccessor(services, boundedContextManager,
                requestIdFromHttpContextAccessor, tempServiceProvider);
            AddMiriServiceBus(services, boundedContextManager, requestContextManager, tempServiceProvider);
            AddMiriModelBinderProviders(mvcbuilder, requestContextManager, tempServiceProvider);

            // register dependencies after registration of Miriwirk-classes
            RegisterDependenciesOfBoundedContexts(services, miriworkConfiguration, boundedContextManager);
            RegisterApplicationServicesOfBoundedContexts(services, miriworkConfiguration, boundedContextManager);
        }

        private static BoundedContextManager CreateBoundedContextManager(IServiceCollection services, 
            MiriworkConfiguration miriworkConfiguration, string[] boundedContextTypeNames, Type[] boundedContextTypes)
        {
            var boundedContextManager = new BoundedContextManager(miriworkConfiguration);
            if (boundedContextTypeNames != null)
                boundedContextManager.Init(boundedContextTypeNames);
            else
                boundedContextManager.Init(boundedContextTypes);

            services.AddSingleton<IMiriBoundedContextsAccessor>(boundedContextManager);
            
            return boundedContextManager;
        }

        private static RequestIdFromHttpContextProvider CreateRequestIdFromHttpContextProvider(
            ServiceProvider serviceProvider)
        {
            return new RequestIdFromHttpContextProvider(serviceProvider.GetService<IHttpContextAccessor>(),
                serviceProvider.GetService<IMemoryCache>());
        }

        private static RequestContextManager CreateRequestContextAccessor(IServiceCollection services,
            BoundedContextManager boundedContextManager, RequestIdFromHttpContextProvider requestIdFromHttpContextAccessor, 
            ServiceProvider serviceProvider)
        {
            RequestCollections requestCollections = boundedContextManager.CreateRequestCollections();

            var requestContextManager = new RequestContextManager(serviceProvider.GetService<IHttpContextAccessor>(), 
                requestIdFromHttpContextAccessor, requestCollections.Request2BoundedContextId,
                requestCollections.Request2RequestMetadata);
            services.AddSingleton<IRequestContextAccessor>(requestContextManager);

            return requestContextManager;
        }

        private static void AddMiriServiceBus(IServiceCollection services, BoundedContextManager boundedContextManager,
            RequestContextManager requestContextManager, ServiceProvider tempServiceProvider)
        {
            Dictionary<Type, Type> requestType2ApplicationServiceType = new Dictionary<Type, Type>();
            foreach (RequestMetadata requestMetadata in boundedContextManager.AllRequestMetadata)
            {
                if (!requestType2ApplicationServiceType.ContainsKey(requestMetadata.RequestType))
                    requestType2ApplicationServiceType.Add(requestMetadata.RequestType, requestMetadata.ApplicationServiceType);
            }

            MiriServiceBus serviceBus = new MiriServiceBus(tempServiceProvider.GetService<IHttpContextAccessor>(),
                requestContextManager, requestType2ApplicationServiceType);
            services.AddSingleton<IMiriServiceBus>(serviceBus);
        }

        private static void AddMiriModelBinderProviders(IMvcBuilder mvcbuilder, 
            RequestContextManager requestContextManager, ServiceProvider tempServiceProvider)
        {
            IModelMetadataProvider modelMetadataProvider = tempServiceProvider.GetService<IModelMetadataProvider>();

            // add MiriQueryModelBinderProvider for GET/DELETE-requests
            mvcbuilder.AddMvcOptions(options => options.ModelBinderProviders.Insert(0,
                new MiriQueryModelBinderProvider(requestContextManager, modelMetadataProvider, 
                    tempServiceProvider.GetService<ILoggerFactory>())));

            // create MiriJsonInputFormatter and add MiriBodyModelBinderProvider for PUT/POST-requests
            MiriJsonInputFormatter miriFormatter = new MiriJsonInputFormatter(
                tempServiceProvider.GetService<ILoggerFactory>(),
                tempServiceProvider.GetService<IOptions<MvcJsonOptions>>(),
                tempServiceProvider.GetService<ArrayPool<char>>(),
                tempServiceProvider.GetService<ObjectPoolProvider>(),
                requestContextManager,
                modelMetadataProvider);

            mvcbuilder.AddMvcOptions(options => options.ModelBinderProviders.Insert(0,
                new MiriBodyModelBinderProvider(new List<IInputFormatter> { miriFormatter },
                    tempServiceProvider.GetService<IHttpRequestStreamReaderFactory>())));
        }

        private static void RegisterDependenciesOfBoundedContexts(IServiceCollection services, MiriworkConfiguration miriworkConfiguration,
            BoundedContextManager boundedContextManager)
        {
            if (miriworkConfiguration?.DependenciesRegistrationType == DependenciesRegistrationType.DependenciesRegisteredByApplication)
                return;

            foreach (IMiriBoundedContext boundedContext in boundedContextManager.BoundedContexts)
            {
                RegistrationResult registrationResult = boundedContext.RegisterDependencies(services);

                if (registrationResult.ResultType == RegistrationResultType.ReturnedDependenciesModuleToRegister)
                {
                    throw new ArgumentException("Cannot register unknown dependencies module in Miriwork." + Environment.NewLine
                        + "Perhaps wrong value for MiriworkConfiguration.DependenciesRegistrationType?");
                }
            }
        }

        private static void RegisterApplicationServicesOfBoundedContexts(IServiceCollection services, 
            MiriworkConfiguration miriworkConfiguration, BoundedContextManager boundedContextManager)
        {
            if (miriworkConfiguration?.RegisterApplicationServicesByMiriwork == false)
                return;

            var applicationServiceTypes = boundedContextManager.AllRequestMetadata.Select(m => m.ApplicationServiceType).Distinct();
            foreach (Type serviceType in applicationServiceTypes)
                services.AddTransient(serviceType);
        }
    }
}