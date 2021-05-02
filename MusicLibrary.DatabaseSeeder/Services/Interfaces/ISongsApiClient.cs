using MusicLibrary.Shared.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.DatabaseSeeder.Services.Interfaces
{
    public interface ISongsApiClient
    {
        Task<IEnumerable<Song>> GetSongsAsync();
    }
}
