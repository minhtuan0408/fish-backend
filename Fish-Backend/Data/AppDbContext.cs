using Fish_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Fish_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }

        public DbSet<Fish> Fishes { get; set; }

        // chuyển kiểu int sang string tự đôn jg
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fish>()
                .Property(f => f.type)
                .HasConversion<string>(); // ⚠️ Rất quan trọng
        }

    }
}

