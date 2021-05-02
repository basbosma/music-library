using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicLibrary.DatabaseSeeder.Services.Interfaces;
using MusicLibrary.Repository.Shared.MusicLibraryDb;
using MusicLibrary.Shared.Models.Entities;
using Npgsql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Common;
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
                    var context = scope.ServiceProvider.GetService<MusicLibraryDbContext>();
                    context.Database.Migrate();

                    var songsApiClient = scope.ServiceProvider.GetRequiredService<ISongsApiClient>();
                    var artistsApiClient = scope.ServiceProvider.GetRequiredService<IArtistsApiClient>();

                    var artistsDto = await artistsApiClient.GetArtists();
                    var songsDto = await songsApiClient.GetSongsAsync();
                    var songs = songsDto.ToList().Where(x => x.Year < 2016);

                    List<Artist> artists = new();

                    foreach (var artistDto in artistsDto)
                    {
                        artists.Add(new Artist
                        {
                            Id = artistDto.Id,
                            Name = artistDto.Name,
                            Songs = songs.Where(x => x.Artist == artistDto.Name).Select(x => new Song
                            {
                                Id = x.Id,
                                Album = x.Album,
                                ArtistId = artistDto.Id,
                                Bpm = x.Bpm,
                                Duration = x.Duration,
                                Genre = x.Genre,
                                Name = x.Name,
                                Shortname = x.Shortname,
                                SpotifyId = x.SpotifyId,
                                Year = x.Year
                            }).ToList()
                        });
                    };

                    await context.Artists.AddRangeAsync(artists.Where(x => x.Songs.Any(x => x.Genre.Contains("Metal"))));

                    if (await context.SaveChangesAsync() <= 0)
                    {
                        Log.Error(new NpgsqlException(), "Could not store data in database");
                    }
                }
            }
        }
    }
}
