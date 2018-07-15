using System;
using System.Collections.Generic;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Metadata of a request.
    /// </summary>
    public class RequestMetadata
    {
        /// <summary>
        /// Type of request.
        /// </summary>
        public Type RequestType { get; }

        /// <summary>
        /// Httpmethod of request.
        /// </summary>
        public HttpMethod HttpMethod { get; }

        /// <summary>
        /// Type of application service which handles request.
        /// </summary>
        public Type ApplicationServiceType { get; }

        /// <summary>
        /// Id of bounded context which handles request.
        /// </summary>
        public object BoundedContextId { get; internal set; }

        internal RequestMetadata(Type requestType, HttpMethod httpMethod, Type applicationServiceType)
        {
            this.RequestType = requestType;
            this.HttpMethod = httpMethod;
            this.ApplicationServiceType = applicationServiceType;
        }
    }
}