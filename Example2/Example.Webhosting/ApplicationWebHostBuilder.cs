using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Example.Webhosting
{
    public static class ApplicationWebHostBuilder
    {
        public static IWebHostBuilder CreateWebHostBuilder<TStartup>(string[] args) where TStartup : ApplicationStartup =>
            WebHost.CreateDefaultBuilder(args)
                .UseAutofacMultitenantRequestServices(() => ApplicationStartup.ApplicationContainer)
                .UseStartup<TStartup>();
    }
}
