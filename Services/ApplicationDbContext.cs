using Microsoft.EntityFrameworkCore;
using project.Models;

namespace project.Services
{
    public class ApplicationDbContext : DbContext

    {

        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public object User { get; internal set; }
    }
   
}
