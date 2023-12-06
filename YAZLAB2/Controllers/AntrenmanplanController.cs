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
    public class AntrenmanplanController : Controller
    {
        private readonly YazlabDbContext _context;

        public AntrenmanplanController(YazlabDbContext context)
        {
            _context = context;
        }

        // GET: Antrenmanplan
        public async Task<IActionResult> Index()
        {
              return _context.antrenmanplan != null ? 
                          View(await _context.antrenmanplan.ToListAsync()) :
                          Problem("Entity set 'YazlabDbContext.antrenmanplan'  is null.");
        }

        // GET: Antrenmanplan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.antrenmanplan == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.antrenmanplan
                .FirstOrDefaultAsync(m => m.id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // GET: Antrenmanplan/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Antrenmanplan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,danisan_adsoyad,hedefler")] WorkoutPlan workoutPlan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workoutPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workoutPlan);
        }

        // GET: Antrenmanplan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.antrenmanplan == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.antrenmanplan.FindAsync(id);
            if (workoutPlan == null)
            {
                return NotFound();
            }
            return View(workoutPlan);
        }

        // POST: Antrenmanplan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,danisan_adsoyad,hedefler")] WorkoutPlan workoutPlan)
        {
            if (id != workoutPlan.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutPlanExists(workoutPlan.id))
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
            return View(workoutPlan);
        }

        // GET: Antrenmanplan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.antrenmanplan == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.antrenmanplan
                .FirstOrDefaultAsync(m => m.id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // POST: Antrenmanplan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.antrenmanplan == null)
            {
                return Problem("Entity set 'YazlabDbContext.antrenmanplan'  is null.");
            }
            var workoutPlan = await _context.antrenmanplan.FindAsync(id);
            if (workoutPlan != null)
            {
                _context.antrenmanplan.Remove(workoutPlan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutPlanExists(int id)
        {
          return (_context.antrenmanplan?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
