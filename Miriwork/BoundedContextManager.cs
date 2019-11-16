using System;
using System.Collections.Generic;
using System.Reflection;
using Miriwork.Contracts;

namespace Miriwork
{
    internal class BoundedContextManager : IMiriBoundedContextsAccessor
    {
        private RequestMetadataFactory requestMetadataFactory;

        private List<IMiriBoundedContext> boundedContexts;
        private List<RequestMetadata> allRequestMetadata;

        public IEnumerable<IMiriBoundedContext> BoundedContexts => this.boundedContexts;
        public IEnumerable<RequestMetadata> AllRequestMetadata => this.allRequestMetadata;

        public BoundedContextManager(MiriworkConfiguration miriworkConfiguration)
        {
            this.requestMetadataFactory = new RequestMetadataFactory(miriworkConfiguration?.RequestBaseType,
                miriworkConfiguration?.ResponseBaseType, miriworkConfiguration?.ApplicationServiceBaseType);
        }

        public void Init(Type[] boundedContextTypes)
        {
            this.boundedContexts = CreateBoundedContexts(boundedContextTypes);
            this.allRequestMetadata = CreateAllRequestMetadata();
        }

        private List<IMiriBoundedContext> CreateBoundedContexts(Type[] boundedContextTypes)
        {
            var boundedContextsTemp = new List<IMiriBoundedContext>();
            foreach (Type boundedContextType in boundedContextTypes)
            {
                object boundedContextInstance = Activator.CreateInstance(boundedContextType);
                if (!(boundedContextInstance is IMiriBoundedContext boundedContext))
                    throw new ArgumentException($"Bounded context {boundedContextType.Name} does not implement IBoundedContext.");

                boundedContextsTemp.Add(boundedContext);
            }

            return boundedContextsTemp;
        }

        private List<RequestMetadata> CreateAllRequestMetadata()
        {
            var allRequestMetadataTemp = new List<RequestMetadata>();
            foreach (IMiriBoundedContext boundedContext in this.boundedContexts)
            {
                var requestMetadataOfBoundedContext = this.requestMetadataFactory.CreateRequestMetadata(
                    boundedContext.ApplicationServicesAssembly.Assembly);
                foreach (RequestMetadata metadata in requestMetadataOfBoundedContext)
                {
                    metadata.BoundedContextId = boundedContext.Id;
                    allRequestMetadataTemp.Add(metadata);
                }
            }

            return allRequestMetadataTemp;
        }

        public RequestCollections CreateRequestCollections()
        {
            RequestCollections requestCollections = new RequestCollections();
            requestCollections.Request2BoundedContextId = new Dictionary<RequestId, object>();
            requestCollections.Request2RequestMetadata = new Dictionary<RequestId, RequestMetadata>();

            foreach (RequestMetadata metadata in this.allRequestMetadata)
            {
                var requestKey = new RequestId(metadata.RequestType.Name, metadata.HttpMethod);
                requestCollections.Request2BoundedContextId.Add(requestKey, metadata.BoundedContextId);
                requestCollections.Request2RequestMetadata.Add(requestKey, metadata);
            }

            return requestCollections;
        }
    }

    internal class RequestCollections
    {
        public Dictionary<RequestId, object> Request2BoundedContextId;

        public Dictionary<RequestId, RequestMetadata> Request2RequestMetadata;
    }
}