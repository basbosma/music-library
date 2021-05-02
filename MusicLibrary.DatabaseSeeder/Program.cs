using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicLibrary.DatabaseSeeder.Services;
using MusicLibrary.DatabaseSeeder.Services.Interfaces;
using MusicLibrary.Repository.Shared.MusicLibraryDb;
using MusicLibrary.Shared.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MusicLibrary.DatabaseSeeder
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\identityserver.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            var Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);

            services.RegisterDbContexts<MusicLibraryDbContext>(Configuration);

            services.AddLogging(configure => configure.AddSerilog());

            services.AddHttpClient();

            services.AddTransient<IArtistsApiClient, ArtistsApiClient>();
            services.AddTransient<ISongsApiClient, SongsApiClient>();

            await DataAccessLayer.SeedMusicLibrary(services);
        }
    }
}
