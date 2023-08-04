using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;
using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
using UNAC.AppSalud.Domain.DTOs.MedicineDTOs;
using UNAC.AppSalud.Domain.Entities.MedicinesE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Commands.MedicineCommands
{
    public interface IMedicineCommands
    {
        Task<AnswersErrorDTOs> SaveNewMedicineAsync(MedicineDTOs Medicine);
        Task<AnswersErrorDTOs> ChangeXMedicineAsync(MedicineDTOs Medicine);
        Task<AnswersErrorDTOs> DeleteMedicineAsync(int IdMedicine);
    }
    public class MedicineCommands : IMedicineCommands
    {

        private readonly SaludDbContext _context = null;
        private readonly ILogger<MedicineCommands> _logger;
        private readonly IConfiguration _configuration;

        public MedicineCommands(ILogger<MedicineCommands> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        private async Task<bool> SaveMedicine(MedicineDTOs Medicine)
        {
            try
            {
                var SaveMedicine = new MedicinesE
                {
                    s_medicine_name=Medicine.medicine_name,
                    s_medicine_description=Medicine.medicine_description,
                    s_medicine_indications=Medicine.medicine_indications,
                    fk_tblillnesses=Convert.ToInt32(Medicine.id_ilnness)
                };

                await _context.MedicinesE.AddAsync(SaveMedicine);
                await _context.SaveChangesAsync();

                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> ChangeMedicineFeatures(MedicineDTOs Medicine)
        {
            try
            {
                var CheckMedicine = await _context.MedicinesE.FirstOrDefaultAsync(x => x.id == Convert.ToInt32(Medicine.id_medicine));
                if (CheckMedicine != null)
                {
                    CheckMedicine.s_medicine_name = Medicine.medicine_name;
                    CheckMedicine.s_medicine_description = Medicine.medicine_description;
                    CheckMedicine.s_medicine_indications = Medicine.medicine_indications;
                    CheckMedicine.fk_tblillnesses = Convert.ToInt32(Medicine.id_ilnness);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> DeleteXMedicine(int IdMedicine)
        {
            try
            {
                var FindMedicine = await _context.MedicinesE.FirstOrDefaultAsync(x => x.id == IdMedicine);
                if(FindMedicine != null)
                {
                    _context.MedicinesE.Remove(FindMedicine);
                    await _context.SaveChangesAsync();
                }
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<AnswersErrorDTOs> SaveNewMedicineAsync(MedicineDTOs Medicine)
        {
            _logger.LogInformation("Start MedicineCommands.SaveNewMedicineAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var ExistIllness = await _context.IllnessesEs.Where(x => x.id.Equals(Convert.ToInt32(Medicine.id_ilnness))).FirstOrDefaultAsync();
                if (ExistIllness != null) {
                    var SaveNewMedicine = await SaveMedicine(Medicine);
                    StateInformation = SaveNewMedicine;
                    DescriptionStateInformation = SaveNewMedicine == true ? "Se guardo la información satisfactoriamente" : "No se guardo la medicina, intentélo nuevamente.";
                }
                else {
                    StateInformation = false;
                    DescriptionStateInformation = "El enfermedad no existe, creelo e intentélo nuevamente.";
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.SaveNewMedicineAsync...");
                StateInformation = false;
                DescriptionStateInformation = "Tenemos problemas al guardar la información, intentélo nuevamente.";
            }
            AnswersErrorDTOs ReponseConsult = new AnswersErrorDTOs
            {
                stateError = StateInformation,
                descriptionError = DescriptionStateInformation
            };

            return ReponseConsult;
        }

        public async Task<AnswersErrorDTOs> ChangeXMedicineAsync(MedicineDTOs Medicine)
        {
            _logger.LogInformation("Start MedicineCommands.ChangeXMedicineAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var ExistIllness = _context.IllnessesEs.Where(x => x.id.Equals(Convert.ToInt32(Medicine.id_ilnness))).FirstOrDefaultAsync();
                if (ExistIllness != null)
                {
                    var ChangeMedicine = await ChangeMedicineFeatures(Medicine);
                    StateInformation = ChangeMedicine;
                    DescriptionStateInformation = ChangeMedicine == true ? "Se guardo la información satisfactoriamente." : "No se guardo la medicina, intentélo nuevamente.";
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "la enfermedad no existe, creelo e intentélo nuevamente.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.ChangeXMedicineAsync...");
                StateInformation = false;
                DescriptionStateInformation = "Tenemos problemas al guardar la información, intentélo nuevamente.";
            }

            AnswersErrorDTOs ReponseConsult = new AnswersErrorDTOs
            {
                stateError = StateInformation,
                descriptionError = DescriptionStateInformation
            };

            return ReponseConsult;
        }

        public async Task<AnswersErrorDTOs> DeleteMedicineAsync(int IdMedicine)
        {
            _logger.LogInformation("Start MedicineCommands.DeleteMedicineAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var VerifyDontUseMedicine = await _context.MedicinesRegistrationE.Where(x => x.fk_tblmedicines.Equals(IdMedicine)).FirstOrDefaultAsync();
                if (VerifyDontUseMedicine == null)
                {
                    var ChangeMedicine = await DeleteXMedicine(IdMedicine);
                    StateInformation = ChangeMedicine;
                    DescriptionStateInformation = ChangeMedicine == true ? "Se guardo la información satisfactoriamente." : "No se guardo la medicina, intentélo nuevamente.";
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.DeleteMedicineAsync...");
                StateInformation = false;
                DescriptionStateInformation = "Tenemos problemas al guardar la información, intentélo nuevamente.";
            }

            AnswersErrorDTOs ReponseConsult = new AnswersErrorDTOs
            {
                stateError = StateInformation,
                descriptionError = DescriptionStateInformation
            };

            return ReponseConsult;
        }
    }
}
