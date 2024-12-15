using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using Uniqlo.Models;

namespace Uniqlo.DataAccess
{
    public class UniqloDbContext : IdentityDbContext<User>
    {

        public UniqloDbContext(DbContextOptions<UniqloDbContext> options) : base(options) { }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
        public DbSet<ProductRatings> ProductRatings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
