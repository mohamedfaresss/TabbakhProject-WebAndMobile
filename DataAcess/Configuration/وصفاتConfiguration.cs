using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.ArabicDomain;

namespace DataAcess.Configuration
{
    class وصفاتConfiguration : IEntityTypeConfiguration<الوصفات>
    {
        public void Configure(EntityTypeBuilder<الوصفات> builder)
        {
            builder.ToTable("الوصفات");

            builder.HasKey(w => w.بطاقة_تعريف);
            builder.Property(w => w.بطاقة_تعريف).HasColumnName("بطاقة تعريف");
            builder.Property(w => w.اسم).IsRequired().HasMaxLength(100).HasColumnName("اسم");
            builder.Property(w => w.الوقت).HasMaxLength(50).HasColumnName("دقائق");
            builder.Property(w => w.وصف).HasMaxLength(500).HasColumnName("وصف");
            builder.Property(w => w.طريقة_التحضير).HasColumnName("طريقة التحضير");

            builder.HasOne(w => w.التغذية)
                   .WithOne(t => t.الوصفة)
                   .HasForeignKey<التغذية>(t => t.بطاقة_تعريف_الوصفة);

            builder.HasMany(w => w.وصفة_المكونات)
                   .WithOne(wm => wm.الوصفة)
                   .HasForeignKey(wm => wm.بطاقة_تعريف_الوصفة);
        }
    }
}