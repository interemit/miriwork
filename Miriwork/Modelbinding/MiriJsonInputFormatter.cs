using System.Buffers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Miriwork.Contracts;

namespace Miriwork.Modelbinding
{
    internal class MiriJsonInputFormatter : JsonInputFormatter
    {
        private readonly IRequestContextAccessor requestContextAccessor;
        private readonly IModelMetadataProvider modelMetadataProvider;

        public MiriJsonInputFormatter(ILoggerFactory loggerFactory, IOptions<MvcJsonOptions> jsonOptions, 
            ArrayPool<char> charPool, ObjectPoolProvider objectPoolProvider, IRequestContextAccessor requestContextAccessor, 
            IModelMetadataProvider modelMetadataProvider)
            : base(loggerFactory.CreateLogger<JsonInputFormatter>(), jsonOptions.Value.SerializerSettings, 
                charPool, objectPoolProvider, options: null, jsonOptions: jsonOptions.Value)
        {
            this.requestContextAccessor = requestContextAccessor;
            this.modelMetadataProvider = modelMetadataProvider;
        }

        public override Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            // https://github.com/aspnet/Mvc/blob/master/src/Microsoft.AspNetCore.Mvc.Formatters.Json/JsonInputFormatter.cs

            // create "RequestType"-ModelMetadata of current request
            RequestMetadata currentRequestMetadata = this.requestContextAccessor.RequestContext.RequestMetadata;
            ModelMetadata requestModel = this.modelMetadataProvider.GetMetadataForType(currentRequestMetadata.RequestType);

            // create an default InputFormatterContext with the "RequestType"-ModelMetadata
            InputFormatterContext contextWithRequestModel = new InputFormatterContext(
                context.HttpContext,
                context.ModelName,
                context.ModelState,
                requestModel,
                context.ReaderFactory,
                context.TreatEmptyInputAsDefaultValue);

            return base.ReadAsync(contextWithRequestModel);
        }
    }
}