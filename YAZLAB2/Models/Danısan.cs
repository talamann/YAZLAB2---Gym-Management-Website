using System.ComponentModel.DataAnnotations;

namespace YAZLAB2.Models
{
    public class Danısan
    {
       
        public int id { get; set; }
        public string adsoyad { get; set; }
        public string? hedefler { get; set; }
        public string? boy { get; set; }  
        public string? kilo { get; set; } 
        public string? bmi { get; set; }
        public string? yagorani { get; set; }
        
    }
}
