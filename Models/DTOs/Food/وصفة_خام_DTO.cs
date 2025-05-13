using System.Collections.Generic;

namespace Models.DTOs.Food.Arabic
{
    public class وصفة_خام_DTO
    {
        public int بطاقة_تعريف { get; set; }
        public string اسم { get; set; } = null!;
        public string وصف { get; set; } = null!;
        public string? طريقة_التحضير { get; set; }
        public string الوقت { get; set; } = null!; // تغيير من int إلى string

        // خصائص التغذية
        public string النوع { get; set; } = null!;
        public double سعرات_حرارية_لكل100جم { get; set; }
        public double دهون_لكل100جم { get; set; }
        public double سكر_لكل100جم { get; set; }
        public double بروتين_لكل100جم { get; set; }
        public double كربوهيدرات_لكل100جم { get; set; }

        // خصائص المكونات
        public string اسم_المكون { get; set; } = null!;
    }
}