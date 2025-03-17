using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions): base(dbContextOptions)
        {   
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data for difficulties
            //easy medium hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("9fe7d108-36bb-45df-baef-49416ec73aee"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("c9a7b395-1447-46cc-832e-b22127126c01"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("6278ab91-3902-4abd-8299-3cc108b23077") ,
                    Name = "Hard"
                }
            };
            // Seed data to database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("12686546-0ffa-4828-b0d5-272784554c33"),
                    Name = "Ontario",
                    Code = "ON",
                    RegionImageUrl = ""
                },
                new Region
                {
                    Id = Guid.Parse("987bf8c2-1401-4313-998b-249fe6244396"),
                    Name = "Alberta",
                    Code = "AB",
                    RegionImageUrl = ""
                },
                new Region
                {
                    Id = Guid.Parse("c10712b0-f9f8-4d11-8185-78e18b5f65da"),
                    Name = "British Columbia",
                    Code = "BC",
                    RegionImageUrl = ""
                },
            };
            //Seed to db
            modelBuilder.Entity<Region>().HasData(regions);
    

        }

    }
}
