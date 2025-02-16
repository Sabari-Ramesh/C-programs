using BankApplicationJWT.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankApplicationJWT.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure One-to-One relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Bank)
                .WithOne(b => b.User)
                .HasForeignKey<Bank>(b => b.UserId);

            // Ensure AccountNumber is unique and has a max length of 10
            modelBuilder.Entity<Bank>()
                .HasIndex(b => b.AccountNumber)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
