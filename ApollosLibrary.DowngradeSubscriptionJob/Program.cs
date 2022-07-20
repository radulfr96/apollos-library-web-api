using ApollosLibrary.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace ApollosLibrary.DowngradeSubscriptionJob
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder();

            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorageQueues();
                IConfiguration config = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddEnvironmentVariables()
                                .Build();

                b.Services.AddDbContext<ApollosLibraryContext>(options => options.UseNpgsql(config.GetSection("ConnectionString").Value, o => o.UseNodaTime()));
                b.Services.AddDbContext<ApollosLibraryContext>();
                b.Services.AddScoped<IClock>(c =>
                {
                    return SystemClock.Instance;
                });
            });

            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });

            var host = builder.Build();
            using (host)
            {
                var jobHost = host.Services.GetRequiredService(typeof(IJobHost)) as JobHost;
                await host.StartAsync();
                await jobHost.CallAsync("Run");
                await host.StopAsync();
            }
        }
    }
}
