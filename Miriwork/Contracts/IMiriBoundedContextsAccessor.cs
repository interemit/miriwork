using System.Collections.Generic;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Accessor for bounded contexts and metadata of requests.
    /// </summary>
    public interface IMiriBoundedContextsAccessor
    {
        /// <summary>
        /// Returns all bounded contexts.
        /// </summary>
        IEnumerable<IMiriBoundedContext> BoundedContexts{ get; }

        /// <summary>
        /// Returns all metadata of requests over all bounded contexts.
        /// </summary>
        IEnumerable<RequestMetadata> AllRequestMetadata { get; }
    }
}