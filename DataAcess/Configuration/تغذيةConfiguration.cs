using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.ArabicDomain;

namespace DataAcess.Configuration
{
    class تغذيةConfiguration : IEntityTypeConfiguration<التغذية>
    {
        public void Configure(EntityTypeBuilder<التغذية> builder)
        {
            builder.ToTable("التغذية");

            builder.HasKey(t => t.بطاقة_تعريف_الوصفة);
            builder.Property(t => t.بطاقة_تعريف_الوصفة).HasColumnName("بطاقة تعريف");
            builder.Property(t => t.النوع).HasMaxLength(50).HasColumnName("النوع");
            builder.Property(t => t.سعرات_حرارية_لكل100جم).HasColumnName("السعرات الحرارية");
            builder.Property(t => t.دهون_لكل100جم).HasColumnName("سمين");
            builder.Property(t => t.سكر_لكل100جم).HasColumnName("سكر");
            builder.Property(t => t.بروتين_لكل100جم).HasColumnName("بروتين");
            builder.Property(t => t.كربوهيدرات_لكل100جم).HasColumnName("الكربوهيدرات");
        }
    }
}