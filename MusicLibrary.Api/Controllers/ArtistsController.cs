using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibrary.Repository.Interfaces;
using MusicLibrary.Shared;
using MusicLibrary.Shared.Models.Dtos;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CrmPwa.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistsRepository _artistsRepository;
        private readonly ILogger<ArtistsController> _logger;
        private readonly ApiResponse _invalidData;

        public ArtistsController(
            IArtistsRepository artistsRepository,
            ILogger<ArtistsController> logger)
        {
            _artistsRepository = artistsRepository;
            _logger = logger;
            _invalidData = new ApiResponse(Status400BadRequest, "Invalid Data");
        }

        [HttpGet]
        public async Task<ApiResponse> ReadAllArtistsAsync(string bandName)
        {
            return ModelState.IsValid ? await _artistsRepository.ReadArtistsAsync(bandName) : _invalidData;
        }

        [HttpPut]
        public async Task<ApiResponse> UpdateArtistsAsync([FromBody] IEnumerable<ArtistDto> updatedArtists)
        {
            return ModelState.IsValid ? await _artistsRepository.UpdateArtistsAsync(updatedArtists) : _invalidData;
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse> DeleteArtistAsync(int id)
        {
            return ModelState.IsValid ? await _artistsRepository.DeleteArtistAsync(id) : _invalidData;
        }

        [HttpPost]
        public async Task<ApiResponse> CreateArtistsAsync([FromBody] IEnumerable<ArtistDto> newArtists)
        {
            return ModelState.IsValid ? await _artistsRepository.CreateArtistsAsync(newArtists) : _invalidData;
        }
    }
}
