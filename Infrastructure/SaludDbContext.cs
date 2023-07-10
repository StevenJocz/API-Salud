using Microsoft.EntityFrameworkCore;
using UNAC.AppSalud.Domain.Entities.DiagnosisFormE;
using UNAC.AppSalud.Domain.Entities.DiagnosticFormE;
using UNAC.AppSalud.Domain.Entities.LoginE;

namespace UNAC.AppSalud.Infrastructure
{
    public class SaludDbContext : DbContext
    {
        private readonly string _connection;

        public SaludDbContext(string connection)
        {
            _connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<LoginE> LoginEs { get; set; }
        public virtual DbSet<HistorialrefreshtokenE> HistorialrefreshtokenEs { get; set; }
        public virtual DbSet<CodigoRestablecimientoE> CodigoRestablecimientoEs { get; set; }
        public virtual DbSet<DiagnosisFormAnswers> DiagnosisFormAnswers { get; set; }
        public virtual DbSet<DiagnosisFormE> DiagnosisFormE { get; set; }

    }
}