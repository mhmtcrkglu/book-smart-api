using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BookSmart.Api;

public static class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateWebHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureAppConfiguration(AddConfiguration);
                webBuilder.UseStartup<Startup>();
            });
    }

    private static void AddConfiguration(
        IConfigurationBuilder configurationBuilder)
    {
        var configPath = Path.Join(Directory.GetCurrentDirectory(), "config");
        configurationBuilder
            .AddEnvironmentVariables()
            .AddJsonFile($"{configPath}/appsettings.json");
    }
}