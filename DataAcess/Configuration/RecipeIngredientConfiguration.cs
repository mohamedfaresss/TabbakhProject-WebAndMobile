using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Configuration
{
    class RecipeIngredientConfiguration : IEntityTypeConfiguration<Recipe_Ingredient>
    {
        public void Configure(EntityTypeBuilder<Recipe_Ingredient> builder)
        {
            builder.HasKey(ri => new { ri.RecipeId, ri.Ingredient_Id });

            builder.Property(ri => ri.Amount).HasMaxLength(50);
        }
    }
}
