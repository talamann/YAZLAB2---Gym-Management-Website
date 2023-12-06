using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Npgsql;
using Microsoft.EntityFrameworkCore;

using YAZLAB2.Models;
using Npgsql.Internal;
using System.Data;
using System.Collections.Immutable;

namespace YAZLAB2.Controllers
{
    public class DanisanUserController : Controller
    {
        
        private readonly YazlabDbContext _context;

        public DanisanUserController(YazlabDbContext context)
        {
           
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        // GET: DanisanUser
        public async Task<IActionResult> Index(int? id)
        {
            id = SharedClass.id;
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

            return View("DanisanIndex",danısan);
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
        public async Task<IActionResult> LoginUser(Kullanici k)
        {
            if (_context.kullanicilar.FindAsync(k.id).Result.rol.Equals("danisan")&&
                _context.kullanicilar.FindAsync(k.id).Result.sifre.Equals(k.sifre))
            {
                SharedClass.id = k.id;
                SharedClass.rol = k.rol;
                SharedClass.loggedin = true;
                Danısan d = _context.danisan.Find(k.id);

                return View("DanisanIndex", model: d);
            }
            else if (_context.kullanicilar.FindAsync(k.id).Result.rol.Equals("admin")&&
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




        // GET: DanisanUser/Edit/5

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
            return View();
        }

        // POST: DanisanUser/Edit/5
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
                return View("DanisanIndex");
            }
            return View("DanisanIndex");
        }
        public IActionResult match(int? id)
        {
            string connectionString = "Host=localhost; Database=postgres; Username=postgres; Password=Talha2003";
            id = SharedClass.id;
            List<Antrenor> a = new List<Antrenor>();
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string Query = "SELECT d.id, d.adsoyad, a.id, a.adsoyad, a.uzmanliklar\r\nFROM danisan d\r\nINNER JOIN antrenorler a ON d.hedefler = a.uzmanliklar\r\nWHERE NOT EXISTS (\r\n    SELECT 1\r\n    FROM eslesmeler e\r\n    WHERE e.danisan_id = d.id AND e.antrenor_id = a.id\r\n) AND d.id =" + id;

                using (NpgsqlCommand command = new NpgsqlCommand(Query, connection))
                {
                 using(NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Antrenor an = new Antrenor();
                            an.id = reader.GetInt32(2);
                            an.adsoyad=reader.GetString(3);
                            an.uzmanliklar = reader.GetString(4);
                            a.Add(an);
                        }
                    }
                }
                connection.Close();
            }
            return View("Eslesmeler", a);
        }
        public IActionResult egitmenlerim(int? id)
        {
            id = SharedClass.id;
            List<Eslesmeler> a = _context.eslesmeler.Where(e=>e.danisan_id==id).ToList();
            return View(a);

        }
        public async Task<IActionResult> eslestir(int id)
        {
            var an = await _context.antrenorler
                .FirstOrDefaultAsync(m => m.id == id);


            string connectionString = "Host=localhost; Database=postgres; Username=postgres; Password=Talha2003";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                Danısan d = _context.danisan.Find(SharedClass.id);
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO eslesmeler VALUES (@SharedId, @DanisanAdSoyad, @AntrenorId, @AntrenorAdSoyad, @Uzmanlik);", connection))
                {
                    command.Parameters.AddWithValue("@SharedId", SharedClass.id);
                    command.Parameters.AddWithValue("@DanisanAdSoyad", d.adsoyad);
                    command.Parameters.AddWithValue("@AntrenorId", id);
                    command.Parameters.AddWithValue("@AntrenorAdSoyad", an.adsoyad );
                    command.Parameters.AddWithValue("@Uzmanlik", an.uzmanliklar);
 

                    // Execute the command
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            return View("DanisanIndex");
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
            List<DietPlan> plans = _context.dietplan
                .Where(e => e.danisan_id == id)
                .ToList();




            return View(plans);
        }
        private bool DanısanExists(int id)
        {
          return (_context.danisan?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
