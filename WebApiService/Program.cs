using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel((config, options) =>
                {
                    options.AllowSynchronousIO = true; /*фикс ошибки сериализации данных https://github.com/graphql-dotnet/graphql-dotnet/issues/1116 */

                    var isUnix = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
                    if (isUnix)
                        options.ListenUnixSocket("/var/run/web_api.sock");
                    else
                        options.Listen(IPAddress.Loopback, 5011);
                })
                .UseStartup<Startup>();
    }
}
