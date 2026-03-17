using ITGN350_Assignment.Models;
using Microsoft.EntityFrameworkCore;

namespace ITGN350_Assignment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.Property(u => u.FullName).HasMaxLength(100);
                entity.Property(u => u.Email).HasMaxLength(150);
                entity.Property(u => u.PasswordHash).HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}