using System.ComponentModel.DataAnnotations;

namespace YAZLAB2.Models
{
    public class DietPlan
    {
        [Key]
        public int id { get; set; }
        public int danisan_id { get; set; }
        public string danisan_adsoyad { get; set; }
        public string hedefler { get; set; }
    }
}
