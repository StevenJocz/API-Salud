using Microsoft.EntityFrameworkCore;
using UNAC.AppSalud.Domain.Entities.AnswersBankE;
using UNAC.AppSalud.Domain.Entities.DiagnosisFormAnswersE;
using UNAC.AppSalud.Domain.Entities.DiagnosticFormE;
using UNAC.AppSalud.Domain.Entities.IllnessesE;
using UNAC.AppSalud.Domain.Entities.LocationE;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Domain.Entities.MedicinesE;
using UNAC.AppSalud.Domain.Entities.MedicinesRegistrationE;
using UNAC.AppSalud.Domain.Entities.QuestionsBankE;
using UNAC.AppSalud.Domain.Entities.UserE;

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

        // Login
        public virtual DbSet<LoginE> LoginEs { get; set; }
        public virtual DbSet<HistorialrefreshtokenE> HistorialrefreshtokenEs { get; set; }
        public virtual DbSet<CodigoRestablecimientoE> CodigoRestablecimientoEs { get; set; }
        public virtual DbSet<DiagnosisFormAnswersE> DiagnosisFormAnswersE { get; set; }
        public virtual DbSet<DiagnosisFormE> DiagnosisFormE { get; set; }
        public virtual DbSet<MedicinesRegistrationE> MedicinesRegistrationE { get; set; }
        public virtual DbSet<UserE> UserEs { get; set; }
        public virtual DbSet<MedicinesE> MedicinesE { get; set; }
        public virtual DbSet<IllnessesE> IllnessesEs { get; set; }
        public virtual DbSet<QuestionsBankE> QuestionsBankE { get; set; }
        public virtual DbSet<AnswersBankE> AnswersBankE { get; set; }
        public virtual DbSet<CountriesE> CountriesEs { get; set; }
        public virtual DbSet<StatesE> StatesEs { get; set; }
        public virtual DbSet<CitiesE> CitiesEs { get; set; }
    }
}