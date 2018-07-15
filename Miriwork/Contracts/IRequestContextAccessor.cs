namespace Miriwork.Contracts
{
    /// <summary>
    /// Accessor for current RequestContext.
    /// </summary>
    public interface IRequestContextAccessor
    {
        /// <summary>
        /// Current RequestContext.
        /// </summary>
        RequestContext RequestContext { get; }
    }
}