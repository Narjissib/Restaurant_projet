using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myapp.Models;

namespace myapp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<myapp.Models.Categorie>? Categorie { get; set; }
        public DbSet<myapp.Models.Plat>? Plat { get; set; }
        public DbSet<myapp.Models.PlatSignature>? PlatSignature { get; set; }
    }
}