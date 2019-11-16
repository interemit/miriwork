using Example.Webhosting;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Example.BoundedContext.Foo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            ApplicationWebHostBuilder.CreateWebHostBuilder<FooStartup>(args);
    }
}
