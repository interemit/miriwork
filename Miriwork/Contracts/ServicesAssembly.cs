using System.Reflection;

namespace Miriwork.Contracts
{
    /// <summary>
    /// Assembly with application services which handle requests.
    /// </summary>
    public class ApplicationServicesAssembly
    {
        /// <summary>
        /// Assembly.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Creates an ApplicationServicesAssembly.
        /// </summary>
        /// <param name="applicationServicesAssembly">assembly with services</param>
        public ApplicationServicesAssembly(Assembly applicationServicesAssembly)
        {
            this.Assembly = applicationServicesAssembly;
        }

        /// <summary>
        /// Creates an ApplicationServicesAssembly from calling assembly (if calling assembly contains application services).
        /// </summary>
        /// <returns>ApplicationServicesAssembly</returns>
        public static ApplicationServicesAssembly FromCallingAssembly()
        {
            return new ApplicationServicesAssembly(Assembly.GetCallingAssembly());
        }
    }
}