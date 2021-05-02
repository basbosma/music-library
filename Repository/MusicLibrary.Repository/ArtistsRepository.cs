using Microsoft.EntityFrameworkCore;
using MusicLibrary.Repository.Interfaces;
using MusicLibrary.Repository.Shared.MusicLibraryDb;
using MusicLibrary.Shared;
using MusicLibrary.Shared.Models.Dtos;
using MusicLibrary.Shared.Models.Entities;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MusicLibrary.Repository
{
    public class ArtistsRepository : IArtistsRepository
    {
        private readonly MusicLibraryDbContext _dbContext;
        public ArtistsRepository(MusicLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> CreateArtistsAsync(IEnumerable<ArtistDto> newArtists)
        {
            try
            {
                List<Artist> artists = new();
                foreach (var artist in newArtists)
                {
                    artists.Add(new Artist
                    {
                        Id = artist.Id,
                        Name = artist.Name
                    });
                }

                await _dbContext.Artists.AddRangeAsync(artists);

                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new NpgsqlException();
                }

                return new ApiResponse(Status201Created, $"Succesfully added {artists.Count} artists");
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }

        public async Task<ApiResponse> DeleteArtistAsync(int id)
        {
            try
            {
                var artist = await _dbContext.Artists.FirstOrDefaultAsync(x => x.Id == id);

                if (artist != null)
                    _dbContext.Artists.Remove(artist);

                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new NpgsqlException();
                }

                return new ApiResponse(Status200OK, $"Succesfully deleted artist with id: {id}.");
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }

        public async Task<ApiResponse> ReadArtistsAsync(string bandName)
        {
            try
            {
                bandName = string.IsNullOrEmpty(bandName) ? "" : bandName.ToLower();
                var artists = await _dbContext.Artists.Where(x => x.Name.ToLower().Contains(bandName))
                    .Select(x => new ArtistDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToListAsync();

                return new ApiResponse(Status200OK, $"Found {artists.Count} artist{(artists.Count > 1 ? 's' : "")}", artists);
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }

        public async Task<ApiResponse> UpdateArtistsAsync(IEnumerable<ArtistDto> updatedArtists)
        {
            try
            {
                foreach (var updatedArtist in updatedArtists)
                {
                    var oldArtist = await _dbContext.Artists.FirstOrDefaultAsync(x => x.Id == updatedArtist.Id);
                    if (oldArtist != null)
                        oldArtist.Name = updatedArtist.Name;
                }

                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new NpgsqlException("Unable to make any changes.");
                }

                return new ApiResponse(Status202Accepted, $"Succesfully modified {updatedArtists.Count()} artists");
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }
    }
}
