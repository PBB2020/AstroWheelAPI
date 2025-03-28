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
        public DbSet<InventoryMaterial> InventoryMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Egyedi index az ApplicationUser Email mezőjére
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // RecipeBook elsődleges kulcsának beállítása
            builder.Entity<RecipeBook>()
                .HasKey(r => r.RecipeBookId);

            // Kapcsolat a Player és RecipeBook között (1 Player → 1 RecipeBook)
            builder.Entity<Player>()
                .HasOne(p => p.RecipeBook)
                .WithOne() 
                .HasForeignKey<Player>(p => p.RecipeBookId); // Külső kulcs a Player-ben

            // Kapcsolótábla konfigurálása
            builder.Entity<InventoryMaterial>()
                .HasKey(im => new { im.InventoryId, im.MaterialId });

            builder.Entity<InventoryMaterial>()
                .HasOne(im => im.Inventory)
                .WithMany(i => i.InventoryMaterials)
                .HasForeignKey(im => im.InventoryId);

            builder.Entity<InventoryMaterial>()
                .HasOne(im => im.Material)
                .WithMany(m => m.InventoryMaterials)
                .HasForeignKey(im => im.MaterialId);

            // Kötelező 1:1 kapcsolat a Player és a Character között
            builder.Entity<Player>()
                .HasOne(p => p.Character)
                .WithMany()
                .HasForeignKey(p => p.CharacterId)
                .IsRequired(); // Kötelező kapcsolat
        }
    }
}
