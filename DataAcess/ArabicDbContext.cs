using DataAcess.Configuration;
using Microsoft.EntityFrameworkCore;
using Models.ArabicDomain;
using Models.DTOs.Food.Arabic; 

namespace DataAcess.DbContexts
{
    public class ArabicDbContext : DbContext
    {
        public ArabicDbContext(DbContextOptions<ArabicDbContext> options) : base(options)
        {
        }

        public DbSet<الوصفات> الوصفات { get; set; }
        public DbSet<التغذية> التغذية { get; set; }
        public DbSet<المكونات> المكونات { get; set; }
        public DbSet<وصفة_المكونات> وصفة_المكونات { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new وصفاتConfiguration());
            modelBuilder.ApplyConfiguration(new تغذيةConfiguration());
            modelBuilder.ApplyConfiguration(new مكوناتConfiguration());
            modelBuilder.ApplyConfiguration(new وصفة_المكوناتConfiguration());

            modelBuilder.Entity<وصفة_خام_DTO>().HasNoKey();
        }

      
    }
}