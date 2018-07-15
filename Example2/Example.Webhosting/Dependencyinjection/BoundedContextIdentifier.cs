using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using Miriwork.Contracts;

namespace Example.Webhosting.Dependencyinjection
{
    // https://github.com/autofac/Autofac.AspNetCore.Multitenant/tree/develop/samples/Sandbox
    public class BoundedContextIdentifier : ITenantIdentificationStrategy
    {
        private IRequestContextAccessor requestContextAccessor;

        public BoundedContextIdentifier(IRequestContextAccessor requestContextAccessor)
        {
            this.requestContextAccessor = requestContextAccessor;
        }

        public bool TryIdentifyTenant(out object tenantId)
        {
            RequestContext requestContext = this.requestContextAccessor.RequestContext;
            if (requestContext == null)
            {
                tenantId = null;
                return false;
            }

            tenantId = requestContext.BoundedContextId;
            return true;
        }
    }
}