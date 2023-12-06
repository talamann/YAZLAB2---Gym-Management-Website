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
    public class danisanlarController : Controller
    {
        private readonly YazlabDbContext _context;

        public danisanlarController(YazlabDbContext context)
        {
            _context = context;
        }

        // GET: danisanlar
        public async Task<IActionResult> Index()
        {
              return _context.danisan != null ? 
                          View(await _context.danisan.ToListAsync()) :
                          Problem("Entity set 'YazlabDbContext.danisan'  is null.");
        }

        // GET: danisanlar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.danisan == null)
            {
                return NotFound();
            }

            var danısan = await _context.danisan
                .FirstOrDefaultAsync(m => m.id == id);
            if (danısan == null)
            {
                return NotFound();
            }

            return View(danısan);
        }

        // GET: danisanlar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: danisanlar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,adsoyad,hedefler,boy,kilo,bmi,yagorani")] Danısan danısan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(danısan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(danısan);
        }

        // GET: danisanlar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.danisan == null)
            {
                return NotFound();
            }

            var danısan = await _context.danisan.FindAsync(id);
            if (danısan == null)
            {
                return NotFound();
            }
            return View(danısan);
        }

        // POST: danisanlar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,adsoyad,hedefler,boy,kilo,bmi,yagorani")] Danısan danısan)
        {
            if (id != danısan.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(danısan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DanısanExists(danısan.id))
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
            return View(danısan);
        }

        // GET: danisanlar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.danisan == null)
            {
                return NotFound();
            }

            var danısan = await _context.danisan
                .FirstOrDefaultAsync(m => m.id == id);
            if (danısan == null)
            {
                return NotFound();
            }

            return View(danısan);
        }

        // POST: danisanlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.danisan == null)
            {
                return Problem("Entity set 'YazlabDbContext.danisan'  is null.");
            }
            var danısan = await _context.danisan.FindAsync(id);
            if (danısan != null)
            {
                _context.danisan.Remove(danısan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DanısanExists(int id)
        {
          return (_context.danisan?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
