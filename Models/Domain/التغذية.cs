namespace Models.ArabicDomain
{
    public class التغذية
    {
        public int بطاقة_تعريف_الوصفة { get; set; }
        public string النوع { get; set; } = null!;
        public double سعرات_حرارية_لكل100جم { get; set; }
        public double دهون_لكل100جم { get; set; }
        public double سكر_لكل100جم { get; set; }
        public double بروتين_لكل100جم { get; set; }
        public double كربوهيدرات_لكل100جم { get; set; }

        public الوصفات الوصفة { get; set; } = null!;
    }
}