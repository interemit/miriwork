using Example.Webhosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.BoundedContext.Foo
{
    public class FooStartup : ApplicationStartup
    {
        public FooStartup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
