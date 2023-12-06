using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YAZLAB2.Models;

namespace YAZLAB2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly YazlabDbContext _context;

        public HomeController(ILogger<HomeController> logger,YazlabDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> RegisterUser(Kullanici kullanici)
        {

            if (ModelState.IsValid)
            {
                _context.kullanicilar.Add(kullanici);
                await _context.SaveChangesAsync();
                Danısan d = new Danısan();
                d.id = kullanici.id;
                d.adsoyad = kullanici.adsoyad;
                _context.danisan.Add(d);
                await _context.SaveChangesAsync();
                return View("Login");

            }
            else
                return View("Login");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public async Task<IActionResult> LoginUser(Kullanici k)
        {
            if (_context.kullanicilar.FindAsync(k.id).Result.rol.Equals("danisan")&&
                _context.kullanicilar.FindAsync(k.id).Result.sifre.Equals(k.sifre))
            {
                SharedClass.id = k.id;
                SharedClass.rol = k.rol;
                SharedClass.loggedin = true;
                Danısan d =  _context.danisan.Find(k.id);

                return View("/Views/Shared/DanisanUser/DanisanIndex.cshtml",model: d);
            }
            else if (_context.kullanicilar.FindAsync(k.id, k.sifre).Result.rol.Equals("antrenor"))
            {
                SharedClass.id = k.id;
                SharedClass.rol = k.rol;
                SharedClass.loggedin = true;
                return View("Antrenorler\\Index");
            }
            else if (_context.kullanicilar.FindAsync(k.id, k.sifre).Result.rol.Equals("admin"))
            {
                SharedClass.id = k.id;
                SharedClass.rol = k.rol;
                SharedClass.loggedin = true;
                return View(Index());
            }
            else
                return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}