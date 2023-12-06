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
    public class AntrenorlerController : Controller
    {
        private readonly YazlabDbContext _context;

        public AntrenorlerController(YazlabDbContext context)
        {
            _context = context;
        }

        // GET: Antrenorler
        public async Task<IActionResult> Index()
        {
              return _context.antrenorler != null ? 
                          View(await _context.antrenorler.ToListAsync()) :
                          Problem("Entity set 'YazlabDbContext.antrenorler'  is null.");
        }

        // GET: Antrenorler/Details/5
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

        // GET: Antrenorler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Antrenorler/Create
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

        // GET: Antrenorler/Edit/5
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

        // POST: Antrenorler/Edit/5
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

        // GET: Antrenorler/Delete/5
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

        // POST: Antrenorler/Delete/5
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

        private bool AntrenorExists(int id)
        {
          return (_context.antrenorler?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
