using MusicLibrary.Repository.Interfaces;
using MusicLibrary.Repository.Shared.MusicLibraryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Repository
{
    public class SongsRepository : ISongsRepository
    {
        private readonly MusicLibraryDbContext _dbContext;
        public SongsRepository(MusicLibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
