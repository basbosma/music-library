using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Shared.Helpers
{
    public static class StartupHelpers
    {
        public static void RegisterDbContexts<TMusicLibraryDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TMusicLibraryDbContext : DbContext
        {

            var connectionStringMusicLibrary = configuration.GetConnectionString("MusicLibrary");

            var migrationsAssembly = "MusicLibrary.Repository.Shared";

            // Config DB for identity
            services.AddDbContext<TMusicLibraryDbContext>(options =>
            {
                options.UseNpgsql(connectionStringMusicLibrary, sql =>
                {
                    sql.MigrationsAssembly(migrationsAssembly);
                    sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(5), null);
                });
            });
        }
    }
}
