using System;
using Microsoft.Extensions.DependencyInjection;

namespace Miriwork
{
    /// <summary>
    /// Extensions for IMvcBuilder.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds Miriwork to Mvc component of Asp.Net Core.
        /// </summary>
        /// <param name="mvcbuilder">MvcBuilder of Asp.Net Core</param>
        /// <param name="services">ServiceCollection of Asp.Net Core</param>
        /// <param name="boundedContextTypes">types of bounded contexts, e.g. "typeof(Example.BoundedContext.Bar)"</param>
        public static void AddMiriwork(this IMvcBuilder mvcbuilder, IServiceCollection services, 
            params Type[] boundedContextTypes)
        {
            Bootstrapper.InitMiriwork(mvcbuilder, services, boundedContextTypes);
        }

        /// <summary>
        /// Adds Miriwork to Mvc component of Asp.Net Core with a MiriworkConfiguration.
        /// </summary>
        /// <param name="mvcbuilder">MvcBuilder of Asp.Net Core</param>
        /// <param name="services">ServiceCollection of Asp.Net Core</param>
        /// <param name="miriworkConfiguration">instance of MiriworkConfiguation</param>
        /// <param name="boundedContextTypes">types of bounded contexts, e.g. "typeof(Example.BoundedContext.Bar)"</param>
        public static void AddMiriwork(this IMvcBuilder mvcbuilder, IServiceCollection services, 
            MiriworkConfiguration miriworkConfiguration, params Type[] boundedContextTypes)
        {
            Bootstrapper.InitMiriwork(mvcbuilder, services, miriworkConfiguration, boundedContextTypes);
        }
    }
}