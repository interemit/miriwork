using Example.Webhosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.BoundedContext.Bar
{
    public class BarStartup : ApplicationStartup
    {
        public BarStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
