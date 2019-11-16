using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;
using Miriwork.Contracts;

namespace Miriwork.Modelbinding
{
    internal class MiriQueryModelBinder : ComplexTypeModelBinder
    {
        private readonly IRequestContextAccessor requestContextAccessor;
        private readonly IModelMetadataProvider modelMetadataProvider;
        private readonly ModelBinderProviderContext modelBinderProviderContext;
        private Dictionary<ModelMetadata, IModelBinder> propertyBinders;

        public MiriQueryModelBinder(IRequestContextAccessor requestContextAccessor, 
            IModelMetadataProvider modelMetadataProvider, ModelBinderProviderContext modelBinderProviderContext,
            ILoggerFactory loggerFactory)
            : base(propertyBinders: new Dictionary<ModelMetadata, IModelBinder>(), loggerFactory: loggerFactory)
        {
            this.requestContextAccessor = requestContextAccessor;
            this.modelMetadataProvider = modelMetadataProvider;
            this.modelBinderProviderContext = modelBinderProviderContext;
        }

        protected override object CreateModel(ModelBindingContext bindingContext)
        {
            // https://github.com/aspnet/AspNetCore/blob/master/src/Mvc/Mvc.Core/src/ModelBinding/Binders/ComplexTypeModelBinder.cs

            // create "RequestType"-ModelMetadata of current request
            RequestMetadata currentRequestMetadata = this.requestContextAccessor.RequestContext.RequestMetadata;
            ModelMetadata requestModel = this.modelMetadataProvider.GetMetadataForType(currentRequestMetadata.RequestType);

            // create propertybinders (like in ComplexTypeModelBinderProvider)
            CreatePropertyBinders(requestModel);

            // set modelmetadata to modelmetadata of request (otherwise it is just "object")
            bindingContext.ModelMetadata = requestModel;
            // create a new instance of request
            return Activator.CreateInstance(requestModel.ModelType);
        }

        private void CreatePropertyBinders(ModelMetadata requestModel)
        {
            this.propertyBinders = new Dictionary<ModelMetadata, IModelBinder>();
            for (var i = 0; i < requestModel.Properties.Count; i++)
            {
                ModelMetadata property = requestModel.Properties[i];
                this.propertyBinders.Add(property, this.modelBinderProviderContext.CreateBinder(property));
            }
        }

        protected override Task BindProperty(ModelBindingContext bindingContext)
        {
            IModelBinder binder = this.propertyBinders[bindingContext.ModelMetadata];
            return binder.BindModelAsync(bindingContext);
        }
    }
}