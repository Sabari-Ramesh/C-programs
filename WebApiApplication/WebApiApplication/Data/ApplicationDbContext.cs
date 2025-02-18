using Microsoft.EntityFrameworkCore;
using WebApiApplication.Models.Entity;

namespace WebApiApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees{ get; set; }
    }
}
