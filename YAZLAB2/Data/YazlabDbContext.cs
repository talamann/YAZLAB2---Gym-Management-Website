using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using YAZLAB2.Models;
namespace YAZLAB2
{
    public class YazlabDbContext:DbContext
    {
        public YazlabDbContext(DbContextOptions<YazlabDbContext> options) : base(options) { }
        public DbSet<Antrenor> antrenorler { get; set; }
        public DbSet<Danısan>  danisan { get; set; }
       
        public DbSet<DietPlan> dietplan { get; set; }
        public DbSet<WorkoutPlan> antrenmanplan { get; set; }
        public DbSet<Kullanici> kullanicilar { get; set; }
        public DbSet<Eslesmeler> eslesmeler { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kullanici>()
                .HasKey(e => new { e.id});
            base.OnModelCreating(modelBuilder);
        }

    }
}
