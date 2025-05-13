namespace Models.ArabicDomins
{
    public class المكونات
    {
        public int بطاقة_تعريف { get; set; }
        public string اسم_المكون { get; set; } = null!;
        public bool النوع { get; set; }

        public List<وصفة_المكونات> وصفة_المكونات { get; set; } = new();
    }
}