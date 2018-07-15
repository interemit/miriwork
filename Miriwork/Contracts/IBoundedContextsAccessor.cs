using System.Collections.Generic;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Accessor for bounded contexts and metadata of requests.
    /// </summary>
    public interface IBoundedContextsAccessor
    {
        /// <summary>
        /// Returns all bounded contexts.
        /// </summary>
        IEnumerable<IBoundedContext> BoundedContexts{ get; }

        /// <summary>
        /// Returns all metadata of requests over all bounded contexts.
        /// </summary>
        IEnumerable<RequestMetadata> AllRequestMetadata { get; }
    }
}