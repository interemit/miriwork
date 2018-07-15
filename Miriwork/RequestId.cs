using System;
using Miriwork.Contracts;

namespace Miriwork
{
    internal struct RequestId
    {
        public string RequestName { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public RequestId(string requestName, HttpMethod httpMethod)
        {
            this.RequestName = requestName;
            this.HttpMethod = httpMethod;
        }

        public static RequestId CreateFromRequestContext(RequestContext requestContext)
        {
            if (requestContext == null)
                throw new Exception("Cannot create RequestId because RequestContext is null.");

            return new RequestId(requestContext.RequestMetadata.RequestType.Name, requestContext.RequestMetadata.HttpMethod);
        }
    }
}