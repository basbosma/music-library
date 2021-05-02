using Microsoft.Extensions.Configuration;
using MusicLibrary.DatabaseSeeder.Services.Interfaces;
using MusicLibrary.Shared.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.DatabaseSeeder.Services
{
    public class ArtistsApiClient : IArtistsApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ArtistsApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Artist>> GetArtists()
        {
            var apiResponse = await _httpClient.GetStringAsync(_configuration.GetValue<string>("FileLocations:Artists"));

            return JsonConvert.DeserializeObject<IEnumerable<Artist>>(apiResponse);
        }
    }
}
