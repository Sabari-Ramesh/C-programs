using EncryptionDemo.Entity;
using Microsoft.EntityFrameworkCore;

namespace EncryptionDemo.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
