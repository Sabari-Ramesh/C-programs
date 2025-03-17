using Microsoft.EntityFrameworkCore;
using RoleBasedJWT.Model.Entity;

namespace RoleBasedJWT.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Account> accounts { get; set; }    
        public DbSet<User> users { get; set; }
    }
}
