using Example.Webhosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.BoundedContext.Foo
{
    public class FooStartup : ApplicationStartup
    {
        public FooStartup(IConfiguration configuration, IHostingEnvironment environment) 
            : base(configuration, environment)
        {
        }
    }
}
