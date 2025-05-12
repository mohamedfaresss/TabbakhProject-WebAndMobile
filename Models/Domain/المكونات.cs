using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ArabicDomain
{
    public class المكونات
    {
        [Key]
        public int بطاقة_المكون { get; set; }

        public string? اسم_المكون { get; set; }

        public ICollection<وصفة_المكونات> وصفة_المكونات { get; set; } = new List<وصفة_المكونات>();
    }
}
