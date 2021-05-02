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
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace MusicLibrary.Repository
{
    public class SongsRepository : ISongsRepository
    {
        private readonly MusicLibraryDbContext _dbContext;
        public SongsRepository(MusicLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> CreateSongsAsync(IEnumerable<SongDto> newSongs)
        {
            try
            {
                List<Song> songs = new();
                foreach (var song in newSongs)
                {
                    var artistId = await _dbContext.Artists.Where(x => x.Name.Equals(song.Artist)).Select(x => x.Id).FirstOrDefaultAsync();
                    if (artistId != 0)
                        songs.Add(new Song
                        {
                            Id = song.Id,
                            Name = song.Name,
                            Album = song.Album,
                            ArtistId = artistId,
                            Bpm = song.Bpm,
                            Duration = song.Duration,
                            Genre = song.Genre,
                            Shortname = song.Shortname,
                            SpotifyId = song.SpotifyId,
                            Year = song.Year
                        });
                }

                await _dbContext.Songs.AddRangeAsync(songs);

                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new NpgsqlException();
                }

                return new ApiResponse(Status201Created, $"Succesfully added {songs.Count} songs");
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }

        public async Task<ApiResponse> DeleteSongAsync(int id)
        {
            try
            {
                var song = await _dbContext.Songs.FirstOrDefaultAsync(x => x.Id == id);

                if (song != null)
                    _dbContext.Songs.Remove(song);

                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new NpgsqlException();
                }

                return new ApiResponse(Status200OK, $"Succesfully deleted song with id: {id}.");
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }

        public async Task<ApiResponse> ReadSongsAsync(string genre)
        {
            try
            {
                var songs = await _dbContext.Songs.Where(x => x.Genre.Contains(genre != null ? genre : ""))
                    .Select(y => new SongDto
                    {
                        Album = y.Album,
                        Artist = y.Artist.Name,
                        Bpm = y.Bpm,
                        Duration = y.Duration,
                        Genre = y.Genre,
                        Id = y.Id,
                        Name = y.Name,
                        Shortname = y.Shortname,
                        SpotifyId = y.SpotifyId,
                        Year = y.Year
                    }).ToListAsync();

                return new ApiResponse(Status200OK, $"Found {songs.Count} songs", songs);
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }

        public async Task<ApiResponse> UpdateSongsAsync(IEnumerable<SongDto> updatedSongs)
        {
            try
            {
                foreach (var updatedSong in updatedSongs)
                {
                    var oldSong = await _dbContext.Songs.FirstOrDefaultAsync(x => x.Id == updatedSong.Id);
                    if (oldSong != null)
                    {
                        var artistId = await _dbContext.Artists.Where(x => x.Name.Equals(updatedSong.Artist)).Select(x => x.Id).FirstOrDefaultAsync();
                        if (artistId != 0)
                        {
                            oldSong.Name = updatedSong.Name;
                            oldSong.Album = updatedSong.Album;
                            oldSong.ArtistId = artistId;
                            oldSong.Bpm = updatedSong.Bpm;
                            oldSong.Duration = updatedSong.Duration;
                            oldSong.Genre = updatedSong.Genre;
                            oldSong.Shortname = updatedSong.Shortname;
                            oldSong.SpotifyId = updatedSong.SpotifyId;
                            oldSong.Year = updatedSong.Year;
                        }
                    }
                }

                if (await _dbContext.SaveChangesAsync() <= 0)
                {
                    throw new NpgsqlException("Unable to make any changes.");
                }

                return new ApiResponse(Status202Accepted, $"Succesfully modified {updatedSongs.Count()} songs");
            }
            catch (Exception e)
            {
                return new ApiResponse(Status500InternalServerError, e.Message);
            }
        }
    }
}
