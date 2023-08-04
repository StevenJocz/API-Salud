using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.IlnessesDTOs;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Queries.IllnessesQueries
{
    public interface IIllnessesQueries
    {
        Task<List<IllnessesDTOs>> ShowIllnessesAsync();
    }
    public class IllnessesQueries : IIllnessesQueries
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<IllnessesQueries> _logger;
        private readonly IConfiguration _configuration;

        public IllnessesQueries(ILogger<IllnessesQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        public async Task<List<IllnessesDTOs>> ShowIllnessesAsync()
        {
            _logger.LogInformation("Start IllnessesQueries.ShowIllnesses");
            List<IllnessesDTOs> FindAllIllnesses;
            try
            {
                FindAllIllnesses = await _context.IllnessesEs.Select(
                    x => new IllnessesDTOs 
                    {
                        Id = Convert.ToString(x.id),
                        illness_name = x.s_illness_name
                    }
                ).ToListAsync();
                return FindAllIllnesses;
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error en el metodo IllnessesQueries.ShowIllnesses...");
                throw ex;
            }
        }

    }
}
