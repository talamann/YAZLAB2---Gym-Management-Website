using Microsoft.EntityFrameworkCore;

namespace YAZLAB2.Models
{
    [Keyless]
    public class Eslesmeler
    {
        public int danisan_id { get; set; }
        public string danisan_adsoyad { get; set; }
        public int antrenor_id { get; set; } 
        public string antrenor_adsoyad { get; set; }
        public string alan { get; set; }
        public Eslesmeler() { }
    }
}
