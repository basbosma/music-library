using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MusicLibrary.Repository.Shared.MusicLibraryDb;
using System;
using System.IO;
using System.Reflection;

namespace MusicLibrary.Repository.Shared
{
    public class MusicLibraryContextFactory : IDesignTimeDbContextFactory<MusicLibraryDbContext>
    {
        public MusicLibraryDbContext CreateDbContext(string[] args)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            var configuration = builder.Build();

            var dbContextBuilder = new DbContextOptionsBuilder<MusicLibraryDbContext>();

            var connectionString = configuration.GetConnectionString("MusicLibrary");

            dbContextBuilder.UseNpgsql(connectionString);

            return new MusicLibraryDbContext(dbContextBuilder.Options);
        }
    }
}
