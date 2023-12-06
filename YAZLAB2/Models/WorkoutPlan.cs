using System.ComponentModel.DataAnnotations;

namespace YAZLAB2.Models
{
    public class WorkoutPlan
    {
        [Key]
        public int id { get; set; }
        public string danisan_adsoyad { get; set; }
        public string hedefler { get; set; }
    }
}
