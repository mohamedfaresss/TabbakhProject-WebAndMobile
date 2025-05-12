using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ArabicDomain
{
    public class الوصفات
    {
        [Key]

        public int بطاقة_تعريف { get; set; }

        public string اسم { get; set; } = null!;

        public int الوقت { get; set; }

        public string وصف { get; set; } = null!;

        public string? طريقة_التحضير { get; set; }

        public التغذية? التغذية { get; set; }

        public ICollection<وصفة_المكونات> وصفة_المكونات { get; set; } = new List<وصفة_المكونات>();
    }
}
