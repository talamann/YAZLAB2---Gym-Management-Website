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
    public class DietPlanController : Controller
    {
        private readonly YazlabDbContext _context;

        public DietPlanController(YazlabDbContext context)
        {
            _context = context;
        }

        // GET: DietPlan
        public async Task<IActionResult> Index()
        {
              return _context.dietplan != null ? 
                          View(await _context.dietplan.ToListAsync()) :
                          Problem("Entity set 'YazlabDbContext.dietplan'  is null.");
        }

        // GET: DietPlan/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: DietPlan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DietPlan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,danisan_id,danisan_adsoyad,hedefler")] DietPlan dietPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dietPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dietPlan);
        }

        // GET: DietPlan/Edit/5
        public async Task<IActionResult> Edit(int? plan_id)
        {
            if (plan_id == null || _context.dietplan == null)
            {
                return NotFound();
            }

            var dietPlan = await _context.dietplan.FindAsync(plan_id);
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
        public async Task<IActionResult> Edit(int plan_id, [Bind("id,danisan_id,danisan_adsoyad,hedefler")] DietPlan dietPlan)
        {
            if (plan_id != dietPlan.id)
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
                return RedirectToAction(nameof(Index));
            }
            return View(dietPlan);
        }

        // GET: DietPlan/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool DietPlanExists(int id)
        {
          return (_context.dietplan?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
