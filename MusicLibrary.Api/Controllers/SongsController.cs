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
    public class SongsController : ControllerBase
    {
        private readonly ISongsRepository _songsRepository;
        private readonly ILogger<SongsController> _logger;
        private readonly ApiResponse _invalidData;

        public SongsController(
            ISongsRepository songsRepository,
            ILogger<SongsController> logger)
        {
            _songsRepository = songsRepository;
            _logger = logger;
            _invalidData = new ApiResponse(Status400BadRequest, "Invalid Data");
        }

        [HttpGet]
        public async Task<ApiResponse> ReadAllSongsAsync(string genre)
        {
            return ModelState.IsValid ? await _songsRepository.ReadSongsAsync(genre) : _invalidData;
        }

        [HttpPut]
        public async Task<ApiResponse> UpdateSongsAsync([FromBody] IEnumerable<SongDto> updatedSongs)
        {
            return ModelState.IsValid ? await _songsRepository.UpdateSongsAsync(updatedSongs) : _invalidData;
        }

        [HttpDelete("{id:int}")]
        public async Task<ApiResponse> DeleteSongAsync(int id)
        {
            return ModelState.IsValid ? await _songsRepository.DeleteSongAsync(id) : _invalidData;
        }

        [HttpPost]
        public async Task<ApiResponse> CreateSongsAsync([FromBody] IEnumerable<SongDto> newSongs)
        {
            return ModelState.IsValid ? await _songsRepository.CreateSongsAsync(newSongs) : _invalidData;
        }
    }
}
