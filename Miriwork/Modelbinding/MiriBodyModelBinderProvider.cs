using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Miriwork.Modelbinding
{
    internal class MiriBodyModelBinderProvider : IModelBinderProvider
    {
        private readonly IList<IInputFormatter> formatters;
        private readonly IHttpRequestStreamReaderFactory readerFactory;

        public MiriBodyModelBinderProvider(MiriJsonInputFormatter miriJsonInputFormatter, IHttpRequestStreamReaderFactory readerFactory)
        {
            // use a custom ModelBinderProvider to register the MiriJsonInputFormatter in the default BodyModelBinder
            this.formatters = new List<IInputFormatter> { miriJsonInputFormatter };
            this.readerFactory = readerFactory;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // use MiriBodyModelBinderProvider only when ModelType is "object" ("object" is used in MiriController)
            if (context.Metadata.ModelType == typeof(object) 
                && context.BindingInfo.BindingSource != null 
                && context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))
            {
                return new BodyModelBinder(this.formatters, this.readerFactory);
            }

            return null;
        }
    }
}