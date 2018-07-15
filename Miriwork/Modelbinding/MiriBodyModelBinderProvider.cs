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

        public MiriBodyModelBinderProvider(IList<IInputFormatter> formatters, IHttpRequestStreamReaderFactory readerFactory)
        {
            this.formatters = formatters;
            this.readerFactory = readerFactory;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            // use MiriBodyModelBinder only when ModelType is "object"
            if (context.Metadata.ModelType == typeof(object) 
                && context.BindingInfo.BindingSource != null 
                && context.BindingInfo.BindingSource.CanAcceptDataFrom(BindingSource.Body))
            {
                if (this.formatters.Count == 0)
                    throw new InvalidOperationException("No formatters");

                return new BodyModelBinder(this.formatters, this.readerFactory);
            }

            return null;
        }
    }
}