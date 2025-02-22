using EmailDemo.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace EmailDemo.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<UserInfo> UserInfo { get; set; }
    }
}
