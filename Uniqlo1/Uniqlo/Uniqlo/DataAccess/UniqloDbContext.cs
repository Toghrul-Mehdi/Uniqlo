using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Uniqlo.Models;

namespace Uniqlo.DataAccess
{
    public class UniqloDbContext : DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public UniqloDbContext(DbContextOptions<UniqloDbContext> options) : base(options) { }
    }
}
