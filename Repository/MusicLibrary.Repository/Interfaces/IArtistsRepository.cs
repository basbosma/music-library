using MusicLibrary.Shared;
using MusicLibrary.Shared.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Repository.Interfaces
{
    public interface IArtistsRepository
    {
        Task<ApiResponse> CreateArtistsAsync(IEnumerable<ArtistDto> newArtists);
        Task<ApiResponse> ReadArtistsAsync(string bandName);
        Task<ApiResponse> UpdateArtistsAsync(IEnumerable<ArtistDto> updatedArtists);
        Task<ApiResponse> DeleteArtistAsync(int id);
    }
}
