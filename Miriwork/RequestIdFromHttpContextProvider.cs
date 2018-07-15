using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Miriwork.Contracts;

namespace Miriwork
{
    internal class RequestIdFromHttpContextProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMemoryCache memoryCache;

        public RequestIdFromHttpContextProvider(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.memoryCache = memoryCache;
        }

        public RequestId? GetRequestIdFromHttpContext()
        {
            HttpContext httpContext = this.httpContextAccessor.HttpContext;
            if (httpContext == null)
                return null;

            RequestId? cacheEntry = memoryCache.GetOrCreate(httpContext, entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromSeconds(2);
                    return GetRequestKey(httpContext);
                });
            
            return cacheEntry.HasValue ? cacheEntry.Value : (RequestId?)null;
        }

        private RequestId? GetRequestKey(HttpContext httpContext)
        {
            if (TryGetRequestName(httpContext, out string requestName) 
                && TryGetHttpMethod(httpContext, out HttpMethod? httpMethod))
                return new RequestId(requestName, httpMethod.Value);

            return null;
        }

        private bool TryGetRequestName(HttpContext httpContext, out string requestName)
        {
            // example: /services/BarRequest
            string url = httpContext.Request.Path.Value;
            Match match = Regex.Match(url, @"\/services\/(\w*)");
            if (match.Success && match.Groups.Count == 2)
            {
                // group[0] is the whole match und group[1] is the requestname (in parentheses)
                requestName = match.Groups[1].Value;
                return true;
            }

            requestName = null;
            return false;
        }

        private bool TryGetHttpMethod(HttpContext httpContext, out HttpMethod? httpMethod)
        {
            if (Enum.TryParse(typeof(HttpMethod), httpContext.Request.Method, ignoreCase: true, result: out object method))
            {
                httpMethod = (HttpMethod)method;
                return true;
            }

            httpMethod = null;
            return false;
        }
    }
}