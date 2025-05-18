using Models.Domain;

namespace Models.ArabicDomain
{
    public class الوصفات
    {
        public int بطاقة_تعريف { get; set; }
        public string اسم { get; set; } = null!;
        public string وصف { get; set; } = null!;
        public string? طريقة_التحضير { get; set; }
        public string الوقت { get; set; } = null!; // يُعيَّن إلى "دقائق" في قاعدة البيانات
        public string? رابط_الصورة { get; set; }
        public التغذية التغذية { get; set; } = null!;
        public List<وصفة_المكونات> وصفة_المكونات { get; set; } = new();

    }
}