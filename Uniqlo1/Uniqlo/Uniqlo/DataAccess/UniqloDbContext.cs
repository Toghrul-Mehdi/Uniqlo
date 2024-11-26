using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Uniqlo.Models;

namespace Uniqlo.DataAccess
{
    public class UniqloDbContext : DbContext
    {

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public UniqloDbContext(DbContextOptions<UniqloDbContext> options) : base(options) { }
    }
}
