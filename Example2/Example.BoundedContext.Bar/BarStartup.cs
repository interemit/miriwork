using Example.Webhosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.BoundedContext.Bar
{
    public class BarStartup : ApplicationStartup
    {
        public BarStartup(IConfiguration configuration, IHostingEnvironment environment) 
            : base(configuration, environment)
        {
        }
    }
}
