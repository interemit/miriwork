using Microsoft.AspNetCore.Http;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Data about the current request.
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Metadata of request.
        /// </summary>
        public RequestMetadata RequestMetadata { get; }

        /// <summary>
        /// Id of bounded context which handles request.
        /// </summary>
        public object BoundedContextId { get; }

        /// <summary>
        /// HttpContext of Asp.Net Core.
        /// </summary>
        public HttpContext HttpContext { get; }

        internal RequestContext(RequestMetadata requestMetadata, object boundedContextId, HttpContext httpContext)
        {
            this.RequestMetadata = requestMetadata;
            this.BoundedContextId = boundedContextId;
            this.HttpContext = httpContext;
        }
    }
}