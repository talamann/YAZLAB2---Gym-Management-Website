using System.ComponentModel.DataAnnotations;

namespace YAZLAB2.Models
{
    public class Antrenor
    {
        [Key]
        public int id { get; set; }
        public string adsoyad { get; set; }
       
        public string? uzmanliklar { get; set; }
        public string? deneyim { get; set; }
        public string? iletisim { get; set; }
    }
}
