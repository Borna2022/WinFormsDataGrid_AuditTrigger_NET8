using Microsoft.EntityFrameworkCore;
using WinFormsApp2.Models; // اگر مدل‌ها در پوشه Models باشن

namespace WinFormsApp2.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<MixedZoneSteelGroup> MixedZoneSteelGroups { get; set; } = null!;
        public DbSet<TmixedZoneSteelGroupAudit> TMixedZoneSteelGroupAudit { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Test;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MixedZoneSteelGroup>()
                .HasKey(m => new { m.Fsg_GroupId, m.Fsg_Row });

            modelBuilder.Entity<MixedZoneSteelGroup>()
                .Property(p => p.Fsg_GroupId).ValueGeneratedNever();
            modelBuilder.Entity<MixedZoneSteelGroup>()
                .Property(p => p.Fsg_Row).ValueGeneratedNever();
            modelBuilder.Entity<MixedZoneSteelGroup>()
                .Property(p => p.Fsg_MixedZoneSteelGroup).ValueGeneratedNever();
        }
    }


}

