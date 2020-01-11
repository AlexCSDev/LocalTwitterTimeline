using Microsoft.EntityFrameworkCore;
using TweetDatabase.Models;

namespace TweetDatabase
{
    public class TweetDbContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tweets.db").EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Tweet>()
                .HasMany(t => t.Media)
                .WithOne(m => m.Tweet)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tweets)
                .WithOne(t => t.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Media>()
                .Property(m => m.MediaType)
                .HasConversion<int>();
        }
    }
}