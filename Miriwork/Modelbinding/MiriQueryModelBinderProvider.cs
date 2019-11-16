using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Miriwork.Contracts;

namespace Miriwork.Modelbinding
{
    internal class MiriQueryModelBinderProvider : IModelBinderProvider
    {
        private readonly IRequestContextAccessor requestContextAccessor;
        private readonly IModelMetadataProvider modelMetadataProvider;
        private readonly ILoggerFactory loggerFactory;

        public MiriQueryModelBinderProvider(IRequestContextAccessor requestContextAccessor,
            IModelMetadataProvider modelMetadataProvider, ILoggerFactory loggerFactory)
        {
            // use a custom ModelBinderProvider to register the MiriQueryModelBinder
            this.requestContextAccessor = requestContextAccessor;
            this.modelMetadataProvider = modelMetadataProvider;
            this.loggerFactory = loggerFactory;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // use MiriQueryModelBinderProvider only when ModelType is "object" ("object" is used in MiriController)
            if (context.Metadata.ModelType == typeof(object)
                && context.BindingInfo.BindingSource != null 
                && context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Query))
            {
                return new MiriQueryModelBinder(this.requestContextAccessor, this.modelMetadataProvider, context, loggerFactory);
            }

            return null;
        }
    }
}