using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.ArabicDomain;

namespace DataAcess.Configuration
{
    class وصفة_المكوناتConfiguration : IEntityTypeConfiguration<وصفة_المكونات>
    {
        public void Configure(EntityTypeBuilder<وصفة_المكونات> builder)
        {
            builder.ToTable("وصفة_المكونات");

            builder.HasKey(wm => new { wm.بطاقة_تعريف_الوصفة, wm.بطاقة_تعريف_المكون });
            builder.Property(wm => wm.بطاقة_تعريف_الوصفة).HasColumnName("بطاقة تعريف الوصفة");
            builder.Property(wm => wm.بطاقة_تعريف_المكون).HasColumnName("بطاقة تعريف المكون");
            builder.Property(wm => wm.كمية).HasMaxLength(50).HasColumnName("كمية");
        }
    }
}