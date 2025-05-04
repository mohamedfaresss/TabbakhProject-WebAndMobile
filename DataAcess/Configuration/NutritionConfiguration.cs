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
    class NutritionConfiguration : IEntityTypeConfiguration<Nutrition>
    {
        public void Configure(EntityTypeBuilder<Nutrition> builder)
        {
            builder.HasKey(n => n.Recipe_Id);

            builder.Property(n => n.Type).HasMaxLength(50);
        }
    }
}
