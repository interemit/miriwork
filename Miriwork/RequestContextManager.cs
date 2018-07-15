using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Miriwork.Contracts;

namespace Miriwork
{
    internal class RequestContextManager : IRequestContextAccessor
    {
        private AsyncLocal<RequestContext> requestContext;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RequestIdFromHttpContextProvider requestIdFromHttpContextProvider;
        private readonly Dictionary<RequestId, object> request2BoundedContextId;
        private readonly Dictionary<RequestId, RequestMetadata> request2RequestMetadata;

        public RequestContextManager(IHttpContextAccessor httpContextAccessor,
            RequestIdFromHttpContextProvider requestIdFromHttpContextProvider,
            Dictionary<RequestId, object> request2BoundedContextId,
            Dictionary<RequestId, RequestMetadata> request2RequestMetadata)
        {
            this.requestContext = new AsyncLocal<RequestContext>();

            this.httpContextAccessor = httpContextAccessor;
            this.requestIdFromHttpContextProvider = requestIdFromHttpContextProvider;
            this.request2BoundedContextId = request2BoundedContextId;
            this.request2RequestMetadata = request2RequestMetadata;
        }

        public RequestContext RequestContext
        {
            get 
            {
                // the RequestContext has to be created by HttpContext, because it is needed before MiriServiceBus is used
                if (this.requestContext.Value == null)
                    CreateRequestContext(requestId: this.requestIdFromHttpContextProvider.GetRequestIdFromHttpContext());

                return this.requestContext.Value;
            }
        }

        private void CreateRequestContext(RequestId? requestId)
        {
            // requestId can be null if HttpContext is null
            if (requestId.HasValue)
            {
                this.requestContext.Value = new RequestContext(
                    this.request2RequestMetadata[requestId.Value],
                    this.request2BoundedContextId[requestId.Value],
                    this.httpContextAccessor.HttpContext
                );
            }
        }
    }
}