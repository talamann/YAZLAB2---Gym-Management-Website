using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YAZLAB2;
using YAZLAB2.Models;

namespace YAZLAB2.Controllers
{
    public class kullanicilarController : Controller
    {
        private readonly YazlabDbContext _context;

        public kullanicilarController(YazlabDbContext context)
        {
            _context = context;
        }

        // GET: kullanicilar
        public async Task<IActionResult> Index()
        {
              return _context.kullanicilar != null ? 
                          View(await _context.kullanicilar.ToListAsync()) :
                          Problem("Entity set 'YazlabDbContext.kullanicilar'  is null.");
        }

        // GET: kullanicilar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.kullanicilar == null)
            {
                return NotFound();
            }

            var kullanici = await _context.kullanicilar
                .FirstOrDefaultAsync(m => m.id == id);
            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        // GET: kullanicilar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: kullanicilar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,eposta,adsoyad,sifre,rol")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kullanici);
                await _context.SaveChangesAsync();
                if (kullanici.rol.Equals("danisan"))
                {
                    Danısan d = new Danısan();
                    d.id = kullanici.id;
                    d.adsoyad = kullanici.adsoyad;
                    _context.Add(d);
                    await _context.SaveChangesAsync();
                }
                else if (kullanici.rol.Equals("antrenor"))
                {
                    Antrenor a = new Antrenor();
                    a.id = kullanici.id;
                    a.adsoyad = kullanici.adsoyad;
                    _context.Add(a);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(kullanici);
        }

        // GET: kullanicilar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.kullanicilar == null)
            {
                return NotFound();
            }

            var kullanici = await _context.kullanicilar.FindAsync(id);
            if (kullanici == null)
            {
                return NotFound();
            }
            return View(kullanici);
        }

        // POST: kullanicilar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,eposta,adsoyad,sifre,rol")] Kullanici kullanici)
        {
            if (id != kullanici.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kullanici);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KullaniciExists(kullanici.id))
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
            return View(kullanici);
        }

        // GET: kullanicilar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.kullanicilar == null)
            {
                return NotFound();
            }

            var kullanici = await _context.kullanicilar
                .FirstOrDefaultAsync(m => m.id == id);
            if (kullanici == null)
            {
                return NotFound();
            }

            return View(kullanici);
        }

        // POST: kullanicilar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.kullanicilar == null)
            {
                return Problem("Entity set 'YazlabDbContext.kullanicilar'  is null.");
            }
            var kullanici = await _context.kullanicilar.FindAsync(id);
            if (kullanici != null)
            {
                _context.kullanicilar.Remove(kullanici);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KullaniciExists(int id)
        {
          return (_context.kullanicilar?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
