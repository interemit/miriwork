using System;
using System.Collections.Generic;
using System.Text;

namespace Miriwork
{
    /// <summary>
    /// Type of dependency injection used in application.
    /// </summary>
    public enum DependenciesRegistrationType
    {
        /// <summary>
        /// Dependencies of bounded contexts are registered by Miriwork automatically (default).
        /// </summary>
        DependenciesRegisteredByMiriwork = 0,

        /// <summary>
        /// Dependencies of bounded contexts have to be registered by your application.
        /// </summary>
        DependenciesRegisteredByApplication = 1
    }
}
