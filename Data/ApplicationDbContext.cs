using AutoSphere.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace AutoSphere.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                                    : base(options){}

        protected ApplicationDbContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Features column to use PostgreSQL text[]
            modelBuilder.Entity<Vehicle>()
                .Property(v => v.Features)
                .HasColumnType("text[]");

            modelBuilder.Entity<Vehicle>()
                 .HasIndex(v => new { v.Make, v.Model, v.Year, v.Price });
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<SavedSearch> SavedSearches { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
    }
}

