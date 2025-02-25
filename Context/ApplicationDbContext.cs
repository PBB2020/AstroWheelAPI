using AstroWheelAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AstroWheelAPI.Context
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<RecipeBook> RecipeBooks { get; set; }
        public DbSet<Island> Islands { get; set; }
        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<RecipeBook>()
                .HasKey(r => r.RecipeId);
        }
    }
}
