using CheckSUm.Entity;
using Microsoft.EntityFrameworkCore;

namespace CheckSUm.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AccountDetails> Accounts { get; set; }
    }
}
