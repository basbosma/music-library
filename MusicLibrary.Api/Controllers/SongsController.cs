using System.Threading.Tasks;
using CrmPwa.Shared.NET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibrary.Repository.Interfaces;
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
        public async Task<ApiResponse> GetAllSongsAsync()
        {
            return ModelState.IsValid ? null : _invalidData;
        }
    }
}
