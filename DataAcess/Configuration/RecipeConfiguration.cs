using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Configuration
{
    class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(r => r.Recipe_Id);

            builder.Property(r => r.Recipe_Name).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Time).HasMaxLength(50);
            builder.Property(r => r.Description).HasMaxLength(500);

            builder.HasOne(r => r.Nutrition)
                   .WithOne(n => n.Recipe)
                   .HasForeignKey<Nutrition>(n => n.Recipe_Id);

            builder.HasMany(r => r.Recipe_Ingredient)
                   .WithOne(ri => ri.Recipe)
                   .HasForeignKey(ri => ri.RecipeId);
        }
    }
}
