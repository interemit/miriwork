using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Example.Webhosting
{
    public class ApplicationWebHost
    {
        public static void Run(string[] args) => BuildWebHost(args).Run();

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseAutofacMultitenantRequestServices(() => Startup.ApplicationContainer)
                .UseStartup<Startup>()
                .Build();
    }
}
