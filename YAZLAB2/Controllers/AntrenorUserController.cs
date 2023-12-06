using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using YAZLAB2;
using YAZLAB2.Models;

namespace YAZLAB2.Controllers
{
    public class AntrenorUserController : Controller
    {
        private readonly YazlabDbContext _context;

        public AntrenorUserController(YazlabDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> RegisterUser(Kullanici kullanici)
        {
            kullanici.rol = "antrenor";

            if (ModelState.IsValid)
            {
                _context.kullanicilar.Add(kullanici);
                await _context.SaveChangesAsync();
                Antrenor a = new Antrenor();
                a.id = kullanici.id;
                a.adsoyad = kullanici.adsoyad;
                _context.antrenorler.Add(a);
                await _context.SaveChangesAsync();
                return View("Details",a);
            }
            else
                return View("Login");
        }

        public async Task<IActionResult> LoginUser(Kullanici k)
        {
            if (_context.kullanicilar.FindAsync(k.id).Result.rol.Equals("antrenor") &&
                _context.kullanicilar.FindAsync(k.id).Result.sifre.Equals(k.sifre))
            {
                SharedClass.id = k.id;
                SharedClass.rol = k.rol;
                SharedClass.loggedin = true;
                Antrenor a = new Antrenor();
                   a=  _context.antrenorler.Find(k.id);

                return View("Details", model: a);
            }
            else if (_context.kullanicilar.FindAsync(k.id).Result.rol.Equals("admin") &&
                _context.kullanicilar.FindAsync(k.id).Result.sifre.Equals(k.sifre))
            {
                SharedClass.id = k.id;
                SharedClass.rol = k.rol;
                SharedClass.loggedin = true;
                return View();
            }
            else
                return BadRequest();
        }
        // GET: AntrenorUser
        public async Task<IActionResult> Index()
        {
              return _context.antrenorler != null ? 
                          View(await _context.antrenorler.ToListAsync()) :
                          Problem("Entity set 'YazlabDbContext.antrenorler'  is null.");
        }

        // GET: AntrenorUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.antrenorler == null)
            {
                return NotFound();
            }

            var antrenor = await _context.antrenorler
                .FirstOrDefaultAsync(m => m.id == id);
            if (antrenor == null)
            {
                return NotFound();
            }

            return View(antrenor);
        }

        // GET: AntrenorUser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AntrenorUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,adsoyad,uzmanliklar,deneyim,iletisim")] Antrenor antrenor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(antrenor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(antrenor);
        }
        public async Task<IActionResult> GetClients()
        {
            int id = SharedClass.id;
            List<int> e = _context.eslesmeler.Where(es=> es.antrenor_id == id)
                .Select(es=> es.danisan_id)
                .ToList();
            List<Danısan> eslesmis = new List<Danısan>();
            foreach(int i in e)
            {
                eslesmis.AddRange(_context.danisan.Where(e => e.id == i).ToList());
            }
            return View("Danisanlarim", eslesmis);
        }
        // GET: AntrenorUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.antrenorler == null)
            {
                return NotFound();
            }

            var antrenor = await _context.antrenorler.FindAsync(id);
            if (antrenor == null)
            {
                return NotFound();
            }
            return View(antrenor);
        }
        public IActionResult Login()
        {
            return View();
        }

        // POST: AntrenorUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,adsoyad,uzmanliklar,deneyim,iletisim")] Antrenor antrenor)
        {
            if (id != antrenor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(antrenor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AntrenorExists(antrenor.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(antrenor);
        }

        // GET: AntrenorUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.antrenorler == null)
            {
                return NotFound();
            }

            var antrenor = await _context.antrenorler
                .FirstOrDefaultAsync(m => m.id == id);
            if (antrenor == null)
            {
                return NotFound();
            }

            return View(antrenor);
        }
        public async Task<IActionResult> AntPlanUser(int? id)
        {
            id = SharedClass.id;
            List<WorkoutPlan> plans = _context.antrenmanplan
                .Where(e => e.id == id)
                .ToList();




            return View(plans);
        }
        public async Task<IActionResult> DietPlanUser(int? id)
        {
            id = SharedClass.id;
            List<int> e = _context.eslesmeler.Where(es => es.antrenor_id == id)
                .Select(es => es.danisan_id)
                .ToList();
            List<DietPlan> eslesmis = new List<DietPlan>();
            foreach (int i in e)
            {
                eslesmis.AddRange(_context.dietplan.Where(e => e.danisan_id == i).ToList());
            }
            


            return View("DietPlanUser1",eslesmis);
        }
        // POST: AntrenorUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.antrenorler == null)
            {
                return Problem("Entity set 'YazlabDbContext.antrenorler'  is null.");
            }
            var antrenor = await _context.antrenorler.FindAsync(id);
            if (antrenor != null)
            {
                _context.antrenorler.Remove(antrenor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CreateDietPlan ()
        {
            return View();
        }

        // POST: DietPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDietPlan([Bind("id,danisan_id,danisan_adsoyad,hedefler")] DietPlan dietPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dietPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(DietPlanUser));
            }
            return View(dietPlan);
        }

        // GET: DietPlan/Edit/5
        public async Task<IActionResult> EditDietPlan(int? id)
        {
            if (id == null || _context.dietplan == null)
            {
                return NotFound();
            }

            var dietPlan = await _context.dietplan.FindAsync(id);
            if (dietPlan == null)
            {
                return NotFound();
            }
            return View(dietPlan);
        }

        // POST: DietPlan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editdp(int id, [Bind("id,danisan_id,danisan_adsoyad,hedefler")] DietPlan dietPlan)
        {
            if (id != dietPlan.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dietPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DietPlanExists(dietPlan.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(DietPlanUser));
            }
            return View(dietPlan);
        }

        // GET: DietPlan/Delete/5
        public async Task<IActionResult> DeleteDietPlan(int? id)
        {
            if (id == null || _context.dietplan == null)
            {
                return NotFound();
            }

            var dietPlan = await _context.dietplan
                .FirstOrDefaultAsync(m => m.id == id);
            if (dietPlan == null)
            {
                return NotFound();
            }

            return View(dietPlan);
        }

        // POST: DietPlan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDietPlanConfirmed(int id)
        {

            if (_context.dietplan == null)
            {
                return Problem("Entity set 'YazlabDbContext.dietplan'  is null.");
            }
            var dietPlan = await _context.dietplan.FindAsync(id);
            if (dietPlan != null)
            {
                _context.dietplan.Remove(dietPlan);
            }

            await _context.SaveChangesAsync();
            return View("DietPlanUser");
        }

        private bool DietPlanExists(int id)
        {
            return (_context.dietplan?.Any(e => e.id == id)).GetValueOrDefault();
        }
    

    private bool AntrenorExists(int id)
        {
          return (_context.antrenorler?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
