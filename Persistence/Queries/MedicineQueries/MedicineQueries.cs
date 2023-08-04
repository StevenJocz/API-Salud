using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.MedicineDTOs;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Queries.MedicineQueries
{

    public interface IMedicineQueries
    {
        Task<List<MedicineListDTOs>> ShowMedicinesAsync();
    }
    public class MedicineQueries : IMedicineQueries
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<MedicineQueries> _logger;
        private readonly IConfiguration _configuration;

        public MedicineQueries(ILogger<MedicineQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        public async Task<List<MedicineListDTOs>> ShowMedicinesAsync()
        {
            _logger.LogInformation("Start MedicineQueries.ShowMedicines");
            List<MedicineListDTOs> FindAllMedicines;
            try
            {
                 FindAllMedicines = await _context.MedicinesE.Join(
                        _context.IllnessesEs,
                        Medicine => Medicine.fk_tblillnesses,
                        Illness => Illness.id,
                        (Medicine, Illness) => new { MedicinesE = Medicine, IllnessesE = Illness }
                 ).Select(Result => new MedicineListDTOs
                 {
                     id_medicine = Convert.ToString(Result.MedicinesE.id),
                     medicine_name = Result.MedicinesE.s_medicine_name,
                     medicine_description = Result.MedicinesE.s_medicine_description,
                     medicine_indications = Result.MedicinesE.s_medicine_indications,
                     name_illness = Result.IllnessesE.s_illness_name
                 }).ToListAsync();

                return FindAllMedicines;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineQueries.ShowMedicines...");
                throw ex;
            }
        }
    }
}
