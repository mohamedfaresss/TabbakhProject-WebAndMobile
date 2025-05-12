using DataAcess.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs.Food;

namespace DataAcess.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // this is pull
            // this is pull 2 
        }

        // Arabic tables
        public DbSet<Models.ArabicDomain.الوصفات> الوصفات { get; set; }
        public DbSet<Models.ArabicDomain.التغذية> التغذية { get; set; }
        public DbSet<Models.ArabicDomain.المكونات> المكونات { get; set; }
        public DbSet<Models.ArabicDomain.وصفة_المكونات> وصفة_المكونات { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DbSet<UserAllergy> UserAllergies { get; set; }
        public DbSet<UserDisease> UserDiseases { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Nutrition> Nutrition { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe_Ingredient> Recipe_Ingredient { get; set; }
        public DbSet<RecipeRawDTO> RecipeRawDTO { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Models.ArabicDomain.وصفة_المكونات>()
    .HasKey(x => new { x.بطاقة_الوصفة, x.بطاقة_المكون });

            builder.Entity<Models.ArabicDomain.وصفة_المكونات>()
                .HasOne(x => x.الوصفة)
                .WithMany(r => r.وصفة_المكونات)
                .HasForeignKey(x => x.بطاقة_الوصفة);

            builder.Entity<Models.ArabicDomain.وصفة_المكونات>()
                .HasOne(x => x.المكون)
                .WithMany(i => i.وصفة_المكونات)
                .HasForeignKey(x => x.بطاقة_المكون);

            builder.Entity<Models.ArabicDomain.التغذية>()
                .HasKey(x => x.بطاقة_الوصفة);

            builder.Entity<Models.ArabicDomain.التغذية>()
                .HasOne(x => x.الوصفة)
                .WithOne(r => r.التغذية)
                .HasForeignKey<Models.ArabicDomain.التغذية>(x => x.بطاقة_الوصفة);

            base.OnModelCreating(builder);
            builder.Entity<FavoriteRecipe>()
    .HasKey(fr => new { fr.UserId, fr.RecipeId });

            builder.Entity<FavoriteRecipe>()
                .HasOne(fr => fr.User)
                .WithMany(u => u.FavoriteRecipes)
                .HasForeignKey(fr => fr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteRecipe>()
                .HasOne(fr => fr.Recipe)
                .WithMany(r => r.FavoritedBy)
                .HasForeignKey(fr => fr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Apply separate configuration classes
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RecipeConfiguration());
            builder.ApplyConfiguration(new NutritionConfiguration());
            builder.ApplyConfiguration(new IngredientConfiguration());
            builder.ApplyConfiguration(new RecipeIngredientConfiguration());
            builder.Entity<RecipeRawDTO>().HasNoKey();
        }

    }
}
