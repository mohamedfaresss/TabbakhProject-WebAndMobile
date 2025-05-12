using System.ComponentModel.DataAnnotations;

namespace Models.ArabicDomain
{
    public class التغذية
    {
        [Key]
        public int بطاقة_الوصفة { get; set; }

        public double سعرات_حرارية_لكل100جم { get; set; }

        public double دهون_لكل100جم { get; set; }

        public double سكر_لكل100جم { get; set; }

        public double بروتين_لكل100جم { get; set; }

        public double كربوهيدرات_لكل100جم { get; set; }

        public string النوع { get; set; } = null!;

        public الوصفات? الوصفة { get; set; }
    }
}
