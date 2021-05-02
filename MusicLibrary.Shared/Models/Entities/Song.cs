using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Shared.Models.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Shortname { get; set; }
        public int Bpm { get; set; }
        public int Duration { get; set; }
        public string Genre { get; set; }
        public string SpotifyId { get; set; }
        public string Album { get; set; }

        public Artist Artist { get; set; }
        public int ArtistId { get; set; }
    }
}
