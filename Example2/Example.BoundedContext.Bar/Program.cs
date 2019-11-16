using Example.Webhosting;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Example.BoundedContext.Bar
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            ApplicationWebHostBuilder.CreateWebHostBuilder<BarStartup>(args);
    }
}
