
namespace Models.DTOs.Food.Arabic
{
    public class وصفة_مع_تغذية_DTO
    {
        public DateTime تاريخ_الإضافة;

        public int بطاقة_تعريف { get; set; }
        public string اسم_الوصفة { get; set; } = null!;
        public string الوصف { get; set; } = null!;
        public string? طريقة_التحضير { get; set; }
        public string الوقت { get; set; } = null!; // تغيير من int إلى string
        public double سعرات_حرارية_لكل100ج { get; set; }
        public double دهون_لكل100ج { get; set; }
        public double سكر_لكل100ج { get; set; }
        public double بروتين_لكل100ج { get; set; }
        public double كربوهيدرات_لكل100 { get; set; }
        public string النوع { get; set; } = null!;
        public List<مكون_مع_الكمية_DTO> المكونات { get; set; } = new();
        public bool مفضل { get; set; } = false;
        public bool في_السلة { get; set; } = false;
    }
}