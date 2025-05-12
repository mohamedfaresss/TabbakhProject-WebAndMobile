using System.ComponentModel.DataAnnotations;

namespace Models.ArabicDomain
{
    public class وصفة_المكونات
    {
        [Key]
        public int بطاقة_الوصفة { get; set; }

        public int بطاقة_المكون { get; set; }

        public double? كمية { get; set; }

        public المكونات? المكون { get; set; }

        public الوصفات? الوصفة { get; set; }
    }
}
