using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace Epam.Demo.App.GrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IWebHostBuilder CreateHostBuilder(string[] args)
        {
            var host = new WebHostBuilder().UseKestrel()
                 .ConfigureServices(services =>
                 {
                     var serviceProvider = services.BuildServiceProvider();
                     var env = serviceProvider.GetService<IHostingEnvironment>();
                     services.AddSingleton<IConfiguration>(provider =>
                     {
                         return
                                 new ConfigurationBuilder()
                                     .SetBasePath(env.ContentRootPath)
                                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                                     .AddEnvironmentVariables().Build();
                     });

                     services.AddDefaultRepository();
                     services.AddCastleInterception();
                 }).UseStartup<Startup>();
            return host;
        }
    }
}