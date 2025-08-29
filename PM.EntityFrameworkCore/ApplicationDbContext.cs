using Microsoft.EntityFrameworkCore;
using PM.EntityFrameworkCore.Entities;

namespace PM.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
