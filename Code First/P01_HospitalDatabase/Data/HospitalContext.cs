using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<PatientMedicament> Prescriptions { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(p =>
            {
                p.Property(fn => fn.FirstName).HasColumnType("nvarchar(50)");
                p.Property(ln => ln.LastName).HasColumnType("nvarchar(50)");
                p.Property(a => a.Address).HasColumnType("nvarchar(250)");
                p.Property(e => e.Email).HasColumnType("varchar(80)");
                p.Property(i => i.HasInsurance).HasColumnType("bit");

            });

            modelBuilder.Entity<Visitation>()
                .Property(p => p.Comments).HasColumnType("nvarchar(250)");

            modelBuilder.Entity<Diagnose>(d =>
            {
                d.Property(n => n.Name).HasColumnType("nvarchar(50)");
                d.Property(c => c.Comments).HasColumnType("nvarchar(250)");
            });

            modelBuilder.Entity<Medicament>()
                .Property(m => m.Name).HasColumnType("nvarchar(50)");

            modelBuilder.Entity<Doctor>(d =>
            {
                d.Property(n => n.Name).HasColumnType("nvarchar(100)");
                d.Property(s => s.Specialty).HasColumnType("nvarchar(100)");
            });

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Visitations)
                .WithOne(v => v.Patient)
                .HasForeignKey(p => p.PatientId);

            modelBuilder.Entity<Patient>()
                .HasMany(p => p.Diagnoses)
                .WithOne(d => d.Patient)
                .HasForeignKey(p => p.PatientId);

            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Visitations)
                .WithOne(v => v.Doctor)
                .HasForeignKey(d => d.DoctorId);

            modelBuilder.Entity<PatientMedicament>()
                .HasKey(pm => new
                {
                    pm.MedicamentId,
                    pm.PatientId
                });

        }
    }
}
