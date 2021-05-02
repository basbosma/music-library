using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicLibrary.DatabaseSeeder.Services.Interfaces;
using MusicLibrary.Repository.Shared.MusicLibraryDb;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.DatabaseSeeder
{
    public static class DataAccessLayer
    {
        public static async Task SeedMusicLibrary(IServiceCollection services)
        {
            Log.Information("Starting Music Library seeding");
            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    //var context = scope.ServiceProvider.GetService<MusicLibraryDbContext>();
                    //context.Database.Migrate();

                    var songsApiClient = scope.ServiceProvider.GetRequiredService<ISongsApiClient>();
                    var artistsApiClient = scope.ServiceProvider.GetRequiredService<IArtistsApiClient>();

                    var artists = await artistsApiClient.GetArtists();
                    var songs = await songsApiClient.GetSongsAsync();
                }
            }
        }
    }
}
