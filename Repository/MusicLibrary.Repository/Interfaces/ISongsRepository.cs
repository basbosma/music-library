using MusicLibrary.Shared;
using MusicLibrary.Shared.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Repository.Interfaces
{
    public interface ISongsRepository
    {
        Task<ApiResponse> CreateSongsAsync(IEnumerable<SongDto> newSongs);
        Task<ApiResponse> ReadSongsAsync(string genre);
        Task<ApiResponse> UpdateSongsAsync(IEnumerable<SongDto> updatedSongs);
        Task<ApiResponse> DeleteSongAsync(int id);
    }
}
