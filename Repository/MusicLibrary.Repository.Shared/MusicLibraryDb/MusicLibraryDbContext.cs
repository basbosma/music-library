using Microsoft.EntityFrameworkCore;
using MusicLibrary.Shared.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibrary.Repository.Shared.MusicLibraryDb
{
    public class MusicLibraryDbContext : DbContext
    {
        public MusicLibraryDbContext(DbContextOptions<MusicLibraryDbContext> options)
           : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging")
                optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Artist>()
                .HasIndex(x => x.Id)
                .IsUnique();
            builder.Entity<Artist>()
                .HasIndex(x => x.Name)
                .IsUnique();

            builder.Entity<Song>()
                .HasIndex(x => x.Id)
                .IsUnique();
            builder.Entity<Song>()
                .HasIndex(x => x.Name)
                .IsUnique();
            builder.Entity<Song>()
                .HasOne(x => x.Artist)
                .WithMany(x => x.Songs)
                .HasForeignKey(x => x.ArtistId);
        }
    }
}
