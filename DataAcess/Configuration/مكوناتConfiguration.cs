using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.ArabicDomain;

namespace DataAcess.Configuration
{
    class مكوناتConfiguration : IEntityTypeConfiguration<المكونات>
    {
        public void Configure(EntityTypeBuilder<المكونات> builder)
        {
            builder.ToTable("المكونات");

            builder.HasKey(m => m.بطاقة_تعريف);
            builder.Property(m => m.بطاقة_تعريف).HasColumnName("بطاقة تعريف");
            builder.Property(m => m.اسم_المكون).IsRequired().HasMaxLength(100).HasColumnName("المكونات");
            builder.Property(m => m.النوع).HasColumnName("النوع");

            builder.HasMany(m => m.وصفة_المكونات)
                   .WithOne(wm => wm.المكون)
                   .HasForeignKey(wm => wm.بطاقة_تعريف_المكون);
        }
    }
}