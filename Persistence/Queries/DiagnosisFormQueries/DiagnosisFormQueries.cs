using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Queries.DiagnosisFormQueries
{
    public class DiagnosisFormQueries
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<DiagnosisFormQueries> _logger;
        private readonly IConfiguration _configuration;

        public DiagnosisFormQueries(ILogger<DiagnosisFormQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }
    }
}
