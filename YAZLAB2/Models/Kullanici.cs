
using System.ComponentModel.DataAnnotations;

namespace YAZLAB2.Models
{
    public class Kullanici
    {
        public Kullanici() { }

        
        public int id {get; set;} 
        public string eposta { get; set; }
        public string adsoyad { get; set; }
        public string sifre { get; set; }
        public string rol { get; set; }

    }
}
