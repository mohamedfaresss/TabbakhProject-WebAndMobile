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
    class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.HasKey(i => i.Ingredient_Id);
            builder.Property(i => i.Ingredient_Name).IsRequired().HasMaxLength(100);

            builder.HasMany(i => i.Recipe_Ingredient)
                   .WithOne(ri => ri.Ingredient)
                   .HasForeignKey(ri => ri.Ingredient_Id);
        }
    }
}
